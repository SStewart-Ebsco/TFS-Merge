using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using ER_BestPickReports_Dev.App_Code;
using System.Collections.Generic;

namespace ER_BestPickReports_Dev.blogfiles.mobile
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
            ((ER_BestPickReports_Dev.blogfiles.mobile.BlogMobile)Master).strcity = strcity;

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

        private void ShowResults()
        {
            listTable = BlogPostDL.SearchBlogPosts(cityid, strkeyword, !LoggedIn);
            listTable = listTable.GroupBy(b => b.PostId).Select(b => b.First()).ToList();

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
                var postUrl = "/blog/post/" + listTable[recordNumber].UrlTitle;
                if (listTable[recordNumber].ImagePath != "" && bool.Parse(listTable[recordNumber].ImageSummary))
                {
                    Image img = (Image)e.Item.FindControl("PostImage");
                    img.ImageUrl = "/blogfiles/assets/media/" + listTable[recordNumber].ImagePath;
                    img.Visible = true;
                    var imgLink = (HyperLink)e.Item.FindControl("PostImageLink");
                    imgLink.NavigateUrl = postUrl;
                }

                HyperLink title = (HyperLink)e.Item.FindControl("PostTitle");
                title.Text = listTable[recordNumber].Title;
                title.NavigateUrl = postUrl;

                HyperLink more = (HyperLink)e.Item.FindControl("ReadMoreLink");
                more.NavigateUrl = postUrl;

                Label PostMonth = (Label)e.Item.FindControl("PostDateMonth");
                PostMonth.Text = FormatDate(listTable[recordNumber].PublishDate);
                Label PostDate = (Label)e.Item.FindControl("PostDate");
                PostDate.Text = DateTime.Parse(listTable[recordNumber].PublishDate).Day.ToString();
                recordNumber++;
            }
        }

        private void SetPaging()
        {
            More.Visible = false;
            Previous.Visible = false;
            if (page < totalPages)
                More.Visible = true;
            if (page > 1)
                Previous.Visible = true;
        }

        protected void Previous_Click(object sender, EventArgs e)
        {
            page -= 1;
            PageSaved.Value = page.ToString();
            SetPaging();

            //Get posts
            ShowResults();
        }

        protected void More_Click(object sender, EventArgs e)
        {
            page += 1;
            PageSaved.Value = page.ToString();
            SetPaging();

            //Get posts
            ShowResults();
        }
    }
}