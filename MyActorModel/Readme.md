# Minimal Actor Model

## Klassen

 * ActorRefFactory = Elternklasse für ActorSystem und Actor
   -parent, -children
   +ActorOf
   +ActorFor

 * ActorSystem : ActorRefFactory

 * Actor : ActorRefFactory
   +Self
   +Sender
   -mailbox, -receivers
   #PreStart, #PreRestart, #PostStop, #PostRestart
   #Unhandled
   +Receive<T>(Action<T>)


 * ActorRef : IActorRef
   +Tell()
   +Ask()

 * Props
   +Create<T>(args)


## Hilfs-Klassen (internal)

 * Envelope = Message + Absender

 * ActorPath = Pfad eines Aktors

 * MessageHandler = Datentyp + Handler

