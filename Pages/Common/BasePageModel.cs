using Contoso.Domain;
using Contoso.Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using Contoso.Core;
using ContosoUniversityWithRazor.Data;
using Domain;

namespace Contoso.Pages.Common
{
    public abstract class BasePageModel : PageModel
    {
        public string ErrorMessage { get; protected set; }
        public abstract string PageTitle { get; }
        [BindProperty] public IEntityData Item { get; set; }
        public string NameSort { get; protected set; }
        public string DateSort { get; protected set; }
        public string CurrentFilter { get; protected set; }
        public string CurrentSort { get; protected set; }
        public virtual bool HasPreviousPage { get; protected set; }
        public virtual bool HasNextPage { get; protected set; }
        public virtual int PageIndex { get; protected set; }
    }
    public abstract class BasePageModel<TEntity, TView> : BasePageModel
        where TEntity : class, IBaseEntity, new()
        where TView : class, IEntityData, new()
    {
        protected readonly ApplicationDbContext context;
        protected readonly IRepo<TEntity> repo;
        protected BasePageModel(IRepo<TEntity> r, ApplicationDbContext c = null)
        {
            context = c;
            repo = r;
        }
        [BindProperty]
        public new TView Item
        {
            get => (TView)base.Item;
            set => base.Item = value;
        }
        protected internal bool isNull(object o) => o is null;
        protected internal string setConcurrencyMsg(bool isError) => isError ? ErrorMessages.ConcurrencyOnDelete : null;
        protected internal virtual void doOnCreate() { }
        protected internal abstract TView toViewModel(TEntity e);
        protected internal abstract TEntity toEntity(TView e);
        protected internal virtual async Task getRelatedItems(TEntity item) { await Task.CompletedTask; }
        internal async Task<TView> getItem(int? id, bool concurrencyError = false)
        {
            var item = await repo.Get(id);
            await getRelatedItems(item);
            ErrorMessage = setConcurrencyMsg(concurrencyError);
            return toViewModel(item);
        }
        internal async Task<bool> remove() =>
            !isNull(repo) && await repo.Delete(toEntity(Item));
        internal async Task<bool> add() =>
            !isNull(repo) && await repo.Add(toEntity(Item));
        internal async Task<bool> update() =>
            !isNull(repo) && await repo.Update(toEntity(Item));

        internal async Task<TEntity> find(int? id)
            => isNull(id) ? null : isNull(repo) ? null : await repo.Get(id);

        internal async Task<bool> save(params Func<Task<bool>>[] actions)
        {
            var transaction = isNull(context) ? null : await context.Database.BeginTransactionAsync();
            foreach (var a in actions)
            {
                var b = await a();
                if (b) continue;
                ErrorMessage = repo?.ErrorMessage;
                if (transaction != null) await transaction.RollbackAsync();
                return false;
            }
            if (!isNull(context)) await context.SaveChangesAsync();
            if (transaction != null) await transaction.CommitAsync();
            return true;
        }
        protected internal virtual void doBeforeCreate(TView v, string[] selectedCourses = null) { }
        protected internal virtual void doBeforeDelete(TView v) { }
        protected internal virtual void doBeforeEdit(TView v, string[] selectedCourses = null) { }

        internal IActionResult indexPage() =>
            RedirectToPage("./Index", new { handler = "Index" });
        public async Task<IActionResult> OnGetDeleteAsync(int id, bool concurrencyError = false)
            => isNull(Item = await getItem(id, concurrencyError)) ? NotFound() : Page();
        public async Task<IActionResult> OnGetDetailsAsync(int id)
            => isNull(Item = await getItem(id)) ? NotFound() : Page();
        public async Task<IActionResult> OnGetEditAsync(int id)
            => isNull(Item ??= await getItem(id)) ? NotFound() : Page();
        public IActionResult OnGetCreate()
        {
            doOnCreate();
            return Page();
        }
        public async virtual Task<IActionResult> OnPostDeleteAsync(int id)
        {
            doBeforeDelete(Item);
            if (await save(remove)) return indexPage();
            if (repo?.EntityInDb is null) return indexPage();
            return RedirectToPage("./Delete",
                new { id, concurrencyError = true, handler = "Delete" });
        }
        public async virtual Task<IActionResult> OnPostEditAsync(int id, string[] selectedCourses = null)
        {
            if (!ModelState.IsValid) return Page();
            doBeforeEdit(Item, selectedCourses);
            if (await save(update)) return indexPage();
            setPreviousValues(toViewModel(repo?.EntityInDb));
            Item.RowVersion = repo?.EntityInDb?.RowVersion;
            ModelState.Remove("Item.RowVersion");
            return Page();
        }
        public async virtual Task<IActionResult> OnPostCreateAsync(string[] selectedCourses = null)
        {
            if (!ModelState.IsValid) return Page();
            doBeforeCreate(Item, selectedCourses);
            if (await save(add)) return indexPage();
            return Page();
        }


        private void setPreviousValues(TView dbValues)
        {
            if (isNull(dbValues)) return;
            foreach (var p in dbValues.GetType().GetProperties())
            {
                if (!p.CanRead) continue;
                var dbValue = p.GetValue(dbValues);
                var clientValue = p.GetValue(Item);
                if (dbValue?.ToString() == clientValue?.ToString()) continue;
                ModelState.AddModelError($"Item.{p.Name}",
                    $"Current value: {dbValue}");
            }
        }
    }
}
