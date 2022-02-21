using Ledger.Constants;
using Ledger.Processors;

namespace Ledger.Factories
{
    public static class ProcessorFactory
    {
        public static IProcessor CreateProcessor(ProcessorType processorType, object args)
        {
            IProcessor processor = null;
            switch (processorType)
            {
                case ProcessorType.FileProcessor:
                default:
                    processor = new FileProcessor((string)args);
                    break;
            }

            return processor;
        }
    }
}
