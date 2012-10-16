using System.IO;
using System.Web;
using System.Web.Mvc;
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


	public class MacController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Set(string setName)
		{
			var chart = new Chart
			            	{
			            		Items = new[]
			            		        	{
			            		        		new ChartItem() {x = 10, y = -200, label = "theLabel1"},
			            		        		new ChartItem() {x = 20, y = -50, label = "theLabel2"},
			            		        		new ChartItem() {x = 30, y = 50, label = "theLabel3"},
			            		        		new ChartItem() {x = 40, y = 100, label = "theLabel4"}
			            		        	}
			            	};
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
			if (file != null && file.ContentLength > 0)
			{
					using (StreamReader reader = new StreamReader(file.InputStream))
					{
						string text = reader.ReadToEnd();
						var chart = JsonConvert.DeserializeObject<Chart>(text);
				}
			}
			// redirect back to the index action to show the form once again
			return RedirectToAction("Set");
		}
	}
}