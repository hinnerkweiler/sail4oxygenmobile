using System;
using Konscious.Security.Cryptography;
using System.Text;

namespace Sailing4oxygenApi.Models
{
	public class NewUser
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Firstname { get; set; }
		public string Password { get; set; }
        public string BoatName { get; set; }

        public NewUser(string email, string name, string firstname, string password, string boatname)
		{
			this.Email = email;
			this.Name = name;
			this.Firstname = firstname;
			this.Password = password;
			this.BoatName = boatname;
		}

	}
}

