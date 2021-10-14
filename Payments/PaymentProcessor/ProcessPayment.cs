using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentProcessor
{
    public class ProcessPayment : IProcessPayment
    {
        public Task<bool> Pay()
        {
            throw new NotImplementedException();
        }
    }
}
