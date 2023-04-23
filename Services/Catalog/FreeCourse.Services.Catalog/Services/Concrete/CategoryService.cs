using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Models.Settings;
using FreeCourse.Services.Catalog.Services.Abstract;
using FreeCourse.Shared.Dtos;
using MongoDB.Driver;

namespace FreeCourse.Services.Catalog.Services.Concrete
{
    public class CategoryService:ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;

        private readonly IMapper _mapper;

        public CategoryService(IMongoCollection<Category> categoryCollection, IMapper mapper,
            IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<ResponseDto<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(category => true).ToListAsync();

            return ResponseDto<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories), 200);
        }

        public async Task<ResponseDto<CategoryDto>> CreateAsync(Category category)
        {
            await _categoryCollection.InsertOneAsync(category);

            return ResponseDto<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 204);
        }

        public async Task<ResponseDto<CategoryDto>> GetByIdAsync(string id)
        {
            var category = await _categoryCollection.Find<Category>(x => x.Id == id).FirstOrDefaultAsync();

            if (category == null)
            {
                return ResponseDto<CategoryDto>.Fail("Category Not Found!", 404);
            }

            return ResponseDto<CategoryDto>.Success(_mapper.Map<CategoryDto>(category), 200);
        }
    }
}