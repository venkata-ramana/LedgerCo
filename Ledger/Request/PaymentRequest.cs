namespace Ledger.Request
{
    public class PaymentRequest : BaseRequest
    {
        public PaymentRequest(string bankName, string borrowerName, decimal lumpsumAmount, int emi)
        {
            BankName = bankName;
            BorrowerName = borrowerName;
            LumpsumAmount = lumpsumAmount;
            Emi = emi;
        }

        public string BankName { get; set; }
        public string BorrowerName { get; set; }
        public decimal LumpsumAmount { get; set; }
        public int Emi { get; set; }
    }
}
