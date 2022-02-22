using Ledger.Exceptions;
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
        private PaymentRequest paymentRequest { get; set; }

        private ILoanService LoanService { get; set; }

        private IPaymentService PaymentService { get; set; }

        public PaymentRequestHandler(ILoanService LoanService,IPaymentService paymentService)
        {
            this.LoanService = LoanService;
            this.PaymentService = paymentService;
        }

        public void SetRequest(BaseRequest request)
        {
            this.paymentRequest = (PaymentRequest)request;
        }

        public bool ValidateRequest()
        {
            if (String.IsNullOrEmpty(paymentRequest.BankName))
                throw new ArgumentException($"{Constants.Actions.Payment}: {Constants.ErrorMessages.BankNameRequired}");

            if (String.IsNullOrEmpty(paymentRequest.BorrowerName))
                throw new ArgumentException($"{Constants.Actions.Payment}: {Constants.ErrorMessages.BorrowerNameRequired}");

            if (paymentRequest.Emi <= 0)
                throw new ArgumentException($"{Constants.Actions.Payment}: {Constants.ErrorMessages.InvalidEmi}");

            if (paymentRequest.LumpsumAmount <= 0)
                throw new ArgumentException($"{Constants.Actions.Payment}: {Constants.ErrorMessages.InvalidLumpsumAmount}");

            return true;
        }

        public async Task<BaseResponse> ProcessAsync()
        {
            var existingLoanRecord = await LoanService.GetLoanDetailsAsync(paymentRequest.BankName, paymentRequest.BorrowerName);
            if (existingLoanRecord == null)
                throw new RecordNotFoundException(Constants.ErrorMessages.LoanRecordNotFound);

            var totalValidEmis = existingLoanRecord.TotalNoOfEmi;
            if (paymentRequest.Emi > totalValidEmis)
                throw new ArgumentException(Constants.ErrorMessages.InvalidEmi);
            var payment = paymentRequest.ToPaymentModel();
            var isSaved = await PaymentService.SavePaymentAsync(paymentRequest.BankName, paymentRequest.BorrowerName, payment);
            return new BaseResponse() { IsSuccess = isSaved };
        }
    }
}
