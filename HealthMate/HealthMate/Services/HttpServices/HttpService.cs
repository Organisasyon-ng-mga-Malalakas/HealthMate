using Fody;
using HealthMate.Extensions;
using HealthMate.Models;
using HealthMate.Models.Tables;
using HealthMate.Services.HttpServices.Symptoms;
using System.Reflection;
using HealthMateSymptoms = HealthMate.Models.Symptoms;
using LocalBodyPartJSON = HealthMate.Models.BodyPart;

namespace HealthMate.Services.HttpServices;
[ConfigureAwait(false)]
public class HttpService
{
	//private readonly HealtmateAPIClient _client;

	//public HttpService(HealtmateAPIClient client)
	//{
	//    _client = client;
	//}

	private readonly ApiClient _client;
	private readonly Dictionary<Symptoms.BodyPart, IEnumerable<HealthMateSymptoms>[]> _bodyPartSymptoms;
	private readonly RealmService _realmService;
	private UserTable _userInfo;

	public HttpService(ApiClient client, RealmService realmService)
	{
		_client = client;
		_realmService = realmService;
		Task.Run(async () =>
		{
			var userData = await realmService.FindAll<UserTable>();
			_userInfo = userData.Any() && userData.First() is UserTable firstUserData
				? firstUserData
				: new UserTable
				{
					Gender = "Male",
					Birthdate = new DateTime(2001, 1, 1)
				};
		});

		using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("HealthMate.Services.HttpServices.symptoms.json") ?? throw new FileNotFoundException("Embedded resource not found");
		var bodyParts = stream.DeserializeStream<LocalBodyPartJSON>();
		_bodyPartSymptoms = new Dictionary<Symptoms.BodyPart, IEnumerable<HealthMateSymptoms>[]>
		{
			{ Symptoms.BodyPart.Head, [bodyParts.Head.HeadThroatNeck, bodyParts.Head.FaceEyes, bodyParts.Head.ForeheadHeadInGeneral, bodyParts.Head.HairScalp, bodyParts.Head.MouthJaw, bodyParts.Head.NoseEarsThroatNeck] },
			{ Symptoms.BodyPart.Upperbody, [bodyParts.Upperbody.ChestBack, bodyParts.Upperbody.Back, bodyParts.Upperbody.Chest, bodyParts.Upperbody.LateralChest] },
			{ Symptoms.BodyPart.Lowerbody, [bodyParts.Lowerbody.AbdomenPelvisButtocks, bodyParts.Lowerbody.Abdomen, bodyParts.Lowerbody.ButtocksRectum, bodyParts.Lowerbody.GenitalsGroin, bodyParts.Lowerbody.HipsHipJoint, bodyParts.Lowerbody.Pelvis] },
			{ Symptoms.BodyPart.Legs, [bodyParts.Legs.Leg, bodyParts.Legs.Foot, bodyParts.Legs.LegsGeneral, bodyParts.Legs.LowerLegAnkle, bodyParts.Legs.ThighKnee, bodyParts.Legs.Toes] },
			{ Symptoms.BodyPart.Arms, [bodyParts.Arms.ArmsShoulder, bodyParts.Arms.ArmsGeneral, bodyParts.Arms.Finger, bodyParts.Arms.ForearmElbow, bodyParts.Arms.HandWrist, bodyParts.Arms.UpperArmShoulder] },
			{ Symptoms.BodyPart.General, [bodyParts.General.SkinJointsGeneral, bodyParts.General.GeneralJointsOther, bodyParts.General.Skin] }
		};
	}

	public IEnumerable<HealthMateSymptoms> GetSymptoms(Symptoms.BodyPart bodyPart)
	{
		var bodyPartSymptoms = _bodyPartSymptoms[bodyPart];
		return bodyPartSymptoms.Length != 0
			? bodyPartSymptoms.SelectMany(_ => _)
			: Enumerable.Empty<HealthMateSymptoms>();
	}

	public async Task<RootDiagnosis> GetDiseaseFromSymptoms(int birthYear, Symptoms.BodyPart bodyPart, Gender gender, string symptomIds)
	{
		using var response = await _client.Symptoms.Result.GetAsync(parameter =>
		{
			parameter.QueryParameters = new Symptoms.Result.ResultRequestBuilder.ResultRequestBuilderGetQueryParameters
			{
				BirthYear = _userInfo.Birthdate.Year,
				BodyPartAsBodyPart = bodyPart,
				GenderAsGender = _userInfo.Gender == "Male" ? Gender.Male : Gender.Female,
				SymptomIds = symptomIds
			};
		});

		return response.DeserializeStream<RootDiagnosis>();
	}

	public async Task<DiagnosisInfo> GetDiseaseInfo(int symptomId, int birthYear, Symptoms.BodyPart bodyPart, Gender gender)
	{
		using var response = await _client.Symptoms.Details[symptomId].GetAsync(parameter =>
		{
			parameter.QueryParameters = new Symptoms.Details.Item.WithDiagnosis_ItemRequestBuilder.WithDiagnosis_ItemRequestBuilderGetQueryParameters
			{
				BirthYear = _userInfo.Birthdate.Year,
				BodyPartAsBodyPart = bodyPart,
				GenderAsGender = _userInfo.Gender == "Male" ? Gender.Male : Gender.Female
			};
		});

		return response.DeserializeStream<DiagnosisInfo>();
	}

	public async Task<Models.Token> Login(string username, string password)
	{
		var response = await _client.User.Login.PostAsync(new Models.Body_login_access_token_route_user_login_post
		{
			Username = username,
			Password = password
		});

		return response;
	}
}