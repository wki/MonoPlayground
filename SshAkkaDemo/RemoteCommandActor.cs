using Akka.Actor;
using Renci.SshNet;
using System;
using System.IO;
// using System.Threading.Tasks;

namespace SshAkkaDemo
{
    public class RemoteCommandActor : ReceiveActor
    {
        private SshCommand sshCommand;
        // private Task task;

        public RemoteCommandActor(SshCommand sshCommand)
        {
            this.sshCommand = sshCommand;
        }

        // TODO: how to start and supervise the RunLoop
        protected override void PreStart()
        {
            // does not work:
            // task = Task.Run(RunLoop);
            RunLoop();
        }

        protected override void PostStop()
        {
            Console.WriteLine("Command Actor stop");
        }

        private void RunLoop()
        {
            var parent = Context.ActorSelection("..");

            var execution = sshCommand.BeginExecute();
            var stdoutReader = new StreamReader(sshCommand.OutputStream);
            var stderrReader = new StreamReader(sshCommand.ExtendedOutputStream);

            while (!execution.IsCompleted)
            {
                var result = stdoutReader.ReadToEnd();
                if (!String.IsNullOrEmpty(result))
                    parent.Tell(new SshActor.ReceivedStdOut(result));

                result = stderrReader.ReadToEnd();
                if (!String.IsNullOrEmpty(result))
                    parent.Tell(new SshActor.ReceivedStdErr(result));
            }

            sshCommand.EndExecute(execution);

            parent.Tell(new SshActor.FinishedCommand(sshCommand.ExitStatus));
            sshCommand.Dispose();
        }
    }
}
