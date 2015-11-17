using System;
using System.Data.SqlClient;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev
{
    public partial class homeownertestimonials : BasePage
    {
        string cityID = "0";
        string areaID = "0";

        private AppCookies bprPreferences = AppCookies.CreateInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = "";
            areaID = bprPreferences.AreaId.ToString();
            cityID = bprPreferences.CityId.ToString();

            if (!IsPostBack)
            {
                HiddenTestimonialCount.Value = "10";

                //Get testimonials and count of total
                if (cityID != "0")
                    sql = "SELECT COUNT(*) FROM HomeownerTestimonial WHERE (CityID = @CITYID) AND (PublishDate <= @DATE)";
                else
                    sql = "SELECT COUNT(*) FROM HomeownerTestimonial WHERE (PublishDate <= @DATE)";
                HiddenTotal.Value = DataAccessHelper.Data.ExecuteScalar(sql,
                    new SqlParameter("@CITYID", cityID),
                    new SqlParameter("@DATE", DateTime.Now)).ToString();

                LoadTestimonials(HiddenTestimonialCount.Value);
            }
        }

        protected void LoadTestimonials(string tc)
        {
            string sql = "";
            using (SqlConnection conn = new SqlConnection(DataAccessHelper.ConnString))
            {
                conn.Open();

                if (cityID != "0")
                    sql = "SELECT TOP " + tc + " * FROM HomeownerTestimonial WHERE (CityID = @CITYID) AND (PublishDate <= @DATE) ORDER BY NEWID()";
                else
                    sql = "SELECT TOP " + tc + " * FROM HomeownerTestimonial WHERE (PublishDate <= @DATE) ORDER BY NEWID()";

                SqlDataReader rdr = DataAccessHelper.Data.ExecuteDatareader(conn, sql,
                    new SqlParameter("@CITYID", cityID),
                    new SqlParameter("@DATE", DateTime.Now));

                TestimonialList.DataSource = rdr;
                TestimonialList.DataBind();
                rdr.Close();

                conn.Close();
            }

            //Hide load more button when needed
            if (int.Parse(HiddenTotal.Value) <= int.Parse(HiddenTestimonialCount.Value))
                MoreContainer.Visible = false;
        }

        protected void LoadReviews_Click(object sender, EventArgs e)
        {
            HiddenTestimonialCount.Value = (int.Parse(HiddenTestimonialCount.Value) + 10).ToString();
            LoadTestimonials(HiddenTestimonialCount.Value);
        }
    }
}