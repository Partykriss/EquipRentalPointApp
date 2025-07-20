using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipRentalPointApp.Models
{
    public class QueryD
    {
        public string EquipmentsString { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public decimal SumPrice { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public decimal Payed { get; set; }
        public decimal Total { get; set; }
        public decimal ToPay { get; set; }
    }
}
