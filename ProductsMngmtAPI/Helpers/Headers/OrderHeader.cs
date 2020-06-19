namespace ProductsMngmtAPI.Helpers.Headers
{
    public class OrderHeader
    {
        public bool ByProductName {get;set;}
        public bool ByProductNameDesc {get;set;}
        public bool ByCategoryName {get;set;}
        public bool ByCategoryNameDesc {get;set;}
        public bool ByExpDate {get;set;}
        public bool ByExpDateDesc {get;set;}

        public OrderHeader(bool byProductName, bool byProductNameDesc, bool byCategoryName, bool byCategoryNameDesc, bool byExpDate, bool byExpDateDesc)
        {

            ByProductName = byProductName;
            ByProductNameDesc = byProductNameDesc;
            ByCategoryName = byCategoryName;
            ByCategoryNameDesc = byCategoryNameDesc;
            ByExpDate = byExpDate;
            ByExpDateDesc = byExpDateDesc;
        }
    }
}