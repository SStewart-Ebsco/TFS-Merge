using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace ER_BestPickReports_Dev.App_Code
{
    public class Testimonial
    {
        private string _author;
        private string _text;
        private DateTime _publishedDate;

        public string author
        { get { return _author; } }

        public string text
        { get { return _text; } }

        public DateTime publishedDate
        { get { return _publishedDate; } }

        public Testimonial(string tAuthor, string tText, DateTime tPublishDate)
        {
            _author = tAuthor;
            _text = tText;
            _publishedDate = tPublishDate;
        }

        public static List<Testimonial> GetByContractorIDCategoryIDAreaID(int contractorID, int categoryID, int areaID, DateTime pDate)
        {
            List<Testimonial> testimonialList = new List<Testimonial>();

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr = null;

            string queryString = @"SELECT T.Author, T.Body, T.PublishDate 
                                    FROM Testimonial T
	                                    INNER JOIN TestimonialContractorArea TCA ON (T.TestimonialID = TCA.TestimonialID)
                                    WHERE (TCA.CategoryID = @CATID) 
	                                    AND (TCA.AreaID = @AREAID) 
                                        AND (T.PublishDate <= @DATE) 
                                        AND (T.ContractorID = @CID) ";

            SqlCommand command = new SqlCommand(queryString, conn);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("CATID", SqlDbType.Int).Value = categoryID;
            command.Parameters.Add("AREAID", SqlDbType.Int).Value = areaID;
            command.Parameters.Add("CID", SqlDbType.Int).Value = contractorID;
            command.Parameters.Add("DATE", SqlDbType.DateTime).Value = pDate;

            try
            {
                conn.Open();

                rdr = command.ExecuteReader();

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        string tAuthor = rdr["Author"].ToString();
                        string tText = rdr["Body"].ToString();
                        DateTime tPubDate = Convert.ToDateTime(rdr["PublishDate"]);
                        Testimonial t = new Testimonial(tAuthor, tText, tPubDate);
                        testimonialList.Add(t);
                    }
                }
            }
            finally
            {
                if (rdr != null)
                {
                    if (!rdr.IsClosed)
                        rdr.Close();
                    rdr.Dispose();
                }
                rdr = null;

                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
                conn = null;
            }

            return testimonialList;
        }




    }
}