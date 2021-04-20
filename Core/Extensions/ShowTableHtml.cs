using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.JsonPatch.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
/*
namespace ContosoRazorKoopia.Pages.Extensions
{
    public static class ShowTableHtml
    {
        public static IHtmlContent ShowTable<TModel, TResult>(this IHtmlHelper<TModel> h,
            Expression<Func<TModel, IEnumerable<TResult>>> e,
            params string[]propertyNames) => ShowTable(h, e, e,propertyNames);

        public static IHtmlContent ShowTable<TModel, TResult1, TResult2>(this IHtmlHelper<TModel> h,
            Expression<Func<TModel, TResult1>> label,
            Expression<Func<TModel, TResult2>> value,
            params string[] propertyNames)
        {
            var labelStr = h.DisplayNameFor(label);
            IEnumerable<dynamic> v = (value is null) ? getValue(h, label) : getValue(h, value);
            return h.ShowTable(labelStr, v, propertyNames);
        }

        public static IHtmlContent ShowTable<TModel, TClass>(this IHtmlHelper<TModel> h,
            string label,
            IEnumerable<TClass> list,
            params string[] propertyNames)
        {
            if (h == null) throw new ArgumentNullException(nameof(h));
            var s = htmlStrings(h, label, list, propertyNames);
            return new HtmlContentBuilder(s);
        }

        public static IEnumerable<dynamic> getValue <TModel, TResult>(IHtmlHelper<TModel> h,
            Expression<Func<TModel, IEnumerable<TResult>>> value)
        {
            var r = value.Compile();
            var v = r.Invoke(h.ViewData.Model);
            var l = v as IEnumerable<dynamic>;
            return l;
        }

        internal static List<object> htmlStrings<TModel, TClass>(IHtmlHelper<TModel> h,
            string label,
            IEnumerable<TClass> list,
            string[] propertyNames)
        {
            list ??= new List<TClass>();
            var l = new List<object>
            {
                new HtmlString("<dt class =\"col-sm-2\">"),
                h.Raw(label),
                new HtmlString("</dt>"),
                new HtmlString("<dt class =\"col-sm-10\">"),
                new HtmlString("<table class =\"table\">"),
                new HtmlString("<tr>")
            };
            foreach (var item in list)
            {
                foreach (var n in propertyNames)
                {
                    1.Add(new HtmlString($"<th>{getDisplayName(item, n)}</th>"));
                }

                break;
            }
        }
    }
}
*/
