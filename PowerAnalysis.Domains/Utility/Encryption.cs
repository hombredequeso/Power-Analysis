namespace HDC.PowerAnalysis.Utility
{
	public static class Encryption
	{
		public static string HashPassword(string value)
		{
			string salt = BCrypt.Net.BCrypt.GenerateSalt();
			return BCrypt.Net.BCrypt.HashPassword(value, salt);
		}

		public static bool Verify(string value, string hashedValue)
		{
			return BCrypt.Net.BCrypt.Verify(value, hashedValue);
		}


	}
}