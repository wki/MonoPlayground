using System;
using Owin;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Unity.WebApi;
using Akka.Actor;
using WebApiAkkaDemo.AppService;
using WebApiAkkaDemo.Domain;

namespace WebApiAkkaDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.DependencyResolver = new UnityDependencyResolver(SetupContainer());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Api",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.Use<StopwatchMiddleware>();

            app.UseWebApi(config);
        }
            
        private IUnityContainer SetupContainer()
        {
            var myActorSystem = ActorSystem.Create("MyActorSystem");
            var myActor = myActorSystem.ActorOf<AskableActor>("Askable");
            Console.WriteLine("Created actor: {0} ({1})", myActor.Path, myActor.GetType().FullName);

            var askableService = new AskableService(myActor);

            var container = new UnityContainer();
            container.RegisterInstance<ActorSystem>(myActorSystem);
            container.RegisterInstance<IAskableService>(askableService);

            return container;
        }
    }
}
