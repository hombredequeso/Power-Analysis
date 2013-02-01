namespace HDC.PowerAnalysis.Web.ViewModels
{
	public class UserUpdatePasswordModel
	{
		public string Username { get; set; }
		public string OldPassword { get; set; }
		public string NewPassword { get; set; }
		public string ConfirmNewPassword { get; set; }
	}
}