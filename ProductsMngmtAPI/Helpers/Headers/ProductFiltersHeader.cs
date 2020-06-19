using System.Collections.Generic;

namespace ProductsMngmtAPI.Helpers.Headers
{
    public class ProductFiltersHeader
    {
        public string Barecode { get; set; }
        public string ProductName { get; set; }
        public List<int> CategoriesIds { get; set; }

        public ProductFiltersHeader(string barecode, string productName, List<int> categoriesIds)
        {
            Barecode = barecode;
            ProductName = productName;
            if(categoriesIds != null)
                CategoriesIds = categoriesIds;
            else
                CategoriesIds = new List<int>();
        }
    }
}