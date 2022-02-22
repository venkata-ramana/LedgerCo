namespace Ledger.Constants
{
    public static class File
    {
        public const string InputSourceFolder = "FileInput";
    }

    public static class ErrorMessages
    {
        public static string CommandNotFound = "No command found to execute";
        public static string FileNotFound = "File not found at given location";
        public static string FilePathError = "Need valid file path";
        public static string LoanRecodExists = "Loan Record already exists";
        public static string LoanRecordNotFound = "Loan Record not found";
        public static string InvalidEmi = "Requested Emi is invalid";
        public static string ProvideInput = "Please provide inputs";
        public static string InvalidCommand = "Command not identified";
        public static string DuplicateLoanRecord = "Loan record already found. Bank Name: {0}, Customer Name: {1}";
    }

    public static class Actions
    {
        public const string Loan = "LOAN";
        public const string Payment = "PAYMENT";
        public const string Balance = "BALANCE";
    }
}
