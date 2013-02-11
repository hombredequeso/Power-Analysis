using HDC.PowerAnalysis.Core;

namespace HDC.PowerAnalysis.Security
{
	public class Company
	{
		public static implicit operator EntityReference(Company company)
		{
			return new EntityReference(company.Id, company.Description);
		}

		public Company(string description)
		{
			Description = description;
		}

		public string Id { get; private set; }
		public string Description { get; private set; }
	}
}