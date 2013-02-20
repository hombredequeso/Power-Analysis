using System;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace PowerAnalysis.Domains.UnitTests.TestInfrastructure
{
	public class AAATestInfrastructure
	{
		public AAATestInfrastructure()
		{
			_store = new EmbeddableDocumentStore {RunInMemory = true}.Initialize();
			_store.Conventions.DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites;
		}

		[TestFixtureTearDown]
		public void FixtureTearDownAttribute()
		{
			_store.Dispose();
		}

		[TestFixtureSetUp]
		public void GivenFixtureSetup()
		{
			if (_given != null)
			{
				using (_session = _store.OpenSession())
				{
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
			using (_session = _store.OpenSession())
			{
				arrange();
				_session.SaveChanges();
			}
		}

		protected void Act(Action act)
		{
			using (_session = _store.OpenSession())
			{
				act();
				_session.SaveChanges();
			}
		}

		protected void AssertThat(Action assert)
		{
			using (_session = _store.OpenSession())
			{
				assert();
				_session.SaveChanges();
			}
		}
	}
}