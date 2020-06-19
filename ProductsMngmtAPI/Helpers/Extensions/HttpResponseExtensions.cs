using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductsMngmtAPI.Helpers.Headers;

namespace ProductsMngmtAPI.Helpers.Extensions
{
    public static class HttpResponseExtensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            string newHeader = "Application-Error";
            response.Headers.Add("Access-Control-Expose-Headers", newHeader);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.UpdateExposeHeaders(newHeader);
        }

        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            PaginationHeader paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            string newHeader = "Pagination";
            response.Headers.Add(newHeader, JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", newHeader);
            response.UpdateExposeHeaders(newHeader);
        }

        public static void AddProductsFilters(this HttpResponse response, string barecode = "", string productName = "", List<int> categoriesIds = null)
        {
            ProductFiltersHeader productFiltersHeader = new ProductFiltersHeader(barecode, productName, categoriesIds);
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            string newHeader = "ProductFilters";
            response.Headers.Add(newHeader, JsonConvert.SerializeObject(productFiltersHeader, camelCaseFormatter));
            response.UpdateExposeHeaders(newHeader);
        }

        public static void AddOrder(this HttpResponse response, bool byProductName = false, bool byProductNameDesc = false, bool byCategoryName = false, bool byCategoryNameDesc = false, bool byExpDate = false, bool byExpDateDesc = false)
        {
            OrderHeader orderHeader = new OrderHeader(byProductName, byProductNameDesc,  byCategoryName, byCategoryNameDesc, byExpDate, byExpDateDesc);
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            string newHeader = "Order";
            response.Headers.Add(newHeader, JsonConvert.SerializeObject(orderHeader, camelCaseFormatter));
            response.UpdateExposeHeaders(newHeader);
        }

        private static void UpdateExposeHeaders(this HttpResponse response, string newHeader)
        {
            if(response.Headers.ContainsKey("Access-Control-Expose-Headers")){
                var tmp = response.Headers["Access-Control-Expose-Headers"].ToString();
                response.Headers.Remove("Access-Control-Expose-Headers");
                response.Headers.Add("Access-Control-Expose-Headers",$"{tmp},{newHeader}");
            }
        }
    }
}