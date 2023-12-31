﻿using MongoDB.Bson;
using Realms;
using System.Text.Json.Serialization;

namespace HealthMate.Models.Tables;
public partial class User : IRealmObject
{
	[PrimaryKey]
	[JsonIgnore]
	public ObjectId LocalUserId { get; set; }

	[JsonPropertyName("username")]
	public string Username { get; set; }

	[JsonPropertyName("email")]
	public string Email { get; set; }

	[JsonPropertyName("id")]
	public string RemoteUserId { get; set; }

	[JsonPropertyName("gender")]
	public string Gender { get; set; }

	[JsonPropertyName("birthdate")]
	public DateTimeOffset Birthdate { get; set; }

	[Ignored]
	[JsonPropertyName("password")]
	public string Password { get; set; }
}