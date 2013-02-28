using System;

namespace HDC.PowerAnalysis.Core
{
	public class EntityReference : IEquatable<EntityReference>
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

		public bool Equals(EntityReference other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(other.Id, Id);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (EntityReference)) return false;
			return Equals((EntityReference) obj);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public static bool operator ==(EntityReference left, EntityReference right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(EntityReference left, EntityReference right)
		{
			return !Equals(left, right);
		}
	}
}