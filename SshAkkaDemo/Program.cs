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

            sshActor.Tell(new SshActor.RunCommand("echo 'hallo'; echo $$ >>/Users/wolfgang/Desktop/xxx.log; sleep 1; echo 'err'>&2; sleep 2; echo 'pause beendet'"));
            sshActor.Tell(new SshActor.RunCommand("echo $$ >>/Users/wolfgang/Desktop/xxx.log"));
            sshActor.Tell(new SshActor.RunCommand("echo 'ende is mit schrecken'; false"));


            myActorSystem.AwaitTermination();
        }
    }
}
