using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Web.Routing;
using HDC.PowerAnalysis.Mac;
using HDC.PowerAnalysis.Mac.Queries;
using HDC.PowerAnalysis.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Raven.Client;

namespace HDC.PowerAnalysis.Web.Controllers
{
	public static class ViewSerializer
	{
		public static string Serialize(this object x)
		{
			return JsonConvert.SerializeObject(x,
																			 Formatting.Indented,
																			 new JsonSerializerSettings
																			 {
																				 ContractResolver =
																					 new CamelCasePropertyNamesContractResolver()
																			 });			
		}
	}


	public class MacController : Controller
	{
		private readonly IDocumentSession _session;
		private readonly IExecutionContext _executionContext;

		public MacController(IDocumentSession session, IExecutionContext executionContext)
		{
			_session = session;
			_executionContext = executionContext;
		}

		public ActionResult Index()
		{
			var user = _session.Load<User>(_executionContext.UserId);
			List<Chart> existingCharts = _session.Query<Chart>()
				.SecurityFilter(user.Company, user.Roles)
				.ToList();
			return View(existingCharts);
		}

		public ActionResult Set(int id)
		{
			var chart = _session.Load<Chart>("charts/" + id);
			var user = _session.Load<User>(_executionContext.UserId);

			if (user.HasAccessTo(chart))
				return View(chart);
			return this.Redirect("~/");
		}

		public ActionResult Load()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Load(HttpPostedFileBase file)
		{
			try
			{
				if (file != null && file.ContentLength > 0)
				{
					using (StreamReader reader = new StreamReader(file.InputStream))
					{
						string text = reader.ReadToEnd();
						Chart chart = JsonConvert.DeserializeObject<Chart>(text);
						Validate(chart);
						string userName = User.Identity.Name;
						var user = _session.Query<User>().FirstOrDefault(x => x.Username == userName);
						chart.Company = user.Company;
						_session.Store(chart);
						return RedirectToAction("Set", "Mac", new RouteValueDictionary { { "id", chart.Id.Substring(7) } });
					}
				}
				TempData["Message"] = "Error loading file: there was no file, or it had no contents.";
				return View();
			}
			catch (JsonReaderException ex)
			{
				TempData["Message"] = string.Format("Error loading file: {0}", ex.Message);
				return View();
			}
			catch (InvalidChartException ice)
			{
				TempData["Message"] = string.Format("Error loading file: {0}", ice.Message);
				return View();
			}
		}

		private void Validate(Chart chart)
		{
			if (string.IsNullOrWhiteSpace(chart.Name))
			{
				throw new InvalidChartException("Name cannot be empty.");
			}
			if (!chart.Items.Any())
			{
				throw new InvalidChartException("The chart must have at least one item in it.");
			}
		}
	}

	public class InvalidChartException : Exception
	{
		public InvalidChartException(string message):base(message)
		{
		}
	}
}