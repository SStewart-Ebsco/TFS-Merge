using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using ER_BestPickReports_Dev.App_Code;

namespace ER_BestPickReports_Dev.blogfiles
{
    public partial class search : BasePage
    {
        private List<BlogPostDL> listTable;
        int recordNumber = 0;
        string strkeyword = "";
        string strcity = "";
        string cityid = "";
        int pageSize = 10;
        int page = 1;
        int totalPages = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            strkeyword = (Request["tag"] == null || Request["tag"] == "") ? "" : Request["tag"].ToString();
            strcity = (Request["city"] == null || Request["city"] == "") ? "" : Request["city"].ToString();

            //Set masterpage variables
            ((ER_BestPickReports_Dev.blogfiles.Blog)Master).strcity = strcity;

            //Set paging
            if (PageSaved.Value == "")
                PageSaved.Value = page.ToString();
            else
                page = int.Parse(PageSaved.Value);

            //Get city id from urlname
            if (strcity != "")
            {
                string sql = "SELECT CityID FROM CityInfo WHERE URLName=@NAME";
                cityid = DataAccessHelper.Data.ExecuteScalar(sql,
                    new SqlParameter("@NAME", strcity)).ToString();
            }

            if (!IsPostBack)
            {
                Keyword.Text = strkeyword;

                //Show result posts
                ShowResults();
            }
        }

        //private void ShowResults()
        //{
        //    string sql = "";
        //    string sql_add = "";
        //    string sql_join = "";

        //    if (cityid != "")
        //    {
        //        sql_join += " INNER JOIN BlogPostsToCities ON BlogPosts.PostID = BlogPostsToCities.PostID";
        //        sql_add += " AND (BlogPostsToCities.CityID = @CITYID)";
        //    }

        //    if (strkeyword != "")
        //    {
        //        sql_add += " AND CONTAINS(BlogPosts.*, @KEYWORD)";
        //    }
        //    else
        //    {
        //        //Need to error if not a keyword
        //        return;
        //    }

        //    //Filter by PublsihDate if not logged in
        //    if (!LoggedIn)
        //        sql_add += " AND (BlogPosts.PublishDate <= @DATE)";

        //    //Get total count of posts
        //    if (TotalPagesSaved.Value == "")
        //    {
        //        sql = "SELECT COUNT(*) FROM BlogPosts" + sql_join + " WHERE (BlogPosts.Title IS NOT NULL) " + sql_add;
        //        int cnt = int.Parse(Data.ExecuteScalar(sql,
        //            new SqlParameter("@DATE", DateTime.Now),
        //            new SqlParameter("@CITYID", cityid),
        //            new SqlParameter("@KEYWORD", "\"" + strkeyword + "\"")).ToString());
        //        totalPages = (int)Math.Ceiling((float)cnt / (float)pageSize);
        //        TotalPagesSaved.Value = totalPages.ToString();
        //    }
        //    else
        //    {
        //        totalPages = int.Parse(TotalPagesSaved.Value);
        //    }
        //    int startRow = (page - 1) * pageSize + 1;
        //    SetPaging();

        //    sql = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY PublishDate DESC) rownum, BlogPosts.PostID, BlogPosts.Title, BlogPosts.UrlTitle, BlogPosts.Summary, BlogPosts.PublishDate, BlogPosts.ImagePath, BlogPosts.ImageSummary FROM BlogPosts" + sql_join +
        //        " WHERE (BlogPosts.PublishDate <= @DATE)" + sql_add + ") query1 WHERE rownum BETWEEN " + startRow.ToString() + " AND " + ((startRow + pageSize) - 1).ToString();

        //    listTable = Data.ExecuteDataset(sql,
        //        new SqlParameter("@DATE", DateTime.Now),
        //        new SqlParameter("@CITYID", cityid),
        //        new SqlParameter("@KEYWORD", "\"" + strkeyword + "\"")).Tables[0];

        //    PostList.DataSource = listTable;
        //    PostList.DataBind();
        //}

        private void ShowResults()
        {
            listTable = BlogPostDL.SearchBlogPosts(cityid, strkeyword, !LoggedIn);
            listTable = listTable.GroupBy( b => b.PostId).Select(b => b.First()) .ToList();

            //Get total count of posts
            if (String.IsNullOrEmpty(TotalPagesSaved.Value))
            {
                int cnt = listTable.Count(p => !String.IsNullOrEmpty(p.Title));
                totalPages = (int)Math.Ceiling((float)cnt / (float)pageSize);
                TotalPagesSaved.Value = totalPages.ToString();
            }
            else
            {
                totalPages = int.Parse(TotalPagesSaved.Value);
            }

            int startRow = (page - 1) * pageSize;
            SetPaging();

            listTable = (listTable.Count - startRow > pageSize)
                         ? listTable.GetRange(startRow, pageSize)
                         : listTable.GetRange(startRow, listTable.Count - startRow);

            PostList.DataSource = listTable;
            PostList.DataBind();
        }

        protected void PostList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var postUrl = String.Concat("/blog/post/", listTable[recordNumber].UrlTitle);
                HyperLink title = (HyperLink)e.Item.FindControl("PostTitle");
                title.Text = listTable[recordNumber].Title;
                title.NavigateUrl = postUrl ;

                if (listTable[recordNumber].ImagePath != "" && bool.Parse(listTable[recordNumber].ImageSummary))
                {
                    Image img = (Image)e.Item.FindControl("PostImage");
                    img.ImageUrl = "/blogfiles/assets/media/" + listTable[recordNumber].ImagePath;
                    img.Visible = true;

                    var imgLink = (HyperLink)e.Item.FindControl("PostImageLink");
                    imgLink.NavigateUrl = postUrl;
                }


                if (!String.IsNullOrEmpty(listTable[recordNumber].AuthorNames))
                {
                    var postAuthorName = (Label)e.Item.FindControl("Names");
                    var postAuthorTitle = (Label)e.Item.FindControl("Position");
                    postAuthorName.Text = "By " + listTable[recordNumber].AuthorNames;
                    //postAuthorTitle.Text = listTable[recordNumber].AuthorTitles;
                }
                else if (Regex.IsMatch(listTable[recordNumber].Body, @">By ([A-Za-z\s]+)\|([A-Za-z\s&;]+)</p>"))
                {
                    var names = (Label)e.Item.FindControl("Names");
                    var position = (Label)e.Item.FindControl("Position");
                    Regex regex = new Regex(pattern: @">By ([A-Za-z\s]+)\|([A-Za-z\s&;]+)</p>");
                    Match match = regex.Match(listTable[recordNumber].Body);
                    names.Text = match.Groups[1].Value;
                    //position.Text = match.Groups[2].Value;
                }
                else
                {
                    var postAuthorPanel = (Panel)e.Item.FindControl("AuthorsInfo");
                    postAuthorPanel.Visible = false;
                }

                Label summary = (Label)e.Item.FindControl("Summary");
                summary.Text = listTable[recordNumber].Summary;

                HyperLink more = (HyperLink)e.Item.FindControl("ReadMoreLink");
                more.NavigateUrl = postUrl;

                Label PostMonth = (Label)e.Item.FindControl("PostMonth");
                PostMonth.Text = FormatDate(listTable[recordNumber].PublishDate);
                Label PostDate = (Label)e.Item.FindControl("PostDate");
                PostDate.Text = DateTime.Parse(listTable[recordNumber].PublishDate).Day.ToString();

                recordNumber++;
            }
        }

        private void SetPaging()
        {
            PrevNavigation.Visible = false;
            NextNavigation.Visible = false;
            if (page < totalPages)
                PrevNavigation.Visible = true;
            if (page > 1)
                NextNavigation.Visible = true;
        }

        protected void Previous_Click(object sender, EventArgs e)
        {
            page += 1;
            PageSaved.Value = page.ToString();
            ShowResults();
        }

        protected void More_Click(object sender, EventArgs e)
        {
            page -= 1;
            PageSaved.Value = page.ToString();
            ShowResults();
        }
    }
}