using System;
using System.Diagnostics;
using Renci.SshNet;
using System.IO;
using System.Linq;

namespace SshDemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            MeasureTime("Only Ssh Connect", () => DummySshConnect());
            SshConnectDemo();
            MeasureTime("Only Sftp Connect", () => DummySftpConnect());
            // MeasureTime("Sync Directories", () => SyncDirectories());
        }

        public static void MeasureTime(string name, Action callback)
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            callback();
            stopWatch.Stop();

            Console.WriteLine("Time for executing {0}: {1:N3}s", name, stopWatch.Elapsed.TotalSeconds);
            Console.WriteLine("-------");
            Console.WriteLine();
        }

        public static void DummySshConnect()
        {
            using (var client = BuildSshClient())
            {
            }
        }

        public static void DummySftpConnect()
        {
            using (var client = BuildSftpClient())
            {
            }
        }

        public static void SyncDirectories()
        {
            using (var client = BuildSftpClient())
            {
                var files = client.SynchronizeDirectories("/Users/wolfgang/tmp/MyApp", "/Users/wolfgang/scripts", "*");
                Console.WriteLine(String.Join(", ", files.Select(f => f.Name)));
            }
        }

        public static void SshConnectDemo()
        {
            using (var client = BuildSshClient())
            {
                MeasureTime("Ssh Syncronous first time", () => SyncCommandExecution(client));
                MeasureTime("Ssh Syncronous second time", () => SyncCommandExecution(client));
                MeasureTime("Ssh Syncronous third time", () => SyncCommandExecution(client));
                MeasureTime("Ssh Asyncronous", () => AsyncCommandExecution(client));
            }
        }

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

        private static SftpClient BuildSftpClient()
        {
            var client = new SftpClient(
                host: "localhost",
                username: "wolfgang",
                keyFiles: BuildPrivateKeyFiles()
            );

            client.Connect();

            return client;
        }

        private static PrivateKeyFile[] BuildPrivateKeyFiles()
        {
            var sshDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".ssh");
            var keyFile = Path.Combine(sshDir, "id_rsa");

            return new []
            {
                new PrivateKeyFile(keyFile)
            };
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
            using (var command = client.CreateCommand("echo 'hallo'; sleep 1; echo 'err'>&2; sleep 2; echo 'pause beendet'"))
            {
                var ssh = command.BeginExecute();
                var stdoutReader = new StreamReader(command.OutputStream);
                var stderrReader = new StreamReader(command.ExtendedOutputStream);

                while (!ssh.IsCompleted)
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

                command.EndExecute(ssh);

                Console.WriteLine("async execution done, status = {0}", command.ExitStatus);
            }
        }
    }
}