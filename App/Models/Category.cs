using System.Collections.Generic;

namespace EquipRentalPointApp.Models
{
    public class Category
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public List<Equipment>? Equipment { get; set; }
    }
}
