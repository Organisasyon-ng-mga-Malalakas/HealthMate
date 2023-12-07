using MongoDB.Bson;
using Realms;

namespace HealthMate.Models.Tables;

public partial class Questionnaires : IRealmObject
{
	[PrimaryKey]
	public ObjectId QuestionnaireId { get; set; }
	public User User { get; set; }
	public string Key { get; set; }
	public double Value { get; set; }
	public bool IsGeneralQuestionnaire { get; set; }
}