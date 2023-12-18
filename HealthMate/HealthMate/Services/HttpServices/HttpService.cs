using Fody;
using HealthMate.Enums;
using HealthMate.Extensions;
using HealthMate.Models;
using System.Reflection;

namespace HealthMate.Services.HttpServices;

public class HttpService
{
	private readonly HttpClient _authClient;
	private readonly HttpClient _healthClient;
	private string _token = "";
	private readonly UserService _userService;

	public Dictionary<BodyPart, IEnumerable<Issues>> SublocationsDictionary { get; set; }

	public HttpService(IHttpClientFactory httpClientFactory, UserService userService)
	{
		_authClient = httpClientFactory.CreateClient("auth");
		_healthClient = httpClientFactory.CreateClient("health");
		_userService = userService;

		using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("HealthMate.Resources.Sublocations.json") ?? throw new FileNotFoundException("Embedded resource not found");
		var dummyDictionary = stream.DeserializeStream<Dictionary<string, IEnumerable<Issues>>>();
		SublocationsDictionary = dummyDictionary.ToDictionary(_ => _.Key.GetBodyPartEnum(), _ => _.Value);
	}

	private async Task GenerateToken()
	{
		if (!string.IsNullOrWhiteSpace(_token))
			return;

		var response = await _authClient.SendAsync(new HttpRequestMessage
		{
			Method = HttpMethod.Post,
			RequestUri = new Uri("login", UriKind.Relative)
		}, HttpCompletionOption.ResponseHeadersRead);

		if (response.IsSuccessStatusCode)
		{
			var stream = await response.Content.ReadAsStreamAsync();
			var dictionary = stream.NewtonsoftDeserializeStream<Dictionary<string, object>>();
			_token = (string)dictionary["Token"];
		}
		else
			return;
	}

	public async Task<IEnumerable<SymptomInfo>> GetSymptoms(int locationId)
	{
		var loggedUser = await _userService.GetLoggedUser();
		await GenerateToken();
		var selector = DateTime.Now.Year - loggedUser.Birthdate.Year < 12
			? loggedUser.Gender == "male" ? "boy" : "girl"
			: loggedUser.Gender == "male" ? "man" : "woman";

		var response = await _healthClient.SendAsync(new HttpRequestMessage
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"symptoms/{locationId}/{selector}?language=en-gb&token={_token}", UriKind.Relative)
		}, HttpCompletionOption.ResponseHeadersRead);

		if (response.IsSuccessStatusCode)
		{
			var responseStream = await response.Content.ReadAsStreamAsync();
			var diagnosis = responseStream.DeserializeStream<IEnumerable<SymptomInfo>>();

			return diagnosis;
		}
		else
			return Enumerable.Empty<SymptomInfo>();
	}

	public async Task<IEnumerable<Diagnosis>> GetDiagnosis(IEnumerable<int> symptomIds)
	{
		var loggedUser = await _userService.GetLoggedUser();
		await GenerateToken();
		var symptoms = string.Join(",", symptomIds);
		var response = await _healthClient.SendAsync(new HttpRequestMessage
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"diagnosis?symptoms=[{symptoms}]&gender={loggedUser.Gender}&year_of_birth={loggedUser.Birthdate.Year}&language=en-gb&token={_token}", UriKind.Relative)
		}, HttpCompletionOption.ResponseHeadersRead);

		if (response.IsSuccessStatusCode)
		{
			var responseStream = await response.Content.ReadAsStreamAsync();
			var diagnosis = responseStream.DeserializeStream<IEnumerable<Diagnosis>>();

			return diagnosis;
		}
		else
			return Enumerable.Empty<Diagnosis>();
	}

	[ConfigureAwait(false)]
	public async Task<IssueInfo> GetIssueInfo(int issueId)
	{
		await GenerateToken();
		var response = await _healthClient.SendAsync(new HttpRequestMessage
		{
			Method = HttpMethod.Get,
			RequestUri = new Uri($"issues/{issueId}/info?language=en-gb&token={_token}", UriKind.Relative)
		}, HttpCompletionOption.ResponseHeadersRead);

		if (response.IsSuccessStatusCode)
		{
			var responseStream = await response.Content.ReadAsStreamAsync();
			var issueInfo = responseStream.DeserializeStream<IssueInfo>();

			return issueInfo;
		}
		else
			return new IssueInfo();
	}
}