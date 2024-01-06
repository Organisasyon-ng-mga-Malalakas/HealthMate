using HealthMate.Extensions;
using HealthMate.Models.Tables;
using System.Linq.Expressions;

namespace HealthMate.Services;
public class InventoryService(HttpClient httpClient, RealmService realmService, UserService userService)
{
	public async Task<IQueryable<Inventory>> GetInventoryForUser(Expression<Func<Inventory, bool>> expression)
	{
		var inventories = await realmService.Find(expression);
		return inventories;
	}

	public async Task PopulateUserInventoryFromRemote()
	{
		var loggedUser = await userService.GetLoggedUser();
		var inventoryForUser = await realmService.FindAll<Inventory>();
		if (loggedUser is User user && !inventoryForUser.Any())
		{
			var response = await httpClient.SendAsync(new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri($"/inventory/?user_id={user.RemoteUserId}", UriKind.Relative)
			}, HttpCompletionOption.ResponseHeadersRead);

			if (response.IsSuccessStatusCode)
			{
				var stream = await response.Content.ReadAsStreamAsync();
				var inventories = stream.DeserializeStream<IEnumerable<Inventory>>();
				foreach (var inventory in inventories)
					await realmService.Upsert(inventory);
			}
			else
				return;
		}
	}

	public async Task<bool> UpsertInventory(IEnumerable<Inventory> inventories)
	{
		try
		{
			foreach (var inventory in inventories)
				await realmService.Upsert(inventory);

			var loggedUser = await userService.GetLoggedUser();
			var test = await realmService.FindAll<Inventory>();
			var content = new
			{
				user_id = loggedUser.RemoteUserId,
				inventory = test
			};

			var response = await httpClient.SendAsync(new HttpRequestMessage
			{
				Content = content.AsJSONSerializedObject(),
				Method = HttpMethod.Post,
				RequestUri = new Uri("/inventory/", UriKind.Relative)
			}, HttpCompletionOption.ResponseHeadersRead);

			return response.IsSuccessStatusCode;
			//return true;
		}
		catch (Exception ex)
		{

			throw;
		}
	}
}