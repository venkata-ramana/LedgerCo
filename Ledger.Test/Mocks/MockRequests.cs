using Bogus;
using Ledger.Request;
using System;
using System.Collections.Generic;

namespace Ledger.Test.Mocks
{
    public static class MockRequests
    {
        public static List<Tuple<string, string>> _requestedLoanIds = new List<Tuple<string, string>>();

        public static Faker<LoanRequest> _loanFaker = new Faker<LoanRequest>()
                                                        .CustomInstantiator(f => new LoanRequest(
                                                            f.Finance.Bic(),
                                                            f.Name.FullName(),
                                                            f.Finance.Amount(),
                                                            f.Random.Number(1, 4),
                                                            f.Random.Decimal(5, 10)
                                                        ))
                                                        .FinishWith((f, o) => _requestedLoanIds.Add(new Tuple<string, string>(o.BankName, o.BorrowerName)));
    }
}
