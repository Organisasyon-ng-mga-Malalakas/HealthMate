using Fody;
using HealthMate.Models;
using HealthMateBackend;
using Newtonsoft.Json.Linq;

namespace HealthMate.Services.HttpServices;
[ConfigureAwait(false)]
public class HttpService
{
    private readonly HealthMateAPIClient _client;

    public HttpService(HealthMateAPIClient client)
    {
        _client = client;
    }

    public async Task<Dictionary<string, IEnumerable<Symptoms>>> GetSymptoms(int birth_year, Gender gender, Body_part body_Part)
    {
        var response = await _client.GetAsync(birth_year, gender, body_Part);
        return response is JObject jsonResponse
            ? jsonResponse.ToObject<Dictionary<string, IEnumerable<Symptoms>>>()
            : [];
    }

    public async Task<RootDiagnosis> GetDiseaseFromSymptoms(int birth_year, Gender gender, Body_part body_part, string symptom_ids)
    {
        var response = await _client.GetAsync(birth_year, gender, body_part, symptom_ids);
        var diagnosis = response is JObject jsonResponse
            ? jsonResponse.ToObject<RootDiagnosis>()
            : null;

        return diagnosis;
    }

    public async Task<DiagnosisInfo> GetDiseaseInfo(int diagnosis_id, int birth_year, Gender3 gender, Body_part3 body_part)
    {
        var response = await _client.GetAsync(diagnosis_id, birth_year, gender, body_part);
        var diagnosisInfo = response is JObject jsonResponse
            ? jsonResponse.ToObject<DiagnosisInfo>()
            : null;

        return diagnosisInfo;
    }
}