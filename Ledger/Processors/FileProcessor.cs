using Ledger.Factories;
using Ledger.Handlers;
using Ledger.Helpers;
using Ledger.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ledger.Processors
{
    public class FileProcessor : IProcessor
    {
        private readonly string _filePath;

        private RequestHandlerFactory requestHandlerFactory;

        public FileProcessor(string filepath)
        {
            if (string.IsNullOrWhiteSpace(filepath))
            {
                throw new ArgumentException(Constants.ErrorMessages.FilePathError);
            }
            _filePath = filepath;

            requestHandlerFactory = new RequestHandlerFactory();
        }

        public async Task ProcessAsync()
        {
            var commands = GetCommands();
            foreach (string command in commands)
            {
                string[] args = command.Split(" ");
                if (args == null || args.Length == 0)
                    Utils.ConsoleLogError(Constants.ErrorMessages.InvalidCommand);

                var handler = requestHandlerFactory.GetRequestHandler(args[0], args);
                if (handler == null)
                    Utils.ConsoleLogError(Constants.ErrorMessages.InvalidCommand);

                if (handler.ValidateRequest())
                {
                    var response = await handler.ProcessAsync();
                    if (handler.GetType() == typeof(BalanceRequestHandler) && response.IsSuccess)
                    {
                        var balanceResponse = (BalanceResponse)response;
                        Utils.ConsoleLogSuccess($"{balanceResponse.BankName} {balanceResponse.BorrowerName} {balanceResponse.AmountPaid} {balanceResponse.RemainingEmis}");
                    }
                }
            }
        }

        private IEnumerable<string> GetCommands()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var fullFilePath = Path.Combine(basePath, Constants.File.InputSourceFolder, _filePath);

            if (!File.Exists(fullFilePath))
            {
                throw new FileNotFoundException(Constants.ErrorMessages.FileNotFound);
            }

            IEnumerable<string> fileCommands = File.ReadAllLines(fullFilePath);
            if (fileCommands == null || !fileCommands.Any())
            {
                throw new ArgumentException(Constants.ErrorMessages.CommandNotFound);
            }
            return fileCommands;
        }
    }
}
