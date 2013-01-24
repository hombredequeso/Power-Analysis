using System;
using System.Collections.Generic;
using System.Linq;

namespace HDC.PowerAnalysis.Security
{
	public class User
	{
		public User(string username, string password, IEnumerable<string> roles)
		{
			if (username == null) throw new ArgumentNullException("username");
			if (password == null) throw new ArgumentNullException("password");
			if (username.Count() < 3) throw new ArgumentException("UserName must be at least 3 characters long", "username");
			if (password.Count() < 10) throw new ArgumentException("Password must be at least 10 characters long", "password");
			Id = "users/" + username;
			Username = username;
			Password = password;
			_roles = roles.ToList();
		}

		public string Id { get; private set; }
		public string Username { get; private set; }
		public string Password { get; private set; }
		public IEnumerable<string> Roles { get { return _roles; } }
		private IList<string> _roles;
	}
}