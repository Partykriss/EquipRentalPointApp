using System.Collections.Generic;

namespace EquipRentalPointApp.Models
{
    public class Equipment
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string? CategoryString { get; set; }
        public virtual List<Category>? Categories { get; set; }
    }
}
