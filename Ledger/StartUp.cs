using Ledger.Constants;
using Ledger.Factories;
using Ledger.Handlers;
using Ledger.Helpers;
using Ledger.Processors;
using Ledger.Service;
using SimpleInjector;
using System;
using System.Threading.Tasks;

namespace Ledger
{
    public static class DependencyResolver
    {
        public static Container container;

        public static T Resolve<T>() where T: class
        {
            return container.GetInstance<T>();
        }
    }

    internal class StartUp
    {
        private static Container container;

        public static void Main(string[] args)
        {
            container = new Container();
            container.Register<ILoanService, LoanService>();
            container.Register<IPaymentService, PaymentService>();
            container.Register<LoanRequestHandler>();
            container.Register<PaymentRequestHandler>();
            container.Register<BalanceRequestHandler>();
            container.Verify();
            DependencyResolver.container = container;

            Init(args);
        }

        private static void Init(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Utils.ConsoleLogError(ErrorMessages.ProvideInput);
                }

                ProcessCommands(args).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Utils.ConsoleLogError(ex.Message);
            }
        }

        private static async Task ProcessCommands(string[] args)
        {
            IProcessor processor = ProcessorFactory.CreateProcessor(ProcessorType.FileProcessor, args[0]);
            await processor.ProcessAsync();
        }
    }
}
