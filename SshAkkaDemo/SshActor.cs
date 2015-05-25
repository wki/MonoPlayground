using Akka.Actor;
using System;
using Renci.SshNet;
using System.IO;

namespace SshAkkaDemo
{
    public class SshActor : ReceiveActor
    {
        #region Messages
        public class RunCommand
        {
            public string CommandLine { get; private set; }

            public RunCommand(string commandLine)
            {
                CommandLine = commandLine;
            }
        }

        public class ReceivedStdOut
        {
            public string Text { get; private set; }

            public ReceivedStdOut(string text)
            {
                Text = text;
            }
        }

        public class ReceivedStdErr
        {
            public string Text { get; private set; }

            public ReceivedStdErr(string text)
            {
                Text = text;
            }
        }

        public class FinishedCommand
        {
            public int ExitCode { get; private set; }

            public FinishedCommand(int exitCode)
            {
                ExitCode = exitCode;
            }
        }
        #endregion

        private SshClient sshClient;

        // TODO: behaviors disconnected, connected
        public SshActor()
        {
            // warning: takes time, is not responsive!
            sshClient = BuildSshClient();

            Receive<RunCommand>(RunRemoteCommand);
            Receive<ReceivedStdOut>(PrintStdOut);
            Receive<ReceivedStdErr>(PrintStdErr);
            Receive<FinishedCommand>(HandleFinished);
        }

        private void RunRemoteCommand(RunCommand runCommand)
        {
            var sshCommand = sshClient.CreateCommand(runCommand.CommandLine);

            var remoteCommandProp = Props.Create<RemoteCommandActor>(sshCommand);
            Context.ActorOf(remoteCommandProp);

            sshClient.ErrorOccurred += HandleErrorOccured;
            // TODO: if RemoteCommandActor is terminated, dispose command
        }

        void HandleErrorOccured(object sender, Renci.SshNet.Common.ExceptionEventArgs e)
        {
            Console.WriteLine("Error Occured: {0}", e);
        }

        private void PrintStdOut(ReceivedStdOut receiveStdOut)
        {
            Console.WriteLine("Stdout: {0}", receiveStdOut.Text);
        }   

        private void PrintStdErr(ReceivedStdErr receiveStdErr)
        {
            Console.WriteLine("Stderr: {0}", receiveStdErr.Text);
        }

        private void HandleFinished(FinishedCommand finishedCommand)
        {
            Console.WriteLine("Finished command, code: {0}", finishedCommand.ExitCode);

            Sender.Tell(PoisonPill.Instance);
        }

        #region connection management
        private static SshClient BuildSshClient()
        {
            var client = new SshClient(
                host: "localhost",
                username: "wolfgang",
                keyFiles: BuildPrivateKeyFiles()
            );

            client.Connect();

            return client;
        }

        private static PrivateKeyFile[] BuildPrivateKeyFiles()
        {
            var keyFile = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal), 
                ".ssh",
                "id_rsa"
            );

            return new []
            {
                new PrivateKeyFile(keyFile)
            };
        }
        #endregion
    }
}
