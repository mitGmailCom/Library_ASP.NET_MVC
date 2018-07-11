using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_ASP.NET_MVC.Models
{
    public static class HelperUsers
    {
        public static MvcHtmlString ShowListCollectionLibrary(this HtmlHelper helper, IEnumerable<object> list)
        {
            string temp = "--.--.----";
            //TagBuilder tBuilder = new TagBuilder("ul");
            //if (list is IEnumerable<Author>)
            //{
            //    IEnumerable<Author> Authors = list as IEnumerable<Author>;
            //    foreach (var author in Authors)
            //    {
            //        TagBuilder li = new TagBuilder("li");
            //        if (author.DateOfDeath == null)
            //            li.InnerHtml += $"{author.Name}({author.DateOfBirth.ToShortDateString()} - {temp})";
            //        if (author.DateOfDeath != null)
            //            li.InnerHtml += $"{author.Name}({author.DateOfBirth.ToShortDateString()} - {((DateTime)author.DateOfDeath).ToShortDateString()})";
            //        TagBuilder br = new TagBuilder("br");
            //        li.InnerHtml += br.ToString(TagRenderMode.SelfClosing);
            //        tBuilder.InnerHtml += li.ToString();
            //    }
            //}
            TagBuilder tBuilder = new TagBuilder("select");
            tBuilder.MergeAttribute("id", "drdowmlist");
            tBuilder.MergeAttribute("name", "drdowmlist1");
            tBuilder.AddCssClass("authors");
            if (list is IEnumerable<Author>)
            {
                IEnumerable<Author> Authors = list as IEnumerable<Author>;
                foreach (var author in Authors)
                {
                    TagBuilder option = new TagBuilder("option");
                    if (author.DateOfDeath == null)
                        option.InnerHtml += $"{author.Name}({author.DateOfBirth.ToShortDateString()} - {temp})";
                    if (author.DateOfDeath != null)
                        option.InnerHtml += $"{author.Name}({author.DateOfBirth.ToShortDateString()} - {((DateTime)author.DateOfDeath).ToShortDateString()})";
                    tBuilder.InnerHtml += option.ToString();

                }
            }

            return new MvcHtmlString(tBuilder.ToString());
        }
    }
}