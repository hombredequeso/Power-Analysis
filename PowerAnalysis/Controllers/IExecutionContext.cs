using System.Security.Principal;
using System.Threading;

namespace HDC.PowerAnalysis.Web.Controllers
{
	public interface IExecutionContext
	{
		string Username { get; }	 
	}

	public class ExecutionContext : IExecutionContext
	{
		public ExecutionContext()
		{
			
		}	
	
		public string Username
		{
			get
			{
				IPrincipal currentPrinciple = Thread.CurrentPrincipal;
				return currentPrinciple.Identity.Name;
			}
		}
	}
}