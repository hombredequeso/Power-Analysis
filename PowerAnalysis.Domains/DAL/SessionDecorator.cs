using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace HDC.PowerAnalysis.DAL
{
	public class SessionDecorator : IDocumentSession
	{
		private readonly IDocumentSession _innerSession;

		public SessionDecorator(IDocumentSession innerSession, IEnumerable<IStoreDecorator> storeDecorators)
		{
			_innerSession = innerSession;
			_storeDecorators = storeDecorators.ToList();
		}

		public void Dispose()
		{
			_innerSession.Dispose();
		}

		public void Delete<T>(T entity)
		{
			_innerSession.Delete(entity);
		}

		public T Load<T>(string id)
		{
			return _innerSession.Load<T>(id);
		}

		public T[] Load<T>(params string[] ids)
		{
			return _innerSession.Load<T>(ids);
		}

		public T[] Load<T>(IEnumerable<string> ids)
		{
			return _innerSession.Load<T>(ids);
		}

		public T Load<T>(ValueType id)
		{
			return _innerSession.Load<T>(id);
		}

		public T[] Load<T>(params ValueType[] ids)
		{
			return _innerSession.Load<T>(ids);
		}

		public T[] Load<T>(IEnumerable<ValueType> ids)
		{
			return _innerSession.Load<T>(ids);
		}

		public IRavenQueryable<T> Query<T>(string indexName)
		{
			return _innerSession.Query<T>(indexName);
		}

		public IRavenQueryable<T> Query<T>()
		{
			var query = _innerSession.Query<T>();
			return query;
		}

		public IRavenQueryable<T> Query<T, TIndexCreator>() where TIndexCreator : AbstractIndexCreationTask, new()
		{
			return _innerSession.Query<T, TIndexCreator>();
		}

		public ILoaderWithInclude<object> Include(string path)
		{
			return _innerSession.Include(path);
		}

		public ILoaderWithInclude<T> Include<T>(Expression<Func<T, object>> path)
		{
			return _innerSession.Include(path);
		}

		public ILoaderWithInclude<T> Include<T, TInclude>(Expression<Func<T, object>> path)
		{
			return _innerSession.Include<T, TInclude>(path);
		}

		public void SaveChanges()
		{
			_innerSession.SaveChanges();
		}

		public void Store(object entity, Guid etag)
		{
			_innerSession.Store(entity, etag);
		}

		public void Store(object entity, Guid etag, string id)
		{
			_innerSession.Store(entity, etag, id);
		}

		private readonly List<IStoreDecorator>  _storeDecorators; 

		public void Store(dynamic entity)
		{
			_storeDecorators.ForEach(x => x.Do(_innerSession, entity));
			//if (entity.GetType() == typeof(User))
			//{
			//    _innerSession.Store(UsernameReference.Make(entity.Username, entity.Id));
				
			//}
			_innerSession.Store(entity);
		}

		public void Store(dynamic entity, string id)
		{
			_innerSession.Store(entity, id);
		}

		public ISyncAdvancedSessionOperation Advanced
		{
			get { return _innerSession.Advanced; }
		}
	}
}