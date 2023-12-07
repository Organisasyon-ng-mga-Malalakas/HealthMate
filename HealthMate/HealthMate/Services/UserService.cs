using Fody;
using HealthMate.Extensions;
using HealthMate.Models.Tables;

namespace HealthMate.Services;

public class UserService(HttpClient httpClient, RealmService realmService)
{
	private User? _loggedUser;

	public async Task<User> GetLoggedUser()
	{
		return _loggedUser ??= (await realmService.FindAll<User>()).First();
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

				_loggedUser = user;
				_loggedUser.RemoteUserId = returnedUser.RemoteUserId;
				await realmService.Upsert(user);

				return "Success";
			}
			else
				return await response.Content.ReadAsStringAsync();
		}
		catch (Exception ex)
		{
			return $"Exception occured. {ex}";
		}
	}
}

// Best practices for HttpClient: https://bytedev.medium.com/net-core-httpclient-best-practices-4c1b20e32c6