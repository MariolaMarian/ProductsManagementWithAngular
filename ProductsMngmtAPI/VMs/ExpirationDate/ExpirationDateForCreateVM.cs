using System;

namespace ProductsMngmtAPI.VMs.ExpirationDate
{
    public class ExpirationDateForCreateVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public DateTime EndDate{get;set;}
    }
}