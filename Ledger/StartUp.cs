using Ledger.Constants;
using Ledger.Factories;
using Ledger.Processors;
using System;
using System.Threading.Tasks;

namespace Ledger
{
    internal class StartUp
    {
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("No file path specified as input");
                }

                ProcessCommands(args).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occured {ex.Message}");
            }
        }

        private static async Task ProcessCommands(string[] args)
        {
            IProcessor processor = ProcessorFactory.CreateProcessor(ProcessorType.FileProcessor, args[0]);
            await processor.ProcessAsync();
        }
    }
}
