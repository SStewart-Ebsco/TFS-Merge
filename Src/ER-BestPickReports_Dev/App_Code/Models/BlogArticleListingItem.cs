using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ER_BestPickReports_Dev.App_Code.Models
{
    public class BlogArticleListingItem
    {
        public int PostID { get; private set; }
        public string Title { get; private set; }
        public string Categories { get; private set; }
        public bool IsTopBlog { get; private set; }
        public string PublishDate { get; private set; }

        public BlogArticleListingItem(int postID, string title, string categories, bool isTopBlog, string publishDate)
        {
            PostID = postID;
            Title = title;
            Categories = categories;
            IsTopBlog = isTopBlog;
            PublishDate = publishDate;
        }

    }
}