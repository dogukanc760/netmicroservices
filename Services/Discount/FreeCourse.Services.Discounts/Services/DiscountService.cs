

using Dapper;
using Dapper.Contrib.Extensions;

using FreeCourse.Services.Discounts.Models;
using FreeCourse.Shared.Dtos;

using Microsoft.Extensions.Configuration;

using Npgsql;

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discounts.Services
{
    public class DiscountService : IDiscountService
    {


        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<ResponseDto<NoContent>> Delete(int id)
        {
            var status = await _dbConnection.ExecuteAsync("Delete from discount where id=@Id", new { id });
            return status > 0 ? ResponseDto<NoContent>.Success(204) : ResponseDto<NoContent>.Fail("Some error has occured", 500);

        }

        public async Task<ResponseDto<List<Discount>>> GetAll()
        {
            var discount = await _dbConnection.QueryAsync<Models.Discount>("select * from discount");

            return ResponseDto<List<Models.Discount>>.Success(discount.ToList(), 200);
        }

        public async Task<ResponseDto<Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("select * from discount where userid=@UserId, code=@Code", new { UserId = userId, Code = code })).SingleOrDefault();
            if (discount == null)
            {
                return ResponseDto<Models.Discount>.Fail("Not Found.", 404);
            }

            return ResponseDto<Models.Discount>.Success(discount, 200);
        }

        public async Task<ResponseDto<Discount>> GetById(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Models.Discount>("select * from discount where id=@Id", new { id })).SingleOrDefault();
            if (discount == null)
            {
                return ResponseDto<Models.Discount>.Fail("Not Found.", 404);
            }

            return ResponseDto<Models.Discount>.Success(discount, 200);

        }

        public async Task<ResponseDto<NoContent>> Save(Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("INSERT INTO discount (userid, rate, code) values (@UserId, @Rate, @Code)", new { discount });

            return status > 0 ? ResponseDto<NoContent>.Success(204) : ResponseDto<NoContent>.Fail("Some error has occured", 500);
        }

        public async Task<ResponseDto<NoContent>> Update(Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("UPDATE discount set userid=@UserId, rate=@Rate, code=@Code where id=@Id", new { discount });
            return status > 0 ? ResponseDto<NoContent>.Success(204) : ResponseDto<NoContent>.Fail("Some error has occured", 500);

        }
    }
}
