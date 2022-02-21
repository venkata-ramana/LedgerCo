using Ledger.Mapper;
using Ledger.Request;
using Ledger.Response;
using Ledger.Service;
using System;
using System.Threading.Tasks;

namespace Ledger.Handlers
{
    public class PaymentRequestHandler : IRequestHandler
    {
        public PaymentRequest PaymentRequest { get; set; }

        private ILoanService LoanService { get; set; }

        private IPaymentService PaymentService { get; set; }

        public PaymentRequestHandler(ILoanService LoanService,IPaymentService paymentService)
        {
            this.LoanService = LoanService;
            this.PaymentService = paymentService;
        }

        public async Task<BaseResponse> ProcessAsync()
        {
            var existingLoanRecord = await LoanService.GetLoanDetailsAsync(PaymentRequest.BankName, PaymentRequest.BorrowerName);
            if (existingLoanRecord == null)
                throw new ArgumentException(Constants.ErrorMessages.LoanRecordNotFound);

            var totalValidEmis = existingLoanRecord.LoanTenure * 12;
            if (PaymentRequest.Emi > totalValidEmis)
                throw new ArgumentException(Constants.ErrorMessages.InvalidEmi);
            var payment = PaymentRequest.ToPaymentModel();
            var isSaved = await PaymentService.SavePaymentAsync(PaymentRequest.BankName, PaymentRequest.BorrowerName, payment);
            return new BaseResponse() { IsSuccess = isSaved };
        }
    }
}
