using System.Linq;
using System.Security.Authentication;
using MongoDB.Driver;

namespace FrontEnd.Backend
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class BackEndFacade : IBackEndFacade
    {
        private readonly IMongoDatabase db;

        public BackEndFacade()
        {
            var connectionString = Environment.GetEnvironmentVariable("dbconnection");
            var settings = MongoClientSettings.FromUrl(
                new MongoUrl(connectionString)
            );
            settings.SslSettings =
                new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);
            this.db = mongoClient.GetDatabase("schooldb");
        }

        #region Courses

        private IMongoCollection<dynamic> GetCoursesHandle()
        {
            return this.db.GetCollection<dynamic>("courses");
        }

        public async Task CreateCourse(string name, Guid id)
        {
            await this.GetCoursesHandle().InsertOneAsync(
                FromCourse(
                    new Course(
                        id,
                        name)));
        }

        public async Task<IReadOnlyCollection<Course>> GetAllCourses()
        {
            var all = await this.GetCoursesHandle()
                .FindAsync(FilterDefinition<dynamic>.Empty);
            return all
                .ToEnumerable()
                .Select(ToCourse)
                .ToArray();
        }

        public async Task<Course> GetCourse(Guid id)
        {
            var all = await this.GetAllCourses();
            return all
                .SingleOrDefault(x => x.Id == id);
        }

        public static Course ToCourse(dynamic x) => new Course(x._id, x.name);
        public static dynamic FromCourse(Course x) => new { id = x.Id, name = x.Name };

        #endregion

        #region Students
        private IMongoCollection<dynamic> GetStudentsHandle()
        {
            return this.db.GetCollection<dynamic>("students");
        }

        public async Task CreateStudent(Student student)
        {
            await this.GetStudentsHandle().InsertOneAsync(
                FromStudent(
                    student));
        }

        public async Task<IReadOnlyCollection<Student>> GetStudents(Guid courseId)
        {
            var all = await this.GetStudentsHandle()
                .FindAsync(FilterDefinition<dynamic>.Empty);
            return all
                .ToEnumerable()
                .Select(ToStudent)
                .Where(x => x.CourseId == courseId)
                .ToArray();
        }

        public async Task<Student> GetNextStudentToLearn(Guid courseId)
        {
            var allStundentsOfCourse = await this.GetStudents(courseId);
            var random = new Random();
            if (!allStundentsOfCourse.Any())
            {
                throw new Exception("no students");
            }

            var index = random.Next(0, allStundentsOfCourse.Count - 1);

            return allStundentsOfCourse
                .ToArray()[index];
        }
        public static dynamic FromStudent(Student x)
            => new { courseid = x.CourseId, image = x.ImageInBase64, firstname = x.Firstname, lastname = x.Lastname };
        public static Student ToStudent(dynamic x)
            => new Student(x.courseid, x.firstname, x.lastname, x.image);

        #endregion
    }
}