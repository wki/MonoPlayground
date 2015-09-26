Bestandteile:

Program
 	: startet OWIN via startup

Startup
 	: erzeugt OWIN Pipeline, 
 	  startet Akka.NET
 	  erzeugt Askable Service
 	  initialisiert DI mit actorsystem, askable Service

IAsklableService / AskableService
  	: Service, der mit einem Actor spricht, dessen ActorRef
  	  bekannt ist.

AskableActor
	: Aktor, der Fragen beantwortet

AskAQuestion
	: Frage-Command-Object

ActorController
	: Web API Controller /api/actor/(ask|sync|dummy)

StopwatchMiddleware
	: Zeitmessung innerhalb Actor Ablaufkette
	  erzeugt X-Elapsed-Milliseconds Response Header
