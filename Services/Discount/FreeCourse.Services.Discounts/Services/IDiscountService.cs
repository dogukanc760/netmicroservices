using FreeCourse.Services.Discounts.Models;
using FreeCourse.Shared.Dtos;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discounts.Services
{
    public interface IDiscountService
    {
        Task<ResponseDto<List<Models.Discount>>> GetAll();
        Task<ResponseDto<Models.Discount>> GetById(int id);
        Task<ResponseDto<NoContent>> Save(Models.Discount discount);
        Task<ResponseDto<NoContent>> Update(Models.Discount discount);
        Task<ResponseDto<NoContent>> Delete(int id);
        Task<ResponseDto<Models.Discount>> GetByCodeAndUserId(string code, string userId);


    }
}
