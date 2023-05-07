using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRoll.Service
{
   public interface INationalInsuranceContributionService
    {
        decimal NIContribution(decimal totalAmount);
    }
}
