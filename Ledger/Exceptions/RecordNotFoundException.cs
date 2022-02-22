using System;

namespace Ledger.Exceptions
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException(string message) : 
            base(message)
        {
        }
    }
}
