using HDC.PowerAnalysis.DAL;
using StructureMap.Configuration.DSL;

namespace HDC.PowerAnalysis
{
	public class PowerAnalysisDomainsRegistry : Registry
	{
		public PowerAnalysisDomainsRegistry()
		{
			For<IStoreDecorator>().Add<UserNameUniqueDecorator>();
		}
	}
}