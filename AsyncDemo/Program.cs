using System;
using System.Threading.Tasks;

namespace AsyncDemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Before entering each line, a computation will start");
            Console.WriteLine();

            while (true)
            {
                // Start computation.
                var task = Example();

                Console.WriteLine("Computation started. Thread: {0}", task.Id);

                // Handle user input.
                Console.WriteLine("Please enter a line of text ('wait' to wait for task):");
                string result = Console.ReadLine();
                Console.WriteLine("You typed: " + result);

                if (result == "wait")
                {
                    Console.WriteLine("Waiting for task {0}", task.Id);
                    task.Wait();
                    Console.WriteLine("Task {0} finished", task.Id);
                }
            }
        }

        static async Task Example()
        {
            // This method runs asynchronously.
            int t = await Task.Run(() => Allocate());
            Console.WriteLine("Computation: {0}", t);
        }

        static int Allocate()
        {
            // Compute total count of digits in strings.
            int size = 0;
            for (int z = 0; z < 100; z++)
            {
                for (int i = 0; i < 100000; i++)
                {
                    string value = i.ToString();
                    if (value == null)
                    {
                        return 0;
                    }
                    size += value.Length;
                }
            }
            return size;
        }
    }
}
