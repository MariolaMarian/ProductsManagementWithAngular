using System.Collections.Generic;
using ProductsMngmtAPI.VMs.Category;

namespace ProductsMngmtAPI.VMs.User
{
    public class UserWithIncludingsVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<CategorySimpleVM> Categories { get; set; }
    }
}