using ContosoUniversityWithRazor.Data;
using ContosoUniversityWithRazor.Models;
using Domain;

namespace Infra
{
    public interface ICoursesRepo : IRepo<Course> { }

    public sealed class CoursesRepo : BaseRepo<Course>,
        ICoursesRepo
    {
        public CoursesRepo(ApplicationDbContext c) : base(c, c?.Courses) { }
    }
}