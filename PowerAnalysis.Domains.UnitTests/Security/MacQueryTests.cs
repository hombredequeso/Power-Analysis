using System.Linq;
using HDC.PowerAnalysis.Core;
using HDC.PowerAnalysis.Mac;
using HDC.PowerAnalysis.Mac.Queries;
using HDC.PowerAnalysis.Security;
using NUnit.Framework;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Linq;

namespace PowerAnalysis.Domains.UnitTests.Security
{
	[TestFixture]
	public class MacQueryTests
	{
		[Test]
		public void Correct_Charts_Are_Returned_Per_Company()
		{
			using (var dataStore = new EmbeddableDocumentStore { RunInMemory = true }.Initialize())
			{
				dataStore.Conventions.DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites;

				Chart chart = new Chart(){Company = new EntityReference("companies/123", "CompanyDescription")};
				Chart chart2 = new Chart(){Company = new EntityReference("companies/456", "CompanyDescription")};

				using (var session = dataStore.OpenSession())
				{
					session.Store(chart);
					session.Store(chart2);
					session.SaveChanges();
				}

				// Assert
				using (var session = dataStore.OpenSession())
				{
					var charts = session.Query<Chart>()
						.SecurityFilter(chart.Company, null)
						.ToList();
					Assert.AreEqual(1, charts.Count());
					Assert.AreEqual(chart.Id, charts.Single().Id);
				}
			}
		}

		[Test]
		public void All_Charts_Are_Returned_For_Site_Administrator()
		{
			using (var dataStore = new EmbeddableDocumentStore { RunInMemory = true }.Initialize())
			{
				dataStore.Conventions.DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites;

				Chart chart = new Chart() { Company = new EntityReference("companies/123", "CompanyDescription") };
				Chart chart2 = new Chart() { Company = new EntityReference("companies/456", "CompanyDescription") };

				using (var session = dataStore.OpenSession())
				{
					session.Store(chart);
					session.Store(chart2);
					session.SaveChanges();
				}

				// Assert
				using (var session = dataStore.OpenSession())
				{
					var charts = session.Query<Chart>()
						.SecurityFilter(null, new[]{Roles.SiteAdministrator})
						.ToList();
					Assert.AreEqual(2, charts.Count());
				}
			}
		}

	}
}