
using Fody;
using HealthMate.Models.Schemas;
using HealthMate.Models.Tables;
using Newtonsoft.Json;
using System.Net.Http;

namespace HealthMate.Services;
[ConfigureAwait(false)]
public class UserService
{
	private readonly IHttpClientFactory httpClient;
	private readonly RealmService _realmService;
	private UserTable _userInfo;

	public UserService(IHttpClientFactory _httpClient, RealmService realmService)
	{
		_realmService = realmService;
		httpClient = _httpClient; 

		Task.Run(async () =>
		{
			var userData = await realmService.FindAll<UserTable>();
			_userInfo = userData.Any() && userData.First() is UserTable firstUserData
				? firstUserData
			: null;
		});
		
	}

	public async Task<String> Signup(UserCreate userDetails)
	{
		try
		{
			var json = JsonConvert.SerializeObject(userDetails);
			var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
			var client = httpClient.CreateClient("fastapi");
			var response = await client.PostAsync("user/", content);

			if (!response.IsSuccessStatusCode)
			{
				// Get the response content as a string
				string responseContent = await response.Content.ReadAsStringAsync();

				// Deserialize the JSON string to a dictionary
				var res = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
				if (res.TryGetValue("detail", out string value))
				{
					return value;
				}
				else
				{
					return "API Error";
				}
			}

			_userInfo = new UserTable
			{
				Gender = char.ToUpper(userDetails.gender[0]) + userDetails.gender.Substring(1),
				Birthdate = userDetails.birthdate
			};

			return "success";

		}
		catch (Exception ex)
		{
			return "Exception occured.";
		}
	}
}
