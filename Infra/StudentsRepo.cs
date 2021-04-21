using ContosoUniversityWithRazor.Data;
using ContosoUniversityWithRazor.Models;
using Domain;

namespace Infra
{
    public interface IStudentsRepo : IRepo<Student> { }

    public sealed class StudentsRepo : BaseRepo<Student>, 
        IStudentsRepo
    {
        public StudentsRepo(ApplicationDbContext c) : base(c, c?.Students){}
    }
}
