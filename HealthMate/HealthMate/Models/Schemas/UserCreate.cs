using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMate.Models.Schemas;
public class UserCreate
{
	public string username;
	public string email;
	public string password;
	public DateTime birthdate;
	public string gender;

	public UserCreate(string username, string email, string password, DateTime birthdate, string gender)
	{
		this.email = email;
		this.password = password;
		this.username = username;
		this.birthdate = birthdate;
		this.gender = gender.ToLower();
	}
}
