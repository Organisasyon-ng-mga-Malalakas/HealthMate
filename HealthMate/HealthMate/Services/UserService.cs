using Android.Service.Autofill;
using Fody;
using HealthMate.Extensions;
using HealthMate.Models.Tables;
using Realms;
using System.Net.Http.Json;

namespace HealthMate.Services;

public class UserService(HttpClient httpClient, RealmService realmService)
{
	private User? _loggedUser;

	public async Task<User> GetLoggedUser()
	{
		return _loggedUser ??= (await realmService.FindAll<User>()).FirstOrDefault();
	}

	[ConfigureAwait(false)]
	public async Task<string> Signup(User user)
	{
		try
		{
			var userData = new
			{
				username = user.Username,
				email = user.Email,
				birthdate = user.Birthdate,
				gender = user.Gender,
				password = user.Password
			};

			var response = await httpClient.SendAsync(new HttpRequestMessage
			{
				Content = userData.AsJSONSerializedObject(),
				Method = HttpMethod.Post,
				RequestUri = new Uri("/user/", UriKind.Relative)
			}, HttpCompletionOption.ResponseHeadersRead);

			if (response.IsSuccessStatusCode)
			{
				var returnedUser = await response.Content.ReadFromJsonAsync<User>();
				var id = returnedUser.RemoteUserId.ToString(); // may error pag inassign directly: "object reference not set to an instance of an object"
				user.RemoteUserId = id;
				await realmService.Upsert(user);

				return "Success";
			}
			else
			{
				var errorDetails = await response.Content.ReadFromJsonAsync <Dictionary<string, string>>();
				return errorDetails["detail"];
			}
				
		}
		catch (Exception ex)
		{
			return $"Exception occured. {ex}";
		}
	}

	[ConfigureAwait(false)]
	public async Task<string> Login(string username, string password)
	{
		try
		{ 
			var userData = new { username, password };

			var response = await httpClient.SendAsync(new HttpRequestMessage
			{
				Content = userData.AsJSONSerializedObject(),
				Method = HttpMethod.Post,
				RequestUri = new Uri("/user/login/", UriKind.Relative)
			}, HttpCompletionOption.ResponseHeadersRead);

			if (response.IsSuccessStatusCode)
			{
				var auth = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
				if (auth.TryGetValue("token", out string token))
				{
					// Load user data
					// TODO: add user details on login response.
				}

				return "Unable to login.";
			}
			else
			{
				var errorDetails = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
				return errorDetails["detail"];
			}

		}	
		catch (Exception ex)
		{
			return $"Exception occured. {ex}";
		}
	}

	[ConfigureAwait(false)]
	public async Task<string> SaveUserQuestions(User user)
	{
		if (user == null) return "No logged in user.";

		if (user.Questionnaires == null) return "No questions yet.";

		var questions = user.Realm.All<Questionnaires>()
			.Where(q => q.UserId == user.RemoteUserId);

		//var questions = await realmService.Find<Questionnaires>(q => q.UserId == user.RemoteUserId); Error: Realm accessed in incorrect thread

		try
		{
			List<object> questionsArr = new List<object>();
			foreach (var q in questions)
			{
				questionsArr.Add(new { 
					name = q.Key,
					value = q.Value,
					category = q.Category,
				});
			}
			var questionsData = new
			{
				user_id = user.RemoteUserId,
				questions = questionsArr
			};

			var response = await httpClient.SendAsync(new HttpRequestMessage
			{
				Content = questionsData.AsJSONSerializedObject(),
				Method = HttpMethod.Post,
				RequestUri = new Uri("/questions/", UriKind.Relative)
			}, HttpCompletionOption.ResponseHeadersRead);

			if (response.IsSuccessStatusCode)
			{
				return "Success";
			}
			else
			{
				var errorDetails = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
				return errorDetails["detail"];
			}
				
		}
		catch (Exception ex)
		{
			return $"Exception occured. {ex}";
		}
	}
}


// Best practices for HttpClient: https://bytedev.medium.com/net-core-httpclient-best-practices-4c1b20e32c6