using HealthMate.Extensions;
using HealthMate.Models;
using HealthMate.Models.Tables;
using MongoDB.Bson;

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
				//var test = await response.Content.ReadAsStringAsync();

				var stream = await response.Content.ReadAsStreamAsync();
				var schedules = stream.DeserializeStream<IEnumerable<ScheduleDTO>>();
				foreach (var schedule in schedules)
				{
					var actualSchedule = schedule.ToSchedule();
					actualSchedule.Inventory = await realmService.Find<Inventory>(ObjectId.Parse(schedule.InventoryId));
					await realmService.Upsert(actualSchedule);
				}
			}
			else
				return;
		}
	}

	public async Task UpsertSchedule(IEnumerable<Schedule> schedules = null, Schedule schedToUpdate = null)
	{
		var loggedUser = await userService.GetLoggedUser();
		var content = new
		{
			user_id = loggedUser.RemoteUserId,
			schedules = schedules == null
				? [schedToUpdate.ToDataTransferObject()]
				: schedules.Select(_ => _.ToDataTransferObject())
		};

		await httpClient.SendAsync(new HttpRequestMessage
		{
			Content = content.AsJSONSerializedObject(),
			Method = HttpMethod.Post,
			RequestUri = new Uri("/schedule/", UriKind.Relative)
		}, HttpCompletionOption.ResponseHeadersRead);

		if (schedules != null)
			foreach (var schedule in schedules)
				await realmService.Upsert(schedule);
	}
}
