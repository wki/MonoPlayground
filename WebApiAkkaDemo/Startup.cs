using System;
using Owin;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Unity.WebApi;
using Akka.Actor;
using System.Diagnostics;


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

            // app.Use<StopwatchMiddleware>();

            app.UseWebApi(config);
        }

        /* Idee zur Erweiterung
         * 
         * interface IAskableService
         *   async string Ask(string question);
         * 
         * class AskableService : IAskableService
         *   --> kennen der ActorRef auf AskableActor
         *   --> Kapselung der Abfrage aus dem Controller
         * 
         * Instantiierung AskableService
         * 
         * Registrierung AskableService als IAskableService in Unity
         * 
         * Benutzen von IAskableService in Controller
         */


        private IUnityContainer SetupContainer()
        {
            var myActorSystem = ActorSystem.Create("MyActorSystem");
            var myActor = myActorSystem.ActorOf<AskableActor>("Askable");
            Console.WriteLine("Created actor: {0} ({1})", myActor.Path, myActor.GetType().FullName);

            var container = new UnityContainer();
            container.RegisterInstance<ActorSystem>(myActorSystem);

            return container;
        }
    }
}
