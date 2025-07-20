using System;
using System.Collections.Generic;

namespace EquipRentalPointApp.Models
{
    public class Rental
    {
        public int? Id { get; set; }
        public Client Client { get; set; }
        public List<Equipment> Equipments { get; set; }
        public string? EquipmentsString { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public List<Payment>? Payments { get; set; }
        public decimal Payed { get; set; }
        public decimal? Total { get; set; }
        public bool? isPaid { get; set; }
    }
}
