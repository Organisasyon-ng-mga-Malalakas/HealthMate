namespace HealthMate.Enums;
public enum BodyPart
{
	AbdomenPelvisButtocks,
	ArmsAndShoulder,
	ChestAndBack,
	HeadThroatAndNeck,
	Legs,
	SkinJointsAndGeneral
}

public static partial class Extensions
{
	public static int GetBodyPartId(this BodyPart bodyPart)
	{
		return bodyPart switch
		{
			BodyPart.AbdomenPelvisButtocks => 16,
			BodyPart.ArmsAndShoulder => 7,
			BodyPart.ChestAndBack => 15,
			BodyPart.HeadThroatAndNeck => 6,
			BodyPart.Legs => 10,
			BodyPart.SkinJointsAndGeneral => 17,
			_ => throw new NotImplementedException(),
		};
	}

	public static string GetStringRepresentation(this BodyPart bodyPart)
	{
		return bodyPart switch
		{
			BodyPart.AbdomenPelvisButtocks => "Abdomen, pelvis & buttocks",
			BodyPart.ArmsAndShoulder => "Arms & shoulder",
			BodyPart.ChestAndBack => "Chest & back",
			BodyPart.HeadThroatAndNeck => "Head, throat & neck",
			BodyPart.Legs => "Legs",
			BodyPart.SkinJointsAndGeneral => "Skin, joints & general",
			_ => string.Empty
		};
	}

	public static BodyPart GetBodyPartEnum(this string bodyPart)
	{
		return bodyPart switch
		{
			"Abdomen, pelvis & buttocks" => BodyPart.AbdomenPelvisButtocks,
			"Arms & shoulder" => BodyPart.ArmsAndShoulder,
			"Chest & back" => BodyPart.ChestAndBack,
			"Head, throat & neck" => BodyPart.HeadThroatAndNeck,
			"Legs" => BodyPart.Legs,
			"Skin, joints & general" => BodyPart.SkinJointsAndGeneral,
			_ => BodyPart.SkinJointsAndGeneral
		};
	}

	public static string GetPhotoFileName(this BodyPart bodyPart)
	{
		return bodyPart switch
		{
			BodyPart.AbdomenPelvisButtocks => "abp.svg",
			BodyPart.ArmsAndShoulder => "arms_and_shoulder.svg",
			BodyPart.ChestAndBack => "chest_and_back.svg",
			BodyPart.HeadThroatAndNeck => "head_throat_neck.svg",
			BodyPart.Legs => "legs.svg",
			BodyPart.SkinJointsAndGeneral => "general.svg",
			_ => ""
		};
	}
}