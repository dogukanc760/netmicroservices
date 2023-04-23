using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CourseService:ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;


        private readonly IMapper _mapper;

        public CourseService(IMongoCollection<Course> courseCollection, IMapper mapper,
            IDatabaseSettings databaseSettings, IMongoCollection<Category> categoryCollection)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);

            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _mapper = mapper;
            _categoryCollection = categoryCollection;
        }

        public async Task<ResponseDto<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();
            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.Id).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }


            return ResponseDto<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<ResponseDto<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();

            if (course == null)
            {
                return ResponseDto<CourseDto>.Fail("Course Not Found!", 404);
            }

            return ResponseDto<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
        }

        public async Task<ResponseDto<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find<Course>(x => x.UserId == userId).ToListAsync();

            if (!courses.Any())
            {
                return ResponseDto<List<CourseDto>>.Fail("Course Not Found!", 404);
            }

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.Id).FirstAsync();
                }
            }

            return ResponseDto<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<ResponseDto<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDto);

            newCourse.CreatedTime = DateTime.Now;

            await _courseCollection.InsertOneAsync(newCourse);

            return ResponseDto<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 201);
        }

        

        public async Task<ResponseDto<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDto);
            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id, updateCourse);

            if (result == null)
            {
                return ResponseDto<NoContent>.Fail("Course not found!", 404);
            }

            return ResponseDto<NoContent>.Success(204);
        }

        public async Task<ResponseDto<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);

            if (result.DeletedCount > 0)
            {
                return ResponseDto<NoContent>.Success(204);
            }

            return ResponseDto<NoContent>.Fail("Course not found!", 404);
        }
    }
}