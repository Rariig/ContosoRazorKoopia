using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContosoUniversityWithRazor.Models;
using Domain;

namespace Infra
{
    public interface IInstructorsRepo:IRepo<Instructor>
    {
    }
}
