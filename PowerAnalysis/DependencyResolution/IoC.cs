// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.Web;
using AppHarbor.Web.Security;
using HDC.PowerAnalysis.DAL;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using StructureMap;

namespace HDC.PowerAnalysis.Web.DependencyResolution
{
	public static class IoC
	{
		public static IContainer Initialize()
		{
			ObjectFactory.Initialize(x =>
			                         	{
			                         		x.Scan(scan =>
			                         		       	{
			                         		       		scan.TheCallingAssembly();
			                         		       		scan.WithDefaultConventions();
			                         		       	});

			                         		x.AddRegistry<PowerAnalysisDomainsRegistry>();

			                         		// Appharbor authentication:
			                         		x.For<HttpContextBase>()
			                         			.Use(() => new HttpContextWrapper(HttpContext.Current));
			                         		x.For<ICookieAuthenticationConfiguration>()
			                         			.Use<ConfigFileAuthenticationConfiguration>();
			                         		x.For<IAuthenticator>()
			                         			.Use<CookieAuthenticator>();

														x.ForSingletonOf<IDocumentStore>().Use(() =>
														{
															var RavenStore = new DocumentStore
															{
																ConnectionStringName =
																	"RavenDB"
															};
															RavenStore.Initialize();
															return RavenStore;
														});
			                         		// RavenDb:

			                         		x.For<IDocumentSession>().HybridHttpOrThreadLocalScoped().Use(
			                         			context =>
			                         				{
			                         					var session = context.GetInstance<IDocumentStore>().OpenSession();
			                         					session.Advanced.UseOptimisticConcurrency = true;
			                         					return new SessionDecorator(session, context.GetAllInstances<IStoreDecorator>());
			                         				});
			                         	});
			return ObjectFactory.Container;
		}

		public static IContainer InitializeForTests()
		{
			ObjectFactory.Initialize(x =>
			{
				x.Scan(scan =>
				{
					scan.TheCallingAssembly();
					scan.WithDefaultConventions();
				});

				x.AddRegistry<PowerAnalysisDomainsRegistry>();

				// Appharbor authentication:
				x.For<HttpContextBase>()
					.Use(() => new HttpContextWrapper(HttpContext.Current));
				x.For<ICookieAuthenticationConfiguration>()
					.Use<ConfigFileAuthenticationConfiguration>();
				x.For<IAuthenticator>()
					.Use<CookieAuthenticator>();

						x.For<IDocumentStore>().Use(() =>
						{

							var store = new EmbeddableDocumentStore { RunInMemory = true }.Initialize();
							store.Conventions.DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites;
							return store;
						});



				// RavenDb:

				x.For<IDocumentSession>().HybridHttpOrThreadLocalScoped().Use(
					context =>
					{
						var session = context.GetInstance<IDocumentStore>().OpenSession();
						session.Advanced.UseOptimisticConcurrency = true;
						return new SessionDecorator(session, context.GetAllInstances<IStoreDecorator>());
					});
			});
			return ObjectFactory.Container;
			
		}
	}
}