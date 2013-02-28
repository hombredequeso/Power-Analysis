using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HDC.PowerAnalysis.Core;
using HDC.PowerAnalysis.Mac;
using HDC.PowerAnalysis.Security;
using HDC.PowerAnalysis.Utility;
using HDC.PowerAnalysis.Web.Controllers;
using NSubstitute;
using NUnit.Framework;
using PowerAnalysis.Domains.UnitTests.TestInfrastructure;

namespace PowerAnalysis.Domains.UnitTests.Controllers
{
	[TestFixture]
	public class MacControllerTests : AAATestInfrastructure
	{
		private string userId = null;
		private string siteAdminUserId = null;

		private readonly EntityReference companyA = new EntityReference("companies/1", "CompanyA");
		private readonly EntityReference companyB = new EntityReference("companies/2", "CompanyB");

		private IList<string> _companyAChartIds = new List<string>();
		private IList<string> _allChartIds = new List<string>();

		private string _companyBChartId;

		public MacControllerTests()
		{
			_given = () =>
			         	{
			         		var user = new User("userName",
			         		                    "1111111111111",
			         		                    new string[0],
			         		                    new EntityReference("companies/1", "TestCompany"));
			         		_session.Store(user);
			         		userId = user.Id;

			         		var siteAdmin = new User("adminUserName",
			         		                         "222222222222222",
			         		                         new string[] {Roles.SiteAdministrator},
			         		                         companyA);
			         		_session.Store(siteAdmin);
			         		siteAdminUserId = siteAdmin.Id;

			         		var companyACharts = new Chart[2].Select(x => new Chart() {Company = companyA}).ToList();
			         		companyACharts.ForEach(x => _session.Store(x));
			         		_companyAChartIds = companyACharts.Select(x => x.Id).ToList();

			         		var companyBChart1 = new Chart() {Company = companyB};
			         		_session.Store(companyBChart1);
			         		_companyBChartId = companyBChart1.Id;

			         		_allChartIds = new List<string>(_companyAChartIds);
			         		_allChartIds.Add(companyBChart1.Id);
			         	};
		}

		[Test]
		public void Index_Returns_Charts_For_The_Company_Of_Standard_User()
		{
			IList<string> macIds = new List<string>();

			Act(() =>
			    	{
			    		IExecutionContext executionContext = Substitute.For<IExecutionContext>();
			    		executionContext.UserId.Returns(userId);

			    		var controller = new MacController(_session, executionContext);

			    		var result = controller.Index() as ViewResult;
			    		List<Chart> macs = result.Model as List<Chart>;
			    		macIds = macs.Select(x => x.Id).ToList();
			    	});

			Assert.AreEqual(_companyAChartIds, macIds);
		}

		[Test]
		public void Index_Returns_All_Charts_For_User_With_Site_Administrator_Role()
		{
			IList<string> macIds = new List<string>();

			Act(() =>
			    	{
			    		IExecutionContext executionContext = Substitute.For<IExecutionContext>();
			    		executionContext.UserId.Returns(siteAdminUserId);
			    		var controller = new MacController(_session, executionContext);

			    		var result = controller.Index() as ViewResult;
			    		List<Chart> macs = result.Model as List<Chart>;
			    		macIds = macs.Select(x => x.Id).ToList();
			    	});

			Assert.AreEqual(_allChartIds, macIds);
		}

		[Test]
		public void Set_Cannot_Access_Chart_For_Company_Not_Belonged_To_By_User()
		{
			ActionResult result = null;
			Act(() =>
			    	{
			    		IExecutionContext executionContext = Substitute.For<IExecutionContext>();
			    		executionContext.UserId.Returns(userId);
			    		var controller = new MacController(_session, executionContext);
			    		result = controller.Set(ChartIdUtilities.GetInt(_companyBChartId, typeof (Chart)));
			    	});

			Assert.IsInstanceOf<RedirectResult>(result);
			Assert.AreEqual("~/", ((RedirectResult) result).Url);
		}

		[Test]
		public void Set_Returns_Chart_For_Users_Company()
		{
			ActionResult result = null;
			Act(() =>
			{
				IExecutionContext executionContext = Substitute.For<IExecutionContext>();
				executionContext.UserId.Returns(userId);
				var controller = new MacController(_session, executionContext);
				result = controller.Set(ChartIdUtilities.GetInt(_companyAChartIds.First(), typeof(Chart)));
			});

			Assert.IsInstanceOf<ViewResult>(result);
			Assert.AreEqual(_companyAChartIds.First(), ((Chart)((ViewResult)result).Model).Id);
		}
	}
}