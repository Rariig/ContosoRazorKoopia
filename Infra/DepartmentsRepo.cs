using ContosoUniversityWithRazor.Data;
using ContosoUniversityWithRazor.Models;
using Domain;

namespace Infra
{
    public interface IDepartmentsRepo : IRepo<Department> { }

    public sealed class DepartmentsRepo : BaseRepo<Department>,
        IDepartmentsRepo
    {
        public DepartmentsRepo(ApplicationDbContext c) : base(c, c?.Departments) { }
    }
}