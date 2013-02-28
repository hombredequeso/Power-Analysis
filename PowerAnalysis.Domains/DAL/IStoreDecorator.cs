using Raven.Client;

namespace HDC.PowerAnalysis.DAL
{
	public interface IStoreDecorator
	{
		void Do(IDocumentSession session, dynamic entity);
	}
}