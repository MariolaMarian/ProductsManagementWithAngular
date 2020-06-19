using System;
using ProductsMngmtAPI.VMs.Product;
using ProductsMngmtAPI.VMs.User;

namespace ProductsMngmtAPI.VMs.ExpirationDate
{
    public class ExpirationDateVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ProductForExpirationDateVM Product { get; set; }
        public DateTime EndDate{get;set;}
        public bool Collected { get; set; }
        public int? Count { get; set; }
        public DateTime CollectedDate { get; set; }
        public string CollectedById { get; set; }
        public UserSimpleVM CollectedBy { get; set; }
    }
}