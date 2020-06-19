namespace ProductsMngmtAPI.VMs.Category
{
    public class CategoryWithCountsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductsCount {get;set;}
    }
}