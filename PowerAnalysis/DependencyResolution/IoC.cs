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
using Raven.Client;
using Raven.Client.Document;
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

			                         		// Appharbor authentication:
			                         		x.For<HttpContextBase>()
			                         			.Use(() => new HttpContextWrapper(HttpContext.Current));
			                         		x.For<ICookieAuthenticationConfiguration>()
			                         			.Use<ConfigFileAuthenticationConfiguration>();
			                         		x.For<IAuthenticator>()
			                         			.Use<CookieAuthenticator>();

			                         		// RavenDb:
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

			                         		// register RavenDB document session
			                         		x.For<IDocumentSession>().HybridHttpOrThreadLocalScoped().Use(
			                         			context => { return context.GetInstance<IDocumentStore>().OpenSession(); });
			                         	});
			return ObjectFactory.Container;
		}
	}
}