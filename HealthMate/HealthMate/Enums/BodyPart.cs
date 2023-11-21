namespace HealthMate.Enums;
public enum BodyPart
{
    AbdomenPelvisButtocks,
    ArmsShoulder,
    ChestBack,
    HeadThroatNeck,
    Legs,
    SkinJointsGeneral
}

public static partial class Extensions
{
    public static string GetBodyPartName(this BodyPart bodyPart)
    {
        return bodyPart switch
        {
            BodyPart.AbdomenPelvisButtocks => "Abdomen, pelvis & buttocks",
            BodyPart.ArmsShoulder => "Arms & shoulder",
            BodyPart.ChestBack => "Chest & back",
            //BodyPart.HeadThroatNeck => "Head, throat & neck",
            BodyPart.HeadThroatNeck => "Head",
            BodyPart.Legs => "Legs",
            BodyPart.SkinJointsGeneral => "Skin, joints & general",
            _ => string.Empty,
        };
    }
}