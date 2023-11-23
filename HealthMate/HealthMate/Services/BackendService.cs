using Fody;
using HealthMate.Models;
using HealthMateBackend;
using Newtonsoft.Json.Linq;

namespace HealthMate.Services;
[ConfigureAwait(false)]
public class BackendService
{
    private readonly HealthMateAPIClient _client;

    public BackendService()
    {
        var httpClient = new HttpClient();
        _client = new HealthMateAPIClient("https://healthmate-api.mangobeach-087ac216.eastasia.azurecontainerapps.io", httpClient);
    }

    public async Task<Dictionary<string, IEnumerable<Symptoms>>> GetSymptoms(int birth_year, Gender gender, Body_part body_part)
    {
        var response = await _client.GetAsync(birth_year, gender, body_part);
        return response is JObject jsonResponse
            ? jsonResponse.ToObject<Dictionary<string, IEnumerable<Symptoms>>>()
            : [];
    }

    public async void Test()
    {

    }
}
