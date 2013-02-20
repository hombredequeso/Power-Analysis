using System.Collections.Generic;
using System.Linq;
using HDC.PowerAnalysis.Core;
using HDC.PowerAnalysis.Mac;
using HDC.PowerAnalysis.Mac.Queries;
using HDC.PowerAnalysis.Security;
using NUnit.Framework;
using PowerAnalysis.Domains.UnitTests.Controllers;
using PowerAnalysis.Domains.UnitTests.TestInfrastructure;

namespace PowerAnalysis.Domains.UnitTests.Security
{
	[TestFixture]
	public class MacQueryTests : AAATestInfrastructure
	{
		private EntityReference companyA;
		private Chart companyAChart;
		private EntityReference companyB;
		private Chart companyBChart;


		public MacQueryTests()
		{
			_given = () =>
			         	{
			         		companyA = new EntityReference("companies/123", "CompanyDescription");
			         		companyAChart = new Chart() {Company = companyA};
			         		companyB = new EntityReference("companies/456", "CompanyDescription");
			         		companyBChart = new Chart() {Company = companyB};

			         		_session.Store(companyAChart);
			         		_session.Store(companyBChart);
			         	};
		}

		[Test]
		public void Correct_Charts_Are_Returned_Per_Company()
		{
			IList<string> charts = null;
			Act(() =>
			    	{
			    		charts = _session.Query<Chart>()
			    			.SecurityFilter(companyAChart.Company, null)
			    			.Select(x => x.Id).ToList();
			    	});

			var expectedCharts = new List<string>() {companyAChart.Id};
			Assert.AreEqual(expectedCharts, charts);
		}

		[Test]
		public void All_Charts_Are_Returned_For_Site_Administrator()
		{
			IList<string> charts = null;
			Act(() =>
			    	{
			    		charts = _session.Query<Chart>()
			    			.SecurityFilter(companyAChart.Company, new[] {Roles.SiteAdministrator})
							.Select(x => x.Id).ToList();
					});
			
			var expectedCharts = new List<string>() {companyAChart.Id, companyBChart.Id};
			Assert.AreEqual(expectedCharts, charts);
		}
	}
}