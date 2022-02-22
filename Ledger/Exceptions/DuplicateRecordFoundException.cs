using System;

namespace Ledger.Exceptions
{
    public class DuplicateRecordFoundException: Exception
    {
        public DuplicateRecordFoundException(String message) : 
            base(message)
        {

        }
    }
}
