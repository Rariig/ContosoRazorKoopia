using ContosoUniversityWithRazor.Data;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ContosoRazorKoopia.Pages
{

    public abstract class BasePageModel : PageModel
    {
        public string PageName { get; set; }
        public string ErrorMessage { get; set; }
    }
    public abstract class BasePageModel<TEntity, TView> : BasePageModel
        /* where TEntity : class, IEntity, new()
        where TView : class, IEntity, new() */
    {
        protected readonly ApplicationDbContext dbContext;
         /* protected readonly IRepo<IEntity> repo; */

        public string NameSort { get; protected set; }
        public string DateSort { get; protected set; }
        public string CurrentFilter { get; protected set; }
        public string CurrentSort { get; protected set; }
        public string HasPreviousPage { get; }
        public string HasNextPage { get; }
        public string PageIndex { get; }


    }
}
