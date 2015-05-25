using Akka;
using System;
using Akka.Actor;

namespace SshAkkaDemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var myActorSystem = ActorSystem.Create("MyActorSystem");

            var sshProp = Props.Create<SshActor>();
            var sshActor = myActorSystem.ActorOf(sshProp, "ssh");

            sshActor.Tell(new SshActor.RunCommand("echo '1 hallo'; echo $$ >>/Users/wolfgang/Desktop/xxx.log; sleep 1; echo '1 err'>&2; sleep 2; echo '1 pause beendet'"));
            sshActor.Tell(new SshActor.RunCommand("echo $$ >>/Users/wolfgang/Desktop/xxx.log"));
            sshActor.Tell(new SshActor.RunCommand("echo '2 ende is mit schrecken'; sleep 2; echo '2 wirklich ende'; false"));


            myActorSystem.AwaitTermination();
        }
    }
}
