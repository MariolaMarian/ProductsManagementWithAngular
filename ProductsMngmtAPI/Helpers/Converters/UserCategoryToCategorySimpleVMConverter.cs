using AutoMapper;
using ProductsMngmt.BLL.Models;
using ProductsMngmt.ViewModels.VMs.Category;

namespace ProductsMngmtAPI.Helpers.Converters
{
    public class UserCategoryToCategorySimpleVMConverter : ITypeConverter<UserCategory, CategorySimpleVM>
    {
        private IMapper _mapper;
        public UserCategoryToCategorySimpleVMConverter(IMapper mapper)
        {
            _mapper = mapper;
        }
        public CategorySimpleVM Convert(UserCategory source, CategorySimpleVM destination, ResolutionContext context)
        {
            return _mapper.Map<CategorySimpleVM>(source.Category);
        }
    }
}