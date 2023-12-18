using Fody;
using HealthMate.Extensions;
using HealthMate.Models.Tables;
using MongoDB.Bson;
using System.Net.Http.Json;
using System.Text.Json;

namespace HealthMate.Services;

public class UserService(HttpClient httpClient, RealmService realmService)
{
	public async Task<User> GetLoggedUser()
	{
		var allUsers = await realmService.FindAll<User>();
		return allUsers.FirstOrDefault() is User user ? user : null;
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
				var responseStream = await response.Content.ReadAsStreamAsync();
				var returnedUser = responseStream.DeserializeStream<User>();
				user.RemoteUserId = returnedUser.RemoteUserId;
				await realmService.Upsert(user);

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
				RequestUri = new Uri("/user/login", UriKind.Relative)
			}, HttpCompletionOption.ResponseHeadersRead);

			if (response.IsSuccessStatusCode)
			{
				var stream = await response.Content.ReadAsStreamAsync();
				var dictionary = await stream.DeserializeStreamAsync<Dictionary<string, object>>();
				if (dictionary.TryGetValue("data", out var data) && data is JsonElement dataElement && dataElement.ValueKind == JsonValueKind.Object)
				{
					var fetchedUserId = dataElement.GetProperty("id").GetString();
					var fetchedUsername = dataElement.GetProperty("username").GetString();
					var fetchedGender = dataElement.GetProperty("gender").GetString();
					var fetchedEmail = dataElement.GetProperty("email").GetString();
					var fetchedBdate = dataElement.GetProperty("birthdate").GetDateTime();

					var newUser = new User
					{
						Birthdate = fetchedBdate,
						Email = fetchedEmail,
						Gender = fetchedGender,
						LocalUserId = ObjectId.GenerateNewId(),
						RemoteUserId = fetchedUserId,
						Username = fetchedUsername
					};
					await realmService.Upsert(newUser);

					return "Success.";
				}
				else
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

	public async Task UpsertSchedule(IEnumerable<ScheduleRemote> schedule)
	{
		var loggedUser = await GetLoggedUser();
		var content = new
		{
			user_id = loggedUser.RemoteUserId,
			schedules = schedule
		};

		var response = await httpClient.SendAsync(new HttpRequestMessage
		{
			Content = content.AsJSONSerializedObject(),
			Method = HttpMethod.Post,
			RequestUri = new Uri("/schedule/", UriKind.Relative)
		}, HttpCompletionOption.ResponseHeadersRead);

		if (response.IsSuccessStatusCode)
		{
			var stream = await response.Content.ReadAsStreamAsync();
			var dictionary = stream.NewtonsoftDeserializeStream<Dictionary<string, object>>();
		}
		else
			return;
	}
}

// Best practices for HttpClient: https://bytedev.medium.com/net-core-httpclient-best-practices-4c1b20e32c6