using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HDC.PowerAnalysis.Core;
using HDC.PowerAnalysis.Mac;
using HDC.PowerAnalysis.Security;
using HDC.PowerAnalysis.Web.Controllers;
using NSubstitute;
using NUnit.Framework;
using PowerAnalysis.Domains.UnitTests.TestInfrastructure;

namespace PowerAnalysis.Domains.UnitTests.Controllers
{
	[TestFixture]
	public class MacControllerTests : AAATestInfrastructure
	{
		private string userName = "user1";
		private string siteAdminUserName = "user2";

		private readonly EntityReference companyA = new EntityReference("companies/1", "CompanyA");
		private readonly EntityReference companyB = new EntityReference("companies/2", "CompanyB");

		private IList<string> _companyAChartIds = new List<string>();
		private IList<string> _allChartIds = new List<string>();

		public MacControllerTests()
		{
			_given = () =>
			         	{
			         		var user = new User(userName,
			         		                    "1111111111111",
			         		                    new string[0],
			         		                    new EntityReference("companies/1", "TestCompany"));
			         		_session.Store(user);

			         		var siteAdmin = new User(siteAdminUserName,
			         		                         "222222222222222",
			         		                         new string[] {Roles.SiteAdministrator},
			         		                         companyA);
							_session.Store(siteAdmin);

			         		var companyACharts = new Chart[2].Select(x => new Chart() {Company = companyA}).ToList();
							companyACharts.ForEach(x => _session.Store(x));
			         		_companyAChartIds = companyACharts.Select(x => x.Id).ToList();

							var companyBChart1 = new Chart() {Company = companyB};
			         		_session.Store(companyBChart1);

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
			    		executionContext.Username.Returns(userName);
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
				executionContext.Username.Returns(siteAdminUserName);
				var controller = new MacController(_session, executionContext);
				var result = controller.Index() as ViewResult;
				List<Chart> macs = result.Model as List<Chart>;
				macIds = macs.Select(x => x.Id).ToList();
			});

			Assert.AreEqual(_allChartIds, macIds);
		}
	}
}