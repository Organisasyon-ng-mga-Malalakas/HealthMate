﻿using MongoDB.Bson;
using Realms;

namespace HealthMate.Models.Tables;
public partial class UserTable : IRealmObject
{
	[PrimaryKey]
	public ObjectId RealmUserId { get; set; }
	public string Username { get; set; }
	public string Email { get; set; }
	public string UserId { get; set; }
	public string Gender { get; set; }
	public DateTimeOffset Birthdate { get; set; }
}