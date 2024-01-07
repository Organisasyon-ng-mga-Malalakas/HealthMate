using HealthMate.Extensions;
using HealthMate.Models;
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
				var inventories = stream.DeserializeStream<IEnumerable<InventoryDTO>>();
				foreach (var inventory in inventories)
					await realmService.Upsert(inventory.ToInventory());
			}
			else
				return;
		}
	}

	public async Task UpsertInventory(IEnumerable<Inventory> inventories = null, Inventory inventoryToUpdate = null)
	{
		var loggedUser = await userService.GetLoggedUser();
		var content = new
		{
			user_id = loggedUser.RemoteUserId,
			inventory = inventories == null
				? [inventoryToUpdate.ToDataTransferObject()]
				: inventories.Select(_ => _.ToDataTransferObject())
		};

		await httpClient.SendAsync(new HttpRequestMessage
		{
			Content = content.AsJSONSerializedObject(),
			Method = HttpMethod.Post,
			RequestUri = new Uri("/inventory/", UriKind.Relative)
		}, HttpCompletionOption.ResponseHeadersRead);

		if (inventories != null)
			foreach (var inventory in inventories)
				await realmService.Upsert(inventory);
	}

	public async Task DeleteInventory(Inventory inventory)
	{
		var response = await httpClient.SendAsync(new HttpRequestMessage
		{
			Method = HttpMethod.Delete,
			RequestUri = new Uri($"/inventory/?id={inventory.InventoryId}", UriKind.Relative)
		}, HttpCompletionOption.ResponseHeadersRead);
	}
}