﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Ledger.Models
{
    public class LoanDetail
    {
        public string BankName { get; set; }
        public string BorrowerName { get; set; }
        public decimal PrincipalAmount { get; set; }
        public int LoanTenure { get; set; }
        public decimal RateOfInterest { get; set; }

        public List<Payment> Payments { get; set; }

        public DateTime CreateDate { get; set; }

        public int TotalNoOfEmi
        {
            get
            {
                return this.LoanTenure * 12;
            }
        }

        public decimal TotalAmountToBeRepaid
        {
            get
            {
                if (this.LoanTenure > 0)
                    return this.PrincipalAmount + ((this.PrincipalAmount * this.LoanTenure * this.RateOfInterest) / 100);
                else
                    return 0;
            }
        }

        public decimal EmiAmount
        {
            get
            {
                var totalAmountToBeRepaid = TotalAmountToBeRepaid;
                if (totalAmountToBeRepaid > 0)
                    return Math.Ceiling(totalAmountToBeRepaid / this.TotalNoOfEmi);
                else
                    return 0;
            }
        }

        public decimal LumpSumPaidTillEmiNumber(int emiNumber)
        {
            if (this.Payments != null && this.Payments.Count > 0)
            {
                return this.Payments.Where(x => x.EmiNumber <= emiNumber).Sum(x => x.Amount);
            }
            return 0;
        }
    }

    public class Payment
    {
        public int EmiNumber { get; set; }
        public decimal Amount { get; set; }

        public Payment() { }

        public Payment(int emiNumber, int amount)
        {
            this.EmiNumber = emiNumber;
            this.Amount = amount;
        }
    }
}
