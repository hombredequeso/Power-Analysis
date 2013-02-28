using HDC.PowerAnalysis.Security;
using Raven.Client;

namespace HDC.PowerAnalysis.DAL
{
	public class UserNameUniqueDecorator: IStoreDecorator
	{
		public void Do(IDocumentSession session, dynamic entity)
		{
			if (entity.GetType() == typeof(User))
			{
				session.Store(UsernameReference.Make(entity.Username, entity.Id));

			}
		}
	}

	public class UsernameReference
	{
		public static UsernameReference Make(string username, string userId)
		{
			return new UsernameReference("UniqueConstraints/User/Username/" + username, userId);
		}

		public UsernameReference(string id, string userId)
		{
			Id = id;
			UserId = userId;
		}

		public string Id { get; private set; }
		public string UserId { get; private set; }

	}

}