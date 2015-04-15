using System;
using Renci.SshNet;
using System.IO;

namespace SshDemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            SshConnectDemo();
        }

        public static void SshConnectDemo()
        {
            using (var client = BuildSshClient())
            {
                client.Connect();

                SyncCommandExecution(client);
                AsyncCommandExecution(client);
            }
        }

        private static SshClient BuildSshClient()
        {
            return new SshClient(
                host: "localhost",
                username: "wolfgang",
                keyFiles: new PrivateKeyFile[]
                {
                    new PrivateKeyFile("/Users/wolfgang/.ssh/id_rsa")
                });
        }

        private static void SyncCommandExecution(SshClient client)
        {
            using (var command = client.CreateCommand("echo 'hallo'; echo 'error No 42' >&2; false"))
            {
                var result = command.Execute();
                var errorReader = new StreamReader(command.ExtendedOutputStream);

                Console.WriteLine("sync execution done, stdout = {0}, stderr = {1}, status = {2}", 
                    result,
                    errorReader.ReadToEnd(),
                    command.ExitStatus
                );
            }
        }

        private static void AsyncCommandExecution(SshClient client)
        {
            using (var command = client.CreateCommand("echo 'hallo'; sleep 2; echo 'err'>&2; sleep 10; echo 'pause beendet'"))
            {
                var async = command.BeginExecute();
                var stdoutReader = new StreamReader(command.OutputStream);
                var stderrReader = new StreamReader(command.ExtendedOutputStream);

                while (!async.IsCompleted)
                {
                    var result = stdoutReader.ReadToEnd();
                    if (!String.IsNullOrEmpty(result))
                        Console.WriteLine(result);

                    result = stderrReader.ReadToEnd();
                    if (!String.IsNullOrEmpty(result))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ERR: {0}", result);
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                }

                command.EndExecute(async);

                Console.WriteLine("async execution done, status = {0}", command.ExitStatus);
            }
        }
    }
}