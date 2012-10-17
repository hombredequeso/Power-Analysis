using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Web.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PowerAnalysis.Controllers
{
	public class ChartItem
	{
		public int x { get; set; }
		public int y { get; set; }
		public string label { get; set; }
	}

	public class Chart
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public ChartItem[] Items { get; set; }
	}

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


	public class MacController : RavenController
	{
		public ActionResult Index()
		{
			var existingCharts = RavenSession.Query<Chart>().ToList();
			return View(existingCharts);
		}

		public ActionResult Set(int id)
		{
			var chart = RavenSession.Load<Chart>("charts/" + id);

			return View(chart);
		}

		public ActionResult Load()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Load(HttpPostedFileBase file)
		{
			// Verify that the user selected a file
			Chart chart;
			try
			{
				if (file != null && file.ContentLength > 0)
				{
					using (StreamReader reader = new StreamReader(file.InputStream))
					{
						string text = reader.ReadToEnd();
						chart = JsonConvert.DeserializeObject<Chart>(text);
						Validate(chart);
						this.RavenSession.Store(chart);
						return RedirectToAction("Set", "Mac", new RouteValueDictionary() { { "id", chart.Id.Substring(7) } });
					}
				}
				else
				{
					TempData["Message"] = "Error loading file: there was no file, or it had no contents.";
					return View();
				}
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
			if (chart.Items.Count() == 0)
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