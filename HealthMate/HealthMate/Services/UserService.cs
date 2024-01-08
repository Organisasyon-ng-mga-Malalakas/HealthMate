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
		var foundUser = (await realmService.FindAll<User>()).FirstOrDefault();
		preferences.Set("HasUser", foundUser != null);
		preferences.Set("Avatar", foundUser != null ? $"{foundUser.Gender.ToLowerInvariant()}0{Random.Shared.Next(7)}" : "male01");
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

	public async Task DeleteLocalDatabase()
	{
		preferences.Set("HasUser", false);
		await realmService.DeleteAll<Inventory>();
		await realmService.DeleteAll<User>();
		await realmService.DeleteAll<Schedule>();
	}

	public async Task<bool> DeleteAccount()
	{
		var user = await GetLoggedUser();
		var response = await httpClient.SendAsync(new HttpRequestMessage
		{
			Method = HttpMethod.Delete,
			RequestUri = new Uri($"/user/{user.Username}", UriKind.Relative)
		}, HttpCompletionOption.ResponseHeadersRead);

		var isDeleted = response.IsSuccessStatusCode;
		if (isDeleted)
			await DeleteLocalDatabase();

		return isDeleted;
	}

	[ConfigureAwait(false)]
	public async Task<bool> Login(string username, string password)
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

					return true;
				}
				else
					return false;
			}
			else
				//var errorDetails = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
				//return errorDetails["detail"];
				return false;
		}
		catch (Exception ex)
		{
			return false;
		}
	}
}

// Best practices for HttpClient: https://bytedev.medium.com/net-core-httpclient-best-practices-4c1b20e32c6