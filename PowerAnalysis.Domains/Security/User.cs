using System;
using System.Collections.Generic;
using System.Linq;
using HDC.PowerAnalysis.Core;

namespace HDC.PowerAnalysis.Security
{
	public class User
	{
		public User(string username, string password, IEnumerable<string> roles, EntityReference company)
		{
			if (username == null) throw new ArgumentNullException("username");
			if (username.Count() < 3) throw new ArgumentException("UserName must be at least 3 characters long", "username");
			ValidatePassword(password);
			Username = username;
			Password = password;
			_roles = roles.ToList();
			Company = company;
		}

		private static void ValidatePassword(string password)
		{
			if (password == null) throw new ArgumentNullException("password");
			if (password.Count() < 10) throw new ArgumentException("Password must be at least 10 characters long", "password");			
		}

		public string Id { get; private set; }
		public string Username { get; private set; }
		public string Password { get; private set; }
		public IEnumerable<string> Roles { get { return _roles; } }
		private IList<string> _roles;
		public EntityReference Company { get; private set; }

		public void ChangePassword(string newPassword)
		{
			ValidatePassword(newPassword);
			Password = newPassword;
		}
	}
}