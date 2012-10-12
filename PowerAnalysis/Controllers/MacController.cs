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
			            		        		new ChartItem() {x = 10, y = -200, label = "theLabel"},
			            		        		new ChartItem() {x = 20, y = -50, label = "theLabel"},
			            		        		new ChartItem() {x = 30, y = 50, label = "theLabel"},
			            		        		new ChartItem() {x = 40, y = 100, label = "item2"}
			            		        	}
			            	};
			object serializedChart = JsonConvert.SerializeObject(chart,
			                                                     Formatting.Indented,
			                                                     new JsonSerializerSettings
			                                                     	{
			                                                     		ContractResolver =
			                                                     			new CamelCasePropertyNamesContractResolver()
			                                                     	});
			return View(serializedChart);
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
				using (MemoryStream ms = new MemoryStream())
				{
					file.InputStream.CopyTo(ms);
					byte[] array = ms.GetBuffer();

					// convert stream to string.
					// deserialize string into chart.
					// save in Ravendb.
				}
			}
			// redirect back to the index action to show the form once again
			return RedirectToAction("Set");
		}
	}
}