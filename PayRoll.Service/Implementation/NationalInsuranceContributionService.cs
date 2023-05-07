using PayRoll.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayRoll.Service.Implementation
{
    public class NationalInsuranceContributionService : INationalInsuranceContributionService
    {
        private decimal NIRate;
        private decimal NIC;
        private readonly ApplicationDbContext db;

        public NationalInsuranceContributionService(ApplicationDbContext context)
        {
            db = context;
        }
        public decimal NIContribution(decimal totalAmount)
        {
            if (totalAmount < 719)
            {
                //Lower Earning limit Rate & below primary threshold
                NIRate = .0m;
                NIC = 0m;

            }
            else if (totalAmount >= 719 && totalAmount <= 4167)
            {
                //between primary threshold and Upper Earnings limit (UEL)
                NIRate = .12m;
                NIC = ((totalAmount - 719) * NIRate);
            }
            else if (totalAmount > 4167)
            {
                //Above Upper earnings limit (UEL)
                NIRate = .02m;
                NIC = ((4167 - 719) * .12m) + ((totalAmount - 4167) * NIRate);
            }
            return NIC;
        }
    }
}
