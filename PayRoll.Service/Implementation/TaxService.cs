using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRoll.Service.Implementation
{
    public class TaxService : ITaxService
    {
        private decimal taxRate;
        private decimal tax;
        public decimal TaxAmount(decimal totalAmount)
        {
            if(totalAmount <= 48535)
            {

                taxRate = 0.15m;
                tax = totalAmount * taxRate;
            }
            else if(totalAmount > 48535 && totalAmount < 97069)
            {
                taxRate = .205m;
                tax = (totalAmount * taxRate);
            }else if(totalAmount> 97069 && totalAmount< 150473)
            {
                taxRate = 0.26m;
                tax = (totalAmount * taxRate);
            }
            else if (totalAmount > 150474 && totalAmount < 214368)
            {
                taxRate = 0.29m;
                tax = (totalAmount * taxRate);
            }else if(totalAmount > 214368 )
            {
                taxRate = 0.33m;
                tax = (totalAmount * taxRate);

            }
            return tax;
        }
    }
}
