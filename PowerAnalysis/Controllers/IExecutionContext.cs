using System.Security.Principal;
using System.Threading;

namespace HDC.PowerAnalysis.Web.Controllers
{
	public interface IExecutionContext
	{
		string UserId { get; }	 
	}

	public class ExecutionContext : IExecutionContext
	{
		public ExecutionContext()
		{
			
		}	
	
		public string UserId
		{
			get
			{
				IPrincipal currentPrinciple = Thread.CurrentPrincipal;
				return currentPrinciple.Identity.Name;
			}
		}
	}
}