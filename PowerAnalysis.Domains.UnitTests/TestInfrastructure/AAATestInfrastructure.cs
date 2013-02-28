using System;
using HDC.PowerAnalysis.DAL;
using HDC.PowerAnalysis.Web.DependencyResolution;
using NUnit.Framework;
using Raven.Client;
using StructureMap;

namespace PowerAnalysis.Domains.UnitTests.TestInfrastructure
{
	public class AAATestInfrastructure
	{
		static AAATestInfrastructure()
		{
			IoC.InitializeForTests();
		}

		[TestFixtureTearDown]
		public void FixtureTearDownAttribute()
		{
			_store.Dispose();
		}

		private IDocumentSession CreateNewSession(IDocumentStore store)
		{
			var session = store.OpenSession();
			session.Advanced.UseOptimisticConcurrency = true;
			return new SessionDecorator(session, ObjectFactory.GetAllInstances<IStoreDecorator>());
		}

		[TestFixtureSetUp]
		public void GivenFixtureSetup()
		{
			_store = ObjectFactory.GetInstance<IDocumentStore>();
			if (_given != null)
			{
				using (_session = CreateNewSession(_store))
				{
					_session.Advanced.UseOptimisticConcurrency = true; 
					_given();
					_session.SaveChanges();
				}
			}
		}

		protected IDocumentStore _store;
		protected IDocumentSession _session;

		protected Action _given;

		protected void Arrange(Action arrange)
		{
			using (_session = CreateNewSession(_store))
			{
				arrange();
				_session.SaveChanges();
			}
		}

		protected void Act(Action act)
		{
			using (_session = CreateNewSession(_store))
			{
				act();
				_session.SaveChanges();
			}
		}

		protected T ActThrows<T>(Action act) where T:Exception 
		{
			try
			{
				Act(act);
			}
			catch (Exception e)
			{
				Assert.IsInstanceOf<T>(e);
				return (T)e;
			}
			Assert.Fail("Act failed to throw expected exception of type " + typeof(T));
			return null;
		}

		protected void AssertThat(Action assert)
		{
			using (_session = CreateNewSession(_store))
			{
				assert();
				_session.SaveChanges();
			}
		}
	}
}