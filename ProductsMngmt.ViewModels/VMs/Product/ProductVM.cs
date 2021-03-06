using System;
using ProductsMngmt.ViewModels.VMs.Category;

namespace ProductsMngmt.ViewModels.VMs.Product
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barecode { get; set; }
        public int MaxDays { get; set; }
        public int CategoryId { get; set; }
        public CategorySimpleVM Category { get; set; }
        public string Image { get; set; }
        public DateTime NearestDate { get; set; }
    }
}