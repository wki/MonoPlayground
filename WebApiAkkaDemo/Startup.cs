using System;
using Owin;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Unity.WebApi;
using Akka.Actor;
using WebApiAkkaDemo.AppService;
using WebApiAkkaDemo.Domain;
using WebApiAkkaDemo.Domain.Measurement;

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
            var askableService = new AskableService(myActor);

            var sensorCoordinator = myActorSystem.ActorOf<SensorCoordinator>("Sensors");
            var measurementService = new MeasurementService(sensorCoordinator);

            var container = new UnityContainer();
            container.RegisterInstance<ActorSystem>(myActorSystem);
            container.RegisterInstance<IAskableService>(askableService);
            container.RegisterInstance<IMeasurementService>(measurementService);

            return container;
        }
    }
}
