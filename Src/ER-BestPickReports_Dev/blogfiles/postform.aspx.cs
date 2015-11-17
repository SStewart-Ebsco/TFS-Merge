using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web.UI.WebControls;

namespace ER_BestPickReports_Dev.blogfiles
{
    public partial class postform : BasePage
    {
        string postid = "";
        string strcity = "";
        string strcat = "";
        string strmonth = "";
        string stryear = "";
        string urltitle = "";
        string urlname = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            postid = (Request["postid"] == null) ? "" : Request["postid"].ToString();
            strcity = (Request["city"].ToString() == "") ? "" : Request["city"].ToString();
            strcat = (Request["cat"].ToString() == "") ? "" : Request["cat"].ToString();
            strmonth = (Request["month"].ToString() == "") ? "" : Request["month"].ToString();
            stryear = (Request["year"].ToString() == "") ? "" : Request["year"].ToString();

            //VALIDATE THIS PAGE LOGGED IN ONLY
            if (!LoggedIn)
                Response.Redirect("/blog/login");

            //Get page url
            urltitle += "/blog/";

            if (strcat != "")
                urltitle += strcat + "/";

            if (strcity != "")
                urltitle += strcity + "/";

            if (strmonth != "" && stryear != "")
                urltitle += strmonth + "/" + stryear + "/";

            string sql = "";

            if (!IsPostBack)
            {
                //Set editor parameters
                SetTelerikEditorOptions(BodyEditor);
                SetTelerikEditorOptions(SummaryEditor);
                SetTelerikEditorOptions_Min(AtlantaLinks);
                SetTelerikEditorOptions_Min(ChicagoLinks);
                SetTelerikEditorOptions_Min(DallasLinks);
                SetTelerikEditorOptions_Min(NovaLinks);
                SetTelerikEditorOptions_Min(HoustonLinks);
                SetTelerikEditorOptions_Min(MarylandLinks);
                SetTelerikEditorOptions_Min(DCLinks);
                SetTelerikEditorOptions_Min(BhamLinks);

                //Field check box lists for city and category
                sql = "SELECT * FROM CityInfo ORDER BY DisplayName";
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

                if (postid != "")
                {
                    ContentAction.Text = "Edit Blog Post";

                    //Fill fields
                    sql = "SELECT * FROM BlogPosts WHERE (PostID = @PID)";

                    using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
                    {
                        SqlDataReader rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                            new SqlParameter("@DATE", DateTime.Now),
                            new SqlParameter("@PID", postid));

                        if (rdr.Read())
                        {
                            PostTitle.Text = rdr["Title"].ToString();
                            PostUrlTitle.Text = rdr["UrlTitle"].ToString();
                            PublishDate.SelectedDate = DateTime.Parse(rdr["PublishDate"].ToString());
                            SummaryEditor.Content = rdr["Summary"].ToString();
                            BodyEditor.Content = rdr["Body"].ToString();
                            MetaDesc.Text = rdr["MetaDesc"].ToString();
                            AtlantaLinks.Content = rdr["AtlantaLinks"].ToString();
                            ChicagoLinks.Content = rdr["ChicagoLinks"].ToString();
                            DallasLinks.Content = rdr["DallasLinks"].ToString();
                            NovaLinks.Content = rdr["NovaLinks"].ToString();
                            HoustonLinks.Content = rdr["HoustonLinks"].ToString();
                            MarylandLinks.Content = rdr["MarylandLinks"].ToString();
                            DCLinks.Content = rdr["DCLinks"].ToString();
                            BhamLinks.Content = rdr["BhamLinks"].ToString();
                            ShowInSummary.Checked = bool.Parse(rdr["ImageSummary"].ToString());
                            ShowInBody.Checked = bool.Parse(rdr["ImageBody"].ToString());

                            if (rdr["imagepath"].ToString() != "" && rdr["imagepath"].ToString() != null)
                            {
                                ImageFile.ImageUrl = "/blogfiles/assets/media/" + rdr["imagepath"].ToString();
                                imagerow.Visible = true;
                                HiddenImagePath.Value = rdr["imagepath"].ToString();
                            }

                            //Get tag list
                            string taglist = "";
                            char[] charsToTrim = {',',' '};
                            sql = "SELECT * FROM BlogTags WHERE (PostID = @PID)";
                            DataTable tagtable = DataAccessHelper.Data.ExecuteDataset(sql,
                                new SqlParameter("@PID", postid)).Tables[0];
                            foreach (DataRow row in tagtable.Rows)
                            {
                                taglist += row["TagName"].ToString() + ", ";
                            }
                            taglist = taglist.TrimEnd(charsToTrim);
                            Tags.Text = taglist;
                        }
                        rdr.Close();

                        sql = "SELECT CityID FROM BlogPostsToCities WHERE (CityID = @CITYID) AND (PostID = @PID)";
                        foreach (ListItem lstItem in CityID.Items)
                        {
                            //Check for city
                            object o = DataAccessHelper.Data.ExecuteScalar(sql,
                                new SqlParameter("@CITYID", lstItem.Value),
                                new SqlParameter("@PID", postid));

                            if (o != null)
                                lstItem.Selected = true;
                        }

                        sql = "SELECT BlogCatID FROM BlogPostsToCategories WHERE (BlogCatID = @CATID) AND (PostID = @PID)";
                        foreach (ListItem lstItem in BlogCatID.Items)
                        {
                            //check for category
                            object o = DataAccessHelper.Data.ExecuteScalar(sql,
                                new SqlParameter("@CATID", lstItem.Value),
                                new SqlParameter("@PID", postid));

                            if (o != null)
                                lstItem.Selected = true;
                        }

                        conn.Close();
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
                ResizeImage(new FileInfo(savefilepath), 610);
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
            using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
            {
                conn.Open();

                // Update post
                string sql = "UPDATE BlogPosts SET title=@TITLE, urltitle=@URLTITLE, publishdate=@PUBLISH, summary=@SUMMARY, body=@BODY, metakeywords=@KEY, metadesc=@DESC, imagesummary=@IMGSUM, imagebody=@IMGBODY, imagepath=@IMG, atlantalinks=@ATL, chicagolinks=@CHI, dallaslinks=@DAL, novalinks=@NOVA, houstonlinks=@HOU, marylandlinks=@MARY, dclinks=@DC, bhamlinks=@BHAM WHERE (PostID = @PID)";
                DataAccessHelper.Data.ExecuteNonQuery(conn, sql,
                    new SqlParameter("@TITLE", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "Title", DataRowVersion.Default, PostTitle.Text.Trim()),
                    new SqlParameter("@URLTITLE", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "UrlTitle", DataRowVersion.Default, urlname),
                    new SqlParameter("@PUBLISH", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, false, 0, 0, "PublishDate", DataRowVersion.Default, PublishDate.SelectedDate),
                    new SqlParameter("@SUMMARY", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "Summary", DataRowVersion.Default, SummaryEditor.Content.Trim()),
                    new SqlParameter("@BODY", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "Body", DataRowVersion.Default, BodyEditor.Content.Trim()),
                    new SqlParameter("@KEY", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "MetaKeywords", DataRowVersion.Default, Tags.Text.Trim()),
                    new SqlParameter("@DESC", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "MetaDesc", DataRowVersion.Default, MetaDesc.Text.Trim()),
                    new SqlParameter("@IMG", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "ImagePath", DataRowVersion.Default, HiddenImagePath.Value),
                    new SqlParameter("@IMGSUM", SqlDbType.Bit, 0, ParameterDirection.Input, false, 0, 0, "ImageSummary", DataRowVersion.Default, ShowInSummary.Checked),
                    new SqlParameter("@IMGBODY", SqlDbType.Bit, 0, ParameterDirection.Input, false, 0, 0, "ImageBody", DataRowVersion.Default, ShowInBody.Checked),
                    new SqlParameter("@ATL", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "AtlantaLinks", DataRowVersion.Default, AtlantaLinks.Content.Trim()),
                    new SqlParameter("@CHI", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "ChicagoLinks", DataRowVersion.Default, ChicagoLinks.Content.Trim()),
                    new SqlParameter("@DAL", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "DallasLinks", DataRowVersion.Default, DallasLinks.Content.Trim()),
                    new SqlParameter("@NOVA", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "NovaLinks", DataRowVersion.Default, NovaLinks.Content.Trim()),
                    new SqlParameter("@HOU", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "HoustonLinks", DataRowVersion.Default, HoustonLinks.Content.Trim()),
                    new SqlParameter("@MARY", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "MarylandLinks", DataRowVersion.Default, MarylandLinks.Content.Trim()),
                    new SqlParameter("@DC", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "DCLinks", DataRowVersion.Default, DCLinks.Content.Trim()),
                    new SqlParameter("@BHAM", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "BhamLinks", DataRowVersion.Default, BhamLinks.Content.Trim()),
                    new SqlParameter("@PID", SqlDbType.Int, 0, ParameterDirection.Input, false, 0, 0, "PostID", DataRowVersion.Default, int.Parse(postid)));

                //Remove values from blogpoststocategories and blogpoststocities
                sql = "DELETE FROM BlogPostsToCategories WHERE (PostID=@PID);DELETE FROM BlogPostsToCities WHERE (PostID=@PID)";
                DataAccessHelper.Data.ExecuteNonQuery(conn, sql,
                    new SqlParameter("@PID", SqlDbType.Int, 0, ParameterDirection.Input, false, 0, 0, "PostID", DataRowVersion.Default, int.Parse(postid)));

                //Remove values from blogtags
                sql = "DELETE FROM BlogTags WHERE (PostID=@PID)";
                DataAccessHelper.Data.ExecuteNonQuery(conn, sql,
                    new SqlParameter("@PID", SqlDbType.Int, 0, ParameterDirection.Input, false, 0, 0, "PostID", DataRowVersion.Default, int.Parse(postid)));

                //Insert tags
                string[] strtags = Tags.Text.Split(',');
                foreach (string tagitem in strtags)
                {
                    sql = "INSERT INTO BlogTags (postid, tagname) VALUES (@ID, @TAG)";
                    DataAccessHelper.Data.ExecuteNonQuery(conn, sql,
                        new SqlParameter("@ID", postid),
                        new SqlParameter("@TAG", tagitem.Trim()));
                }

                //Insert into blogpoststocategories and blogpoststocities
                foreach (ListItem lstItem in CityID.Items)
                {
                    if (lstItem.Selected == true)
                    {
                        sql = "INSERT INTO BlogPostsToCities (postid, cityid) VALUES (@ID, @CITYID)";
                        DataAccessHelper.Data.ExecuteNonQuery(conn, sql,
                            new SqlParameter("@ID", postid),
                            new SqlParameter("@CITYID", lstItem.Value));
                    }
                }

                foreach (ListItem lstItem in BlogCatID.Items)
                {
                    if (lstItem.Selected == true)
                    {
                        sql = "INSERT INTO BlogPostsToCategories (postid, blogcatid) VALUES (@ID, @CATID)";
                        DataAccessHelper.Data.ExecuteNonQuery(conn, sql,
                            new SqlParameter("@ID", postid),
                            new SqlParameter("@CATID", lstItem.Value));
                    }
                }

                conn.Close();
            }

            Response.Redirect("/blog/post/" + urlname);
        }

        /// <summary>
        /// Add post
        /// </summary>
        private void AddPost()
        {
            using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
            {
                conn.Open();

                // Output parameter.  Get the post id
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@NEWID";
                param.SqlDbType = SqlDbType.Int;
                param.Direction = ParameterDirection.Output;

                // Add post
                string sql = "INSERT INTO BlogPosts (title, urltitle, publishdate, summary, body, metakeywords, metadesc, imagesummary, imagebody, imagepath, atlantalinks, chicagolinks, dallaslinks, novalinks, houstonlinks, marylandlinks, dclinks, bhamlinks) VALUES (@TITLE, @URLTITLE, @PUBLISH, @SUMMARY, @BODY, @KEY, @DESC, @IMGSUM, @IMGBODY, @IMG, @ATL, @CHI, @DAL, @NOVA, @HOU, @MARY, @DC, @BHAM) SET @NEWID = SCOPE_IDENTITY()";
                DataAccessHelper.Data.ExecuteNonQuery(conn, sql,
                    new SqlParameter("@TITLE", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "Title", DataRowVersion.Default, PostTitle.Text.Trim()),
                    new SqlParameter("@URLTITLE", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "UrlTitle", DataRowVersion.Default, urlname),
                    new SqlParameter("@PUBLISH", SqlDbType.SmallDateTime, 0, ParameterDirection.Input, false, 0, 0, "PublishDate", DataRowVersion.Default, PublishDate.SelectedDate),
                    new SqlParameter("@SUMMARY", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "Summary", DataRowVersion.Default, SummaryEditor.Content.Trim()),
                    new SqlParameter("@BODY", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "Body", DataRowVersion.Default, BodyEditor.Content.Trim()),
                    new SqlParameter("@KEY", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "MetaKeywords", DataRowVersion.Default, Tags.Text.Trim()),
                    new SqlParameter("@DESC", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "MetaDesc", DataRowVersion.Default, MetaDesc.Text.Trim()),
                    new SqlParameter("@IMG", SqlDbType.VarChar, 500, ParameterDirection.Input, false, 0, 0, "ImagePath", DataRowVersion.Default, HiddenImagePath.Value),
                    new SqlParameter("@IMGSUM", SqlDbType.Bit, 0, ParameterDirection.Input, false, 0, 0, "ImageSummary", DataRowVersion.Default, ShowInSummary.Checked),
                    new SqlParameter("@IMGBODY", SqlDbType.Bit, 0, ParameterDirection.Input, false, 0, 0, "ImageBody", DataRowVersion.Default, ShowInBody.Checked),
                    new SqlParameter("@ATL", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "AtlantaLinks", DataRowVersion.Default, AtlantaLinks.Content.Trim()),
                    new SqlParameter("@CHI", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "ChicagoLinks", DataRowVersion.Default, ChicagoLinks.Content.Trim()),
                    new SqlParameter("@DAL", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "DallasLinks", DataRowVersion.Default, DallasLinks.Content.Trim()),
                    new SqlParameter("@NOVA", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "NovaLinks", DataRowVersion.Default, NovaLinks.Content.Trim()),
                    new SqlParameter("@HOU", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "HoustonLinks", DataRowVersion.Default, HoustonLinks.Content.Trim()),
                    new SqlParameter("@MARY", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "MarylandLinks", DataRowVersion.Default, MarylandLinks.Content.Trim()),
                    new SqlParameter("@DC", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "DCLinks", DataRowVersion.Default, DCLinks.Content.Trim()),
                    new SqlParameter("@BHAM", SqlDbType.Text, 0, ParameterDirection.Input, false, 0, 0, "BhamLinks", DataRowVersion.Default, BhamLinks.Content.Trim()),
                    param);

                int newpostid = int.Parse(param.Value.ToString());

                //Insert tags
                string[] strtags = Tags.Text.Split(',');
                foreach (string tagitem in strtags)
                {
                    sql = "INSERT INTO BlogTags (postid, tagname) VALUES (@ID, @TAG)";
                    DataAccessHelper.Data.ExecuteNonQuery(conn, sql,
                        new SqlParameter("@ID", newpostid),
                        new SqlParameter("@TAG", tagitem.Trim()));
                }

                //Insert into blogpoststocategories and blogpoststocities
                foreach (ListItem lstItem in CityID.Items)
                {
                    if (lstItem.Selected == true)
                    {
                        sql = "INSERT INTO BlogPostsToCities (postid, cityid) VALUES (@ID, @CITYID)";
                        DataAccessHelper.Data.ExecuteNonQuery(conn, sql,
                            new SqlParameter("@ID", newpostid),
                            new SqlParameter("@CITYID", lstItem.Value));
                    }
                }

                foreach (ListItem lstItem in BlogCatID.Items)
                {
                    if (lstItem.Selected == true)
                    {
                        sql = "INSERT INTO BlogPostsToCategories (postid, blogcatid) VALUES (@ID, @CATID)";
                        DataAccessHelper.Data.ExecuteNonQuery(conn, sql,
                            new SqlParameter("@ID", newpostid),
                            new SqlParameter("@CATID", lstItem.Value));
                    }
                }

                conn.Close();
            }

            Response.Redirect("/blog");
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(urltitle);
        }
    }
}