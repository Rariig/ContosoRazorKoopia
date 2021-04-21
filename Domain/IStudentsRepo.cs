using Contoso.Data;
using ContosoUniversityWithRazor.Models;
using Domain;

namespace Contoso.Domain
{
    public interface IStudentsRepo : IRepo<Student> { }
}