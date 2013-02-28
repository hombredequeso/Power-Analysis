using System;
using System.Collections.Generic;
using HDC.PowerAnalysis.Mac;

namespace HDC.PowerAnalysis.Utility
{
	public static class ChartIdUtilities
	{
		public static int GetInt(string docId, Type t)
		{
			return _idCalculators[t](docId);
		}

		private static IDictionary<Type, Func<string, int>> _idCalculators = new Dictionary<Type, Func<string, int>>()
		                                                                     	{
		                                                                     		{typeof(Chart), x => int.Parse(x.Substring(7))}
		                                                                     	};
	}
}