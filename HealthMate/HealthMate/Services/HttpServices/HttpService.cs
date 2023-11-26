using Fody;
using HealthMate.Models;
using HealthMateBackend;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using BodyPart = HealthMateBackend.Body_part;

namespace HealthMate.Services.HttpServices;
[ConfigureAwait(false)]
public class HttpService
{
    private readonly HealtmateAPIClient _client;

    public HttpService(HealtmateAPIClient client)
    {
        _client = client;
    }

    public IEnumerable<Symptoms> GetSymptoms(BodyPart bodyPart)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream("HealthMate.Services.HttpServices.symptoms.json") ?? throw new FileNotFoundException("Embedded resource not found");
        using var streamReader = new StreamReader(stream);
        using var jsonTextReader = new JsonTextReader(streamReader);
        var serializer = new JsonSerializer();
        var bodyParts = serializer.Deserialize<Models.BodyPart>(jsonTextReader);

        IEnumerable<Symptoms>[] bodyPartSymptoms = bodyPart switch
        {
            BodyPart.Head => [bodyParts.Head.HeadThroatNeck, bodyParts.Head.FaceEyes, bodyParts.Head.ForeheadHeadInGeneral, bodyParts.Head.HairScalp, bodyParts.Head.MouthJaw, bodyParts.Head.NoseEarsThroatNeck],
            BodyPart.Upperbody => [bodyParts.Upperbody.ChestBack, bodyParts.Upperbody.Back, bodyParts.Upperbody.Chest, bodyParts.Upperbody.LateralChest],
            BodyPart.Lowerbody => [bodyParts.Lowerbody.AbdomenPelvisButtocks, bodyParts.Lowerbody.Abdomen, bodyParts.Lowerbody.ButtocksRectum, bodyParts.Lowerbody.GenitalsGroin, bodyParts.Lowerbody.HipsHipJoint, bodyParts.Lowerbody.Pelvis],
            BodyPart.Legs => [bodyParts.Legs.Leg, bodyParts.Legs.Foot, bodyParts.Legs.LegsGeneral, bodyParts.Legs.LowerLegAnkle, bodyParts.Legs.ThighKnee, bodyParts.Legs.Toes],
            BodyPart.Arms => [bodyParts.Arms.ArmsShoulder, bodyParts.Arms.ArmsGeneral, bodyParts.Arms.Finger, bodyParts.Arms.ForearmElbow, bodyParts.Arms.HandWrist, bodyParts.Arms.UpperArmShoulder],
            BodyPart.General => [bodyParts.General.SkinJointsGeneral, bodyParts.General.GeneralJointsOther, bodyParts.General.Skin],
            _ => throw new NotImplementedException(),
        };

        var symptoms = bodyPartSymptoms.Length != 0
            ? bodyPartSymptoms.SelectMany(_ => _)
            : Enumerable.Empty<Symptoms>();

        return symptoms;
    }

    public async Task<RootDiagnosis> GetDiseaseFromSymptoms(int birth_year, Gender gender, BodyPart bodyPart, string symptom_ids)
    {
        var response = await _client.GetAsync(birth_year, gender, bodyPart, symptom_ids);
        var diagnosis = response is JObject jsonResponse
            ? jsonResponse.ToObject<RootDiagnosis>()
            : null;

        return diagnosis;
    }

    public async Task<DiagnosisInfo> GetDiseaseInfo(int diagnosis_id, int birth_year, Gender gender, BodyPart bodyPart)
    {
        var response = await _client.GetAsync(diagnosis_id, birth_year, gender, bodyPart);
        var diagnosisInfo = response is JObject jsonResponse
            ? jsonResponse.ToObject<DiagnosisInfo>()
            : null;

        return diagnosisInfo;
    }
}