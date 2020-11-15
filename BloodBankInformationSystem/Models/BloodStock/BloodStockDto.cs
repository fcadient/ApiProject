using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankInformationSystem.Models.BloodStock
{
    public class BloodStockDto
    {
        public int Id { get; set; }
        public string bloodType { get; set; }
        public int bloodBankId { get; set; }
        public int quantity { get; set; }
        public DateTime bestBefore { get; set; }
        public string status { get; set; }
    }
}
