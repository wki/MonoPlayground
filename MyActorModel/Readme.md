# Minimal Actor Model

This is an experiment not intended for production use.
Instead, it is meant as a playground for experimenting with the issues arising when
creating an actor system. At its heart Reactive Extensions are doing the heavy lifting.

What is omitted:
 * Context (most things are in Node)
 * SupervisorStrategy

we carefully handle
 * ActorSystem.ActorOf erzeugt /user/xxx Actor
 * actor.ActorOf erzeugt Kind-Actor

Initial tree structure:
 * /system/deadLetters
 * /system/log
 * /user


## Classes

 * Node : IActorRefFactory = Parent for ActorSystem and Actor
       -Parent, -Children
       -ChildRootPath == Root-path for children (usually: Self, except /: "/user")
       #DeadLetterActor, #LogActor
       +ActorOf : IActorRef
       +ActorFor : IActorRef

 * ActorSystem : Node
 	   +Name

 * Actor : Node
       +Self
       +Sender
       -Mailbox, -Receivers
       #PreStart, #PreRestart, #PostStop, #PostRestart
       #Unhandled
       +Receive<T>(Action<T>)
       #Become, #Unbecome

 * ActorRef : IActorRef
       -Actor
       +Path
       +Tell()
       +Ask()

 * Props<T>


## Helper classes (internal)

 * Envelope = Message + Sender

 * ActorPath = representation of a path for an actor

 * MessageHandler = Data type and Handler
