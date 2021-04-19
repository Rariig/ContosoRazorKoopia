using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContosoUniversityWithRazor.Data;
using ContosoUniversityWithRazor.Models;
using Domain;

namespace Infra
{
    public interface IInstructorsRepo:IRepo<Instructor> { }

    public sealed class InstructorsRepo : BaseRepo<Instructor>, IInstructorsRepo
    { 
        public InstructorsRepo(ApplicationDbContext c): base (c,c?.Instructors){}
    }
}
