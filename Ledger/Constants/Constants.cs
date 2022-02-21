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
    }

    public static class Actions
    {
        public const string Loan = "LOAN";
        public const string Payment = "PAYMENT";
        public const string Balance = "BALANCE";
    }
}
