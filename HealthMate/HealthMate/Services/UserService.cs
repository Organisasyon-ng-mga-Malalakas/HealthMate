using Fody;
using HealthMate.Extensions;
using HealthMate.Models.Tables;

namespace HealthMate.Services;

public class UserService(HttpClient httpClient, RealmService realmService)
{
	public User? LoggedUser { get; set; }

	public async Task<User> GetLoggedUser()
	{
		return LoggedUser ??= (await realmService.FindAll<User>()).First();
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

				LoggedUser = user;
				LoggedUser.RemoteUserId = returnedUser.RemoteUserId;
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

	public async Task UpsertSchedule(Schedule schedule)
	{
		var content = new
		{
			schedule_id = schedule.ScheduleId.ToString(),
			schedule_state = schedule.ScheduleState,
			time_to_take = schedule.TimeToTake.Date,
			notes = schedule.Notes,
			quantity = schedule.Inventory.Stock,
			image = schedule.PhotoBase64
		};

		var response = await httpClient.SendAsync(new HttpRequestMessage
		{
			Content = content.AsJSONSerializedObject(),
			Method = HttpMethod.Post,
			RequestUri = new Uri("/login/", UriKind.Relative)
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