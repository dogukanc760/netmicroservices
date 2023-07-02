using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Shared.Dtos;

using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<ResponseDto<bool>> Delete(string userId)
        {
            var status = await _redisService.GetDB().KeyDeleteAsync(userId);
            return status ? ResponseDto<bool>.Success(204) : ResponseDto<bool>.Fail("Basket couldnt not delete", 500);

        }

        public async Task<ResponseDto<BasketDto>> GetBasket(string userId)
        {
            var existBasket = await _redisService.GetDB().StringGetAsync(userId);

            if (string.IsNullOrEmpty(existBasket))
            {
                return ResponseDto<BasketDto>.Fail("Cannot found basket!", 404);
            }
            return ResponseDto<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existBasket), 200);
        }

        public async Task<ResponseDto<bool>> SaveOrUpdate(BasketDto basketDto)
        {
            var status = await _redisService.GetDB().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));

            return status ? ResponseDto<bool>.Success(204) : ResponseDto<bool>.Fail("Basket couldnt not update or save", 500);
        }
    }
}
