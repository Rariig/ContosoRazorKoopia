using Contoso.Aids;
using Contoso.Data;
using Contoso.Domain;
using Contoso.Facade;
using Contoso.Infra;
using Contoso.Pages.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoRazorKoopia.Pages;
using ContosoUniversityWithRazor.Data;
using Infra;

namespace Contoso.Pages
{
    public class DepartmentsModel : BasePageModel<DepartmentEntity, DepartmentView>
    {
        public override string PageTitle => "Departments";
        public DepartmentsModel(ApplicationDbContext c) : this(new DepartmentsRepo(c), c) { }
        protected internal DepartmentsModel(IDepartmentsRepo r, ApplicationDbContext c = null) : base(r, c) { }
        protected internal override DepartmentView toViewModel(DepartmentEntity d)
        {
            if (isNull(d)) return null;
            var v = Copy.Members(d.Data, new DepartmentView());
            v.AdministratorName = d.Administrator?.FullName;
            return v;
        }
        protected internal override DepartmentEntity toEntity(DepartmentView v)
        {
            var d = Copy.Members(v, new DepartmentData());
            return new DepartmentEntity(d);
        }










        public SelectList Instructors =>
            new(context.Instructors.OrderBy(x => x.LastName).AsNoTracking(),
                "Id", "FullName", Item?.AdministratorId);
        public IList<DepartmentView> Department { get; set; }
        public async Task OnGetIndexAsync()
        {
            Department = (await repo.Get()).Select(toViewModel).ToList();
        }
    }
}