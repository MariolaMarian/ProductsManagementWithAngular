using AutoMapper;
using ProductsMngmtAPI.Models;

namespace ProductsMngmtAPI.Helpers.Converters
{
    public class UserRoleToNameConverter : ITypeConverter<UserRole, string>
    {
        public string Convert(UserRole source, string destination, ResolutionContext context)
        {
            return source.Role.Name;
        }
    }
}