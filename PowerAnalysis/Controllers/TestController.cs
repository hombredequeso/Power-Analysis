using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace HDC.PowerAnalysis.Web.Controllers
{

	public class TestController : Controller
	{
		[Authorize(Roles = "siteadministrator")]
		public ActionResult Index()
		{
			ViewBag.Message = "Test Application";
			ViewBag.Version = ConfigurationManager.AppSettings["appharbor.commit_id"];
			ViewBag.DbStatus = GetDbAdmin();

			return View();
		}

		private string GetDbAdmin()
		{
			string connString = ConfigurationManager.ConnectionStrings["RavenDB"].ConnectionString;
			int httpStart = connString.IndexOf("http");
			int httpEnd = connString.IndexOf(';');
			string url = connString.Substring(httpStart);
			if (httpEnd != -1)
			{
				string ending = connString.Substring(httpEnd);
				url = url.Substring(0, httpEnd - httpStart) + "/stats" + ending;
			}
			else
			{
				url = url.Trim() + "/stats";
			}

			HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(url);
			WebReq.Method = "GET";
			try
			{
			using (HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse())
			{
				//Now, we read the response (the string), and output it.
				using (Stream Answer = WebResp.GetResponseStream())
				{
					using (StreamReader _Answer = new StreamReader(Answer))
					{
						var raw = _Answer.ReadToEnd();
						JObject json = JObject.Parse(raw);
						string formatted = json.ToString();
						string asHtml = formatted.Replace("\r\n", "<br>");
						return asHtml;
					}
				}
			}

			}
			catch (WebException ex)
			{
				Exception e = ex;
				StringBuilder sb = new StringBuilder();
				while (e != null)
				{
					sb.AppendFormat("{0}<br>", e.Message);
					e = e.InnerException;
				}
				return sb.ToString();
			}
		}

		public ActionResult Uncaughtexception(int id)
		{
			throw new Exception("This is an uncaught exception.");
		}

		public void TestActionA()
		{
			var smtpClient = new SmtpClient();
			string fromAddress = ConfigurationManager.AppSettings["WebSiteEmail"];
			string toAddress = ConfigurationManager.AppSettings["SiteAdministratorEmail"];

			smtpClient.Send(new MailMessage(fromAddress, toAddress, "Test Email", "TestActionA was performed."));
		}

		[HttpPost]
		public ActionResult DoError()
		{
			throw new Exception("This is a post exception");
		}
	}
}