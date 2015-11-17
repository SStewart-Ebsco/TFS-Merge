using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web.UI.WebControls;
using ER_BestPickReports_Dev.App_Code;
using System.Linq;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev.blogfiles
{
    public partial class ArticleDetails : BasePage
    {
        string postid = "";
        string urlname;

        protected void Page_Load(object sender, EventArgs e)
        {
            postid = (Request["postid"] == null) ? "" : Request["postid"].ToString();

            //VALIDATE THIS PAGE LOGGED IN ONLY
            if (!LoggedIn)
                Response.Redirect("/blogfiles/login.aspx");
            
            if (!IsPostBack)
            {
                //Set editor parameters
                SetTelerikEditorOptions(BodyEditor);
                //SetTelerikEditorOptions(SummaryEditor);
                SetTelerikEditorOptions_Min(AtlantaLinks);
                SetTelerikEditorOptions_Min(ChicagoLinks);
                SetTelerikEditorOptions_Min(DallasLinks);
                SetTelerikEditorOptions_Min(NovaLinks);
                SetTelerikEditorOptions_Min(HoustonLinks);
                SetTelerikEditorOptions_Min(MarylandLinks);
                SetTelerikEditorOptions_Min(DCLinks);
                SetTelerikEditorOptions_Min(BhamLinks);
                SetTelerikEditorOptions_Min(BostonLinks);
                SetTelerikEditorOptions_Min(PhilLinks);

                //Field check box lists for city and category
                string sql = "SELECT * FROM CityInfo ORDER BY DisplayName";
                DataTable dt = DataAccessHelper.Data.ExecuteDataset(sql, null).Tables[0];
                CityID.DataSource = dt;
                CityID.DataTextField = "DisplayName";
                CityID.DataValueField = "CityID";
                CityID.DataBind();

                sql = "SELECT * FROM BlogCategories ORDER BY BlogCatName";
                dt = DataAccessHelper.Data.ExecuteDataset(sql, null).Tables[0];
                BlogCatID.DataSource = dt;
                BlogCatID.DataTextField = "BlogCatName";
                BlogCatID.DataValueField = "BlogCatID";
                BlogCatID.DataBind();

                int postId = -1;
                if (int.TryParse(postid, out postId))
                {
                    ContentAction.Text = "Edit Blog Post";
                    var post = BlogPostDL.GetById(postId);
                    if (post != null)
                    {
                        PostTitle.Text = post.Title;
                        PostUrlTitle.Text = post.UrlTitle;
                        PublishDate.SelectedDate = DateTime.Parse(post.PublishDate);
                        SummaryEditor.Text = post.Summary;
                        BodyEditor.Content = post.Body;
                        MetaDesc.Text = post.MetaDesc;
                        AtlantaLinks.Content = post.AtlantaLinks;
                        ChicagoLinks.Content = post.ChicagoLinks;
                        DallasLinks.Content = post.DallasLinks;
                        NovaLinks.Content = post.NovaLinks;
                        HoustonLinks.Content = post.HoustonLinks;
                        MarylandLinks.Content = post.MarylandLinks;
                        DCLinks.Content = post.DCLinks;
                        BhamLinks.Content = post.BhamLinks;
                        BostonLinks.Content = post.BostonLinks;
                        PhilLinks.Content = post.PhiliLinks;
                        ShowInSummary.Checked = bool.Parse(post.ImageSummary);
                        ShowInBody.Checked = bool.Parse(post.ImageBody);
                        AuthorName.Text = post.AuthorNames;
                        AuthorTitle.Text = post.AuthorTitles;
                        AuthorDescription.Text = post.AuthorDescriptions;

                        if (!String.IsNullOrEmpty(post.ImagePath))
                        {
                            ImageFile.ImageUrl = "/blogfiles/assets/media/" + post.ImagePath;
                            imagerow.Visible = true;
                            HiddenImagePath.Value = post.ImagePath;
                        }

                        var tags = BlogPostDL.GetTagsForBlogPost(postId);
                        Tags.Text = string.Join(", ", tags.ToArray());
                    }

                    var cityIds = BlogPostDL.GetCityIdsForBlogPost(postId);
                    foreach (ListItem lstItem in CityID.Items)
                    {
                        if (cityIds.Contains(lstItem.Value))
                            lstItem.Selected = true;
                    }

                    var catIds = CategoriesDL.GetCategoriesForBlogPost(postId);
                    foreach (ListItem lstItem in BlogCatID.Items)
                    {
                        if (catIds.Select(c => c.BlogCatId.ToString(CultureInfo.InvariantCulture)).Contains(lstItem.Value))
                            lstItem.Selected = true;
                    }
                }
                else
                {
                    ContentAction.Text = "Add New Blog Post";
                    PublishDate.SelectedDate = DateTime.Now;
                    PostTitle.Focus();
                }
            }
        }

        protected void UploadImage_Click(object sender, EventArgs e)
        {
            // Save the video
            string savePath = Server.MapPath("/blogfiles/assets/media");
            string fileName = ImagePath.FileName;

            if (ImagePath.HasFile)
            {
                string savefilename = Global.GetSaveFileName(savePath, fileName);
                string savefilepath = Server.MapPath("/blogfiles/assets/media/" + savefilename);
                ImagePath.SaveAs(savefilepath);

                //Resize image if necessary
                
                // SMS: Per the user story, Resizing is to be removed, but I'm leaving this
                // here in case this ever needs to be revisited (perhaps, resizing *should* occur for large images?)
                //ResizeImage(new FileInfo(savefilepath), 610);

                ImageFile.ImageUrl = "/blogfiles/assets/media/" + savefilename;
                imagerow.Visible = true;
                HiddenImagePath.Value = savefilename;
            }
        }
        /// <summary>
        /// Saves the full-size source image 
        /// </summary>
        static public void ResizeImage(FileInfo sourceImage, int width)
        {
            System.Drawing.Image OriginalImage = System.Drawing.Image.FromFile(sourceImage.FullName);
            Size newSize = new Size(width, OriginalImage.Height * width / OriginalImage.Width);
            System.Drawing.Image newImage = NewResizeImage(OriginalImage, newSize, preserveAspectRatio: true);
            OriginalImage.Dispose();
            OriginalImage = null;
            newImage.Save(sourceImage.FullName.ToLower());
        }
        public static System.Drawing.Image NewResizeImage(System.Drawing.Image image, Size size, bool preserveAspectRatio = true)
        {
            int newWidth;
            int newHeight;
            if (preserveAspectRatio)
            {
                int originalWidth = image.Width;
                int originalHeight = image.Height;
                float percentWidth = (float)size.Width / (float)originalWidth;
                float percentHeight = (float)size.Height / (float)originalHeight;
                float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
                newWidth = (int)(originalWidth * percent);
                newHeight = (int)(originalHeight * percent);
            }
            else
            {
                newWidth = size.Width;
                newHeight = size.Height;
            }
            System.Drawing.Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }
        /// <summary>
        /// Check for duplicate url name
        /// </summary>
        private bool CheckDuplicateName(string urltitle)
        {
            string sql = "";

            if (postid == "")
                sql = "SELECT COUNT(1) FROM BlogPosts WHERE (LOWER(urltitle) = @TEXT)";
            else
                sql = "SELECT COUNT(1) FROM BlogPosts WHERE (LOWER(urltitle) = @TEXT) AND (PostID <> @ID)";

            if (int.Parse(DataAccessHelper.Data.ExecuteScalar(sql,
                        new SqlParameter("@ID", postid),
                        new SqlParameter("@TEXT", urltitle.Trim().ToLower())).ToString()) > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            //Validate required fields
            if (PostTitle.Text == "")
            {
                errorrow.Visible = true;
                ErrorText.Text = "Please enter a post title.";
                return;
            }

            //Get urltitle value
            if (PostUrlTitle.Text == "" || PostUrlTitle.Text.Length < 4)
                urlname = CreateURLName(PostTitle.Text.Trim());
            else
                urlname = CreateURLName(PostUrlTitle.Text.Trim());

            //Return if the urltitle is already in use
            if (!CheckDuplicateName(urlname))
            {
                errorrow.Visible = true;
                ErrorText.Text = "The url title created/entered for this post is already in use.";
                return;
            }

            // Insert or update the record
            if (postid == "")
                AddPost();
            else
                UpdatePost();
        }

        /// <summary>
        /// Update post info
        /// </summary>
        private void UpdatePost()
        {
            var tags = Tags.Text.Trim().Split(',').ToList();
            var cityIds = CityID.Items.Cast<ListItem>().Where(item => item.Selected == true).Select(item => item.Value).ToList();
            var blogCatIds = BlogCatID.Items.Cast<ListItem>().Where(item => item.Selected == true).Select(item => item.Value).ToList();

            BlogPostDL.UpdateBlogPost(
                postId: int.Parse(postid),
                title: PostTitle.Text.Trim(),
                urlTitle: urlname,
                publishDate: PublishDate.SelectedDate,
                summary: SummaryEditor.Text.Trim(),
                body: BodyEditor.Content.Trim(),
                metaKeywords: Tags.Text.Trim(),
                metaDesc: MetaDesc.Text.Trim(),
                imagePath: HiddenImagePath.Value,
                imageSummary: ShowInSummary.Checked,
                imageBody: ShowInBody.Checked,
                atlantaLinks: AtlantaLinks.Content.Trim(),
                chicagoLinks: ChicagoLinks.Content.Trim(),
                dallasLinks: DallasLinks.Content.Trim(),
                novaLinks: NovaLinks.Content.Trim(),
                houstonLinks: HoustonLinks.Content.Trim(),
                marylandLinks: MarylandLinks.Content.Trim(),
                dCLinks: DCLinks.Content.Trim(),
                bhamLinks: BhamLinks.Content.Trim(),
                bostonLinks: BostonLinks.Content.Trim(),
                philLinks: PhilLinks.Content.Trim(),
                tags: tags,
                cityIds: cityIds,
                blogCatIds: blogCatIds,
                authorNames: AuthorName.Text.Trim(),
                authorTitles: AuthorTitle.Text.Trim(),
                authorDescriptions: AuthorDescription.Text.Trim());

            Response.Redirect("/articleListing");
        }

        /// <summary>
        /// Add post
        /// </summary>
        private void AddPost()
        {
            if (PublishDate.SelectedDate != null)
            {
                var tags = Tags.Text.Split(',').ToList();
                var cityIds = CityID.Items.Cast<ListItem>().Where(item => item.Selected == true).Select(item => item.Value).ToList();
                var blogCatIds = BlogCatID.Items.Cast<ListItem>().Where(item => item.Selected == true).Select(item => item.Value).ToList();

                BlogPostDL.AddBlogPost(
                title: PostTitle.Text.Trim(),
                urlTitle: urlname,
                publishDate: PublishDate.SelectedDate.Value,
                summary: SummaryEditor.Text.Trim(),
                body: BodyEditor.Content.Trim(),
                metaKeywords: Tags.Text.Trim(),
                metaDesc: MetaDesc.Text.Trim(),
                imagePath: HiddenImagePath.Value,
                imageSummary: ShowInSummary.Checked,
                imageBody: ShowInBody.Checked,
                atlantaLinks: AtlantaLinks.Content.Trim(),
                chicagoLinks: ChicagoLinks.Content.Trim(),
                dallasLinks: DallasLinks.Content.Trim(),
                novaLinks: NovaLinks.Content.Trim(),
                houstonLinks: HoustonLinks.Content.Trim(),
                marylandLinks: MarylandLinks.Content.Trim(),
                dCLinks: DCLinks.Content.Trim(),
                bhamLinks: BhamLinks.Content.Trim(),
                bostonLinks: BostonLinks.Content.Trim(),
                philLinks: PhilLinks.Content.Trim(),
                tags: tags,
                cityIds: cityIds,
                blogCatIds: blogCatIds,
                authorNames: AuthorName.Text.Trim(),
                authorTitles: AuthorTitle.Text.Trim(),
                authorDescriptions: AuthorDescription.Text.Trim());

                Response.Redirect("/articleListing");
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/articleListing");
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            if (LoggedIn)
            {
                //delete object
                string sql = "DELETE FROM BlogPosts WHERE (PostID = @ID);DELETE FROM BlogTags WHERE (PostID = @ID);DELETE FROM BlogPostsToCities WHERE (PostID = @ID);DELETE FROM BlogPostsToCategories WHERE (PostID = @ID)";
                DataAccessHelper.Data.ExecuteNonQuery(sql,
                    new SqlParameter("@ID", int.Parse(postid)));
            }
            Response.Redirect("/articleListing");
        }
    }
}