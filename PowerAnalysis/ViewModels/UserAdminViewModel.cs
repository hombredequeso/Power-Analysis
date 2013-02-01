namespace HDC.PowerAnalysis.Web.ViewModels
{
	public class UserAdminViewModel
	{
		public UserAdminViewModel(string username)
		{
			Username = username;
		}

		public string Username { get; private set; }
	}
}