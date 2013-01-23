using System.ComponentModel.DataAnnotations;

namespace HDC.PowerAnalysis.Web.ViewModels
{
	public class SessionViewModel
	{
		[Required]
		public string Username { get; set; }
		[Required]
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
}
