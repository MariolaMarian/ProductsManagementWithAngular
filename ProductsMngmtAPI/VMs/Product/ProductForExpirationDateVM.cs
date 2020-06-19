namespace ProductsMngmtAPI.VMs.Product
{
    public class ProductForExpirationDateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set;}
        public int MaxDays{get;set;}
    }
}