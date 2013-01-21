using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace PowerAnalysis.Controllers
{
	class JsonHelper
	{
		private const string INDENT_STRING = "    ";
		public static string FormatJson(string str)
		{
			var indent = 0;
			var quoted = false;
			var sb = new StringBuilder();
			for (var i = 0; i < str.Length; i++)
			{
				var ch = str[i];
				switch (ch)
				{
					case '{':
					case '[':
						sb.Append(ch);
						if (!quoted)
						{
							sb.AppendLine();
							Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
						}
						break;
					case '}':
					case ']':
						if (!quoted)
						{
							sb.AppendLine();
							Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
						}
						sb.Append(ch);
						break;
					case '"':
						sb.Append(ch);
						bool escaped = false;
						var index = i;
						while (index > 0 && str[--index] == '\\')
							escaped = !escaped;
						if (!escaped)
							quoted = !quoted;
						break;
					case ',':
						sb.Append(ch);
						if (!quoted)
						{
							sb.AppendLine();
							Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
						}
						break;
					case ':':
						sb.Append(ch);
						if (!quoted)
							sb.Append(" ");
						break;
					default:
						sb.Append(ch);
						break;
				}
			}
			return sb.ToString();
		}
	}

	static class Extensions
	{
		public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
		{
			foreach (var i in ie)
			{
				action(i);
			}
		}
	}



	public class TestController : Controller
	{
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

		[HttpPost]
		public ActionResult DoError()
		{
			throw new Exception("This is a post exception");
		}
	}
}