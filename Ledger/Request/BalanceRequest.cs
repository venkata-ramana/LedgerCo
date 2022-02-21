namespace Ledger.Request
{
    public class BalanceRequest : BaseRequest
    {
        public BalanceRequest(string bankName, string borrowerName, int emi)
        {
            BankName = bankName;
            BorrowerName = borrowerName;
            Emi = emi;
        }

        public string BankName { get; set; }
        public string BorrowerName { get; set; }
        public int Emi { get; set; }
    }
}
