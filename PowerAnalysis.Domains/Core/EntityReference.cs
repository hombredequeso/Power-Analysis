using System;

namespace HDC.PowerAnalysis.Core
{
	public class EntityReference
	{
		public EntityReference(string id, string description)
		{
			if (id == null) throw new ArgumentNullException("id");
			if (description == null) throw new ArgumentNullException("description");
			if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id");
			if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("description");

			Id = id;
			Description = description;
		}

		public string Id { get; private set; }
		public string Description { get; private set; }
	}
}