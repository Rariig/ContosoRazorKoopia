using Contoso.Data;
using System;
using ContosoUniversityWithRazor.Models;

namespace Contoso.Domain
{
    public class DepartmentEntity : BaseEntity<DepartmentData>
    {
        public DepartmentEntity() : this(null) { }
        public DepartmentEntity(DepartmentData d) : base(d)
        {
            administrator = new Lazy<Instructor>(getInstructor);
        }
        private Instructor getInstructor()
            => (new GetRepo().Instance<IInstructorsRepo>() as IInstructorsRepo)?
                .Get(AdministratorId)
                .GetAwaiter()
                .GetResult();
        protected internal Lazy<Instructor> administrator { get; }
        public Instructor Administrator => administrator.Value;
        public string Name => Data?.Name ?? string.Empty;
        public decimal Budget => Data?.Budget ?? 0M;
        public DateTime StartDate { get; set; }
        public int? AdministratorId => Data?.AdministratorId ?? -1;
    }
}