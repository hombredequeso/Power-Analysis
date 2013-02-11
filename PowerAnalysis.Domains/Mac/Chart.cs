using HDC.PowerAnalysis.Core;

namespace HDC.PowerAnalysis.Mac
{
	public class Chart
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public EntityReference Company { get; set; }
		public Item[] Items { get; set; }

		public class Item
		{
			public int x { get; set; }
			public int y { get; set; }
			public string label { get; set; }
		}
	}
}