using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoRazorKoopia.Pages.Extensions
{
    class BasePageModel: PageModel
    {
        public string NameSort { get;protected set; }
        public string DateSort { get; protected set; }
        public string CurrentFilter { get; protected set; }
        public string CurrentSort { get; protected set; }
        public string HasPreviousPage { get; }
        public string HasNextPage { get; }
        public string PageIndex { get; }

    }
}
