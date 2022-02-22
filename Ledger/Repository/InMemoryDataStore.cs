using Ledger.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ledger.Repository
{
    public class InMemoryDataStore : IDataStore
    {
        private static readonly Dictionary<Tuple<string, string>, LoanDetail> _loanRecords = new Dictionary<Tuple<string, string>, LoanDetail>();

        public async Task<LoanDetail> GetLoanDetailsAsync(string bankName, string borrowerName)
        {
            var loanRecordKey = GetLoanRecordKey(bankName, borrowerName);
            return await GetLoanRecordAsync(loanRecordKey);
        }

        public async Task<bool> SaveLoanDetailsAsync(LoanDetail loanDetail)
        {
            var loanRecordKey = GetLoanRecordKey(loanDetail.BankName, loanDetail.BorrowerName);
            return _loanRecords.TryAdd(loanRecordKey, loanDetail);
        }

        public async Task<bool> SavePaymentAsync(string bankName, string borrowerName, Payment payment)
        {
            var loanRecordKey = GetLoanRecordKey(bankName, borrowerName);
            var existingLoanDetails = await GetLoanRecordAsync(loanRecordKey);

            if (existingLoanDetails == null)
                return false;

            if (existingLoanDetails.Payments == null)
                existingLoanDetails.Payments = new List<Payment>();

            existingLoanDetails.Payments.Add(payment);
            return true;
        }

        public Task<LoanDetail> GetLoanRecordAsync(Tuple<string, string> loanRecordKey)
        {
            LoanDetail existingLoanDetails;
            if (_loanRecords.TryGetValue(loanRecordKey, out existingLoanDetails))
            {
                return Task.FromResult(existingLoanDetails);
            }
            return Task.FromResult(existingLoanDetails);
        }

        private Tuple<string, string> GetLoanRecordKey(string bankName, string borrowerName)
        {
            return new Tuple<string, string>(bankName, borrowerName);
        }
    }
}