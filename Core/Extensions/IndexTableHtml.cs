using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Contoso.Core.Extensions
{
    public static class IndexTableHtml
    {
        internal static int defaultHeight = 50;
        public static IHtmlContent RowButtons<TModel>(
            this IHtmlHelper<TModel> h, string pageUrl, string itemId,
            params string[] handlers)
        {
            handlers = setDefaultCaptions(handlers);
            var s = TableData(
                   RowButton(itemId, pageUrl, "Index", "Index", handlers[0]),
                   RowButton(itemId, pageUrl, "Edit", "Edit", handlers[1]),
                   RowButton(itemId, pageUrl, "Details", "Details", handlers[2]),
                   RowButton(itemId, pageUrl, "Delete", "Delete", handlers[3])
                );
            return new HtmlContentBuilder(s);
        }
        public static IHtmlContent RowData<TModel, TResult>(
            this IHtmlHelper<TModel> h,
            Expression<Func<TModel, TResult>> data)
        {
            var s = TableData(h.DisplayFor(data));
            return new HtmlContentBuilder(s);
        }
        public static IHtmlContent TableHeader<TModel, TResult>(
            this IHtmlHelper<TModel> h,
            Expression<Func<TModel, TResult>> data)
        {
            var s = HeaderData(h.DisplayNameFor(data));
            return new HtmlContentBuilder(s);
        }
        public static IHtmlContent RowData<TModel>(
            this IHtmlHelper<TModel> h,
            string data)
        {
            var s = TableData(h.Raw(data));
            return new HtmlContentBuilder(s);
        }
        public static IHtmlContent RowImage<TModel>(
            this IHtmlHelper<TModel> h,
            string imageStr, int? height = null)
        {
            var s = TableData(
                new HtmlString(
                    $"<img src=\"{imageStr}\" alt=\"not uploaded\" " +
                    $"height={height ?? defaultHeight} />"
                    )
                );
            return new HtmlContentBuilder(s);
        }

        internal static string[] setDefaultCaptions(string[] handlers)
        {
            var l = new List<string>(handlers);
            if (l.Count == 0) l.Add(null);
            if (l.Count == 1) l.AddRange(
                new[] { "Edit", "Details", "Delete" });
            return l.ToArray();
        }
        public static IHtmlContent RowButton(string itemId,
            string pageUrl, string action, string handler, string caption)
        {
            return new HtmlString(
                (caption is null)
                    ? string.Empty
                    : $"<a id=\"{action}Btn\" " +
                      $"href=\"{pageUrl}/{action}?handler={handler}&id={itemId}\">" +
                      "<span style=\"font-weight:normal\">" +
                      $"{caption}</span></a> "
            );
        }
        public static List<object> TableData(params IHtmlContent[] data)
        {
            var l = new List<object> { new HtmlString("<td>") };
            l.AddRange(data);
            l.Add(new HtmlString("</td>"));
            return l;
        }
        public static List<object> HeaderData(params object[] data)
        {
            var l = new List<object> { new HtmlString("<th>") };
            l.AddRange(data);
            l.Add(new HtmlString("</th>"));
            return l;
        }
    }
}
