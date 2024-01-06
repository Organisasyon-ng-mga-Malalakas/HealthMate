using HealthMate.Extensions;
using HealthMate.Models.Tables;

namespace HealthMate.Services;
public class ScheduleService(HttpClient httpClient, RealmService realmService, UserService userService)
{
	public async Task<IQueryable<Schedule>> GetSchedules()
	{
		var schedulesForUser = await realmService.FindAll<Schedule>();
		return schedulesForUser;
	}

	public async Task PopulateUserScheduleFromRemote()
	{
		var loggedUser = await userService.GetLoggedUser();
		var schedulesForUser = await realmService.FindAll<Schedule>();
		if (loggedUser is User user && !schedulesForUser.Any())
		{
			var response = await httpClient.SendAsync(new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri($"/schedule/?user_id={user.RemoteUserId}", UriKind.Relative)
			}, HttpCompletionOption.ResponseHeadersRead);

			if (response.IsSuccessStatusCode)
			{
				var stream = await response.Content.ReadAsStreamAsync();
				var schedules = stream.DeserializeStream<IEnumerable<Schedule>>();
				foreach (var schedule in schedules)
					await realmService.Upsert(schedule);
			}
			else
				return;
		}
	}

	public async Task<bool> UpsertSchedule(IEnumerable<Schedule> schedules)
	{
		try
		{
			var loggedUser = await userService.GetLoggedUser();
			var content = new
			{
				user_id = loggedUser.RemoteUserId,
				schedules
			};

			var response = await httpClient.SendAsync(new HttpRequestMessage
			{
				Content = content.AsJSONSerializedObject(),
				Method = HttpMethod.Post,
				RequestUri = new Uri("/schedule/", UriKind.Relative)
			}, HttpCompletionOption.ResponseHeadersRead);

			foreach (var item in schedules)
				await realmService.Upsert(item);

			//return response.IsSuccessStatusCode;
			return true;
		}
		catch (Exception ex)
		{

			throw;
		}
	}
}
