using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace HealthMate.Models;
public class Symptoms : ObservableObject
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("is_critical")]
    public bool IsCritical { get; set; }
    [JsonIgnore]
    public bool IsSelected { get; set; }
}

public class BodyPart
{
    [JsonProperty("head")]
    public Head Head { get; set; }

    [JsonProperty("upperbody")]
    public UpperBody Upperbody { get; set; }

    [JsonProperty("lowerbody")]
    public LowerBody Lowerbody { get; set; }

    [JsonProperty("arms")]
    public Arms Arms { get; set; }

    [JsonProperty("legs")]
    public Legs Legs { get; set; }

    [JsonProperty("general")]
    public General General { get; set; }
}

public class Head
{
    [JsonProperty("Head, throat & neck")]
    public IEnumerable<Symptoms> HeadThroatNeck { get; set; }
    [JsonProperty("Face & eyes")]
    public IEnumerable<Symptoms> FaceEyes { get; set; }
    [JsonProperty("Forehead & head in general")]
    public IEnumerable<Symptoms> ForeheadHeadInGeneral { get; set; }
    [JsonProperty("Hair & scalp")]
    public IEnumerable<Symptoms> HairScalp { get; set; }
    [JsonProperty("Mouth & jaw")]
    public IEnumerable<Symptoms> MouthJaw { get; set; }
    [JsonProperty("Nose, ears, throat & neck")]
    public IEnumerable<Symptoms> NoseEarsThroatNeck { get; set; }
}

public class Arms
{
    [JsonProperty("Arms & shoulder")]
    public IEnumerable<Symptoms> ArmsShoulder { get; set; }

    [JsonProperty("Arms general")]
    public IEnumerable<Symptoms> ArmsGeneral { get; set; }

    [JsonProperty("Finger")]
    public IEnumerable<Symptoms> Finger { get; set; }

    [JsonProperty("Forearm & elbow")]
    public IEnumerable<Symptoms> ForearmElbow { get; set; }

    [JsonProperty("Hand & wrist")]
    public IEnumerable<Symptoms> HandWrist { get; set; }

    [JsonProperty("Upper arm & shoulder")]
    public IEnumerable<Symptoms> UpperArmShoulder { get; set; }
}

public class General
{
    [JsonProperty("Skin, joints & general")]
    public IEnumerable<Symptoms> SkinJointsGeneral { get; set; }

    [JsonProperty("General, joints & other")]
    public IEnumerable<Symptoms> GeneralJointsOther { get; set; }

    [JsonProperty("Skin")]
    public IEnumerable<Symptoms> Skin { get; set; }
}

public class Legs
{
    [JsonProperty("Legs")]
    public IEnumerable<Symptoms> Leg { get; set; }

    [JsonProperty("Foot")]
    public IEnumerable<Symptoms> Foot { get; set; }

    [JsonProperty("Legs general")]
    public IEnumerable<Symptoms> LegsGeneral { get; set; }

    [JsonProperty("Lower leg & ankle")]
    public IEnumerable<Symptoms> LowerLegAnkle { get; set; }

    [JsonProperty("Thigh & knee")]
    public IEnumerable<Symptoms> ThighKnee { get; set; }

    [JsonProperty("Toes")]
    public IEnumerable<Symptoms> Toes { get; set; }
}

public class LowerBody
{
    [JsonProperty("Abdomen, pelvis & buttocks")]
    public IEnumerable<Symptoms> AbdomenPelvisButtocks { get; set; }

    [JsonProperty("Abdomen")]
    public IEnumerable<Symptoms> Abdomen { get; set; }

    [JsonProperty("Buttocks & rectum")]
    public IEnumerable<Symptoms> ButtocksRectum { get; set; }

    [JsonProperty("Genitals & groin")]
    public IEnumerable<Symptoms> GenitalsGroin { get; set; }

    [JsonProperty("Hips & hip joint")]
    public IEnumerable<Symptoms> HipsHipJoint { get; set; }

    [JsonProperty("Pelvis")]
    public IEnumerable<Symptoms> Pelvis { get; set; }
}

public class UpperBody
{
    [JsonProperty("Chest & back")]
    public IEnumerable<Symptoms> ChestBack { get; set; }

    [JsonProperty("Back")]
    public IEnumerable<Symptoms> Back { get; set; }

    [JsonProperty("Chest")]
    public IEnumerable<Symptoms> Chest { get; set; }

    [JsonProperty("Lateral chest")]
    public IEnumerable<Symptoms> LateralChest { get; set; }
}