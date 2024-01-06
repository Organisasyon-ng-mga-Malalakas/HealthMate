using Fody;
using HealthMate.Extensions;
using HealthMate.Models.Tables;
using MongoDB.Bson;
using System.Net.Http.Json;
using System.Text.Json;

namespace HealthMate.Services;

public class UserService(IPreferences preferences, HttpClient httpClient, RealmService realmService)
{
	public async Task<User> GetLoggedUser()
	{
		var allUsers = await realmService.FindAll<User>();
		var foundUser = allUsers.FirstOrDefault() is User user ? user : null;
		preferences.Set("HasUser", foundUser != null);
		preferences.Set("RemoteUserId", foundUser.RemoteUserId ?? "");
		return foundUser;
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
				preferences.Set("RemoteUserId", returnedUser.RemoteUserId);
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
					preferences.Set("RemoteUserId", fetchedUserId);
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
}

// Best practices for HttpClient: https://bytedev.medium.com/net-core-httpclient-best-practices-4c1b20e32c6
/*
 System.InvalidOperationException: 'Each parameter in the deserialization constructor on type 'HealthMate.Models.Tables.ScheduleRemote' must bind to an object property or field on deserialization.
Each parameter name must match with a property or field on the object. Fields are only considered when 'JsonSerializerOptions.IncludeFields' is enabled. The match can be case-insensitive.'
 */