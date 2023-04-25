using System.Collections.Generic;
using System.Threading.Tasks;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Shared.Dtos;

namespace FreeCourse.Services.Catalog.Services.Abstract
{
    public interface ICategoryService
    {
         Task<ResponseDto<List<CategoryDto>>> GetAllAsync();
         Task<ResponseDto<CategoryDto>> CreateAsync(CategoryDto categoryDto);
         Task<ResponseDto<CategoryDto>> GetByIdAsync(string id);
    }
}