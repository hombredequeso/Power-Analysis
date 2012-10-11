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
	}
}