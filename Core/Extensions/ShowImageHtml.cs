using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using ContosoRazorKoopia.Pages.Extensions;

namespace Contoso.Core.Extensions
{
    public static class ShowImageHtml
    {
        internal static int defaultWidth => 100;
        public static IHtmlContent ShowImage<TModel, TResult>(
            this IHtmlHelper<TModel> h,
            Expression<Func<TModel, TResult>> e,
            int? width = null
        ) => ShowImage(h, e, e, width);

        public static IHtmlContent ShowImage<TModel, TResult1, TResult2>(
            this IHtmlHelper<TModel> h,
            Expression<Func<TModel, TResult1>> label,
            Expression<Func<TModel, TResult2>> value = null,
            int? width = null)
        {
            var labelStr = h.DisplayNameFor(label);
            var valueStr = (value is null)
                ? getValue(h, label)
                : getValue(h, value);
            return h.ShowImage(labelStr, valueStr, width);
        }

        public static IHtmlContent ShowImage<TModel>(this IHtmlHelper<TModel> h, string label
            , string value, int? width = null)
        {
            if (h == null) throw new ArgumentNullException(nameof(h));
            var s = htmlStrings(h, label, value, width);
            return new HtmlContentBuilder(s);
        }
        internal static List<object> htmlStrings<TModel>(
            IHtmlHelper<TModel> h,
            string label,
            string value, int? width = null)
        {
            width ??= defaultWidth;
            return ShowHtml.HtmlStrings(
                h.Raw(label),
                new HtmlString(
                    $"<img src=\"{value}\" " +
                    "alt=\"not uploaded\" " +
                    $"width={width} />"));
        }
        internal static string getValue<TModel, TResult>(IHtmlHelper<TModel> h, Expression<Func<TModel, TResult>> e)
        {
            var value = h.DisplayFor(e);
            var writer = new System.IO.StringWriter();
            value.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
