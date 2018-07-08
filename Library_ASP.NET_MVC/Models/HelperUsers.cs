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
            string temp = "----";
            TagBuilder tBuilder = new TagBuilder("ul");
            if (list is IEnumerable<Author>)
            {
                IEnumerable<Author> Authors = list as IEnumerable<Author>;
                foreach (var author in Authors)
                {
                    

                    TagBuilder li = new TagBuilder("li");
                    if (author.DateOfDeath == null)
                        li.InnerHtml += $"{author.Name}({author.DateOfBirth} - {temp})";
                    if (author.DateOfDeath != null)
                        li.InnerHtml += $"{author.Name}({author.DateOfBirth} - {author.DateOfDeath})";
                    TagBuilder br = new TagBuilder("br");
                    li.InnerHtml += br.ToString(TagRenderMode.SelfClosing);
                    tBuilder.InnerHtml += li.ToString();
                }
            }


            return new MvcHtmlString(tBuilder.ToString());
        }
    }
}