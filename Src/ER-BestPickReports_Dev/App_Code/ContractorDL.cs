using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace ER_BestPickReports_Dev.App_Code
{
    public class ContractorDL
    {
        private string _name;
        private int _infoID;

        public string name
        { get { return _name; } }

        public int infoID
        { get { return _infoID; } }

        public string shortDescription { get; set; }
        public string reviewSummary { get; set; }
        public string urlName { get; set; }
        public string email { get; set; }
        public List<PhoneDL> phoneNumbers;
        public string bestPickText { get; set; }
        public string servicesOffered { get; set; }
        public string servicesNotOffered { get; set; }
        public string specializations { get; set; }
        public string minimumJob { get; set; }
        public string warranty { get; set; }
        public string licenseNumber { get; set; }
        public char? workersCompensation { get; set; }
        public string awardsAndCertifications { get; set; }
        public string organizations { get; set; }
        public string companyHistory { get; set; }
        public string employeeInformation { get; set; }
        public string productInformation { get; set; }
        public string hrStatus { get; set; }
        public string additionalInformation { get; set; }
        public bool hasLiability { get; set; }
        public QuoteDL quote { get; set; }
        public MetaDataDL metaData { get; set; }

        public ContractorDL(string cName, int cInfoID)
        {
            _name = cName;
            _infoID = cInfoID;
            phoneNumbers = new List<PhoneDL>();
            quote = null;
            metaData = null;
        }

        public static ContractorDL Get(InfoType infoType, int objectID)
        {
            ContractorDL tempContractor = GetDB(infoType, objectID);
            //if we didn't get anything we need to try a high leve of "info"
            while (tempContractor == null && infoType != InfoType.None)
            {
                //get the next info level
                InfoTypeDL.GetNextLevelUp(ref infoType, ref objectID);
                //fetch contractor again
                tempContractor = GetDB(infoType, objectID);
            }
            return tempContractor;
        }

        public static ContractorDL GetByInfoID(int infoID)
        {
            ContractorDL tempContractor = null;

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr = null;

            try
            {
                conn.Open();

                string strSQL = "SELECT * FROM Info WHERE infoID = @ID";

                SqlCommand sqlCommand = new SqlCommand(strSQL, conn);
                sqlCommand.Parameters.Add("ID", SqlDbType.Int).Value = infoID;

                rdr = sqlCommand.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        string cName = rdr["displayName"].ToString();
                        int cID = Convert.ToInt32(rdr["infoID"]);
                        tempContractor = new ContractorDL(cName, cID);

                        tempContractor.reviewSummary = rdr["reviewSummary"].ToString();
                        tempContractor.urlName = rdr["urlName"].ToString();
                        tempContractor.email = rdr["email"].ToString();

                        if (!String.IsNullOrEmpty(rdr["phone"].ToString()))
                            tempContractor.phoneNumbers.Add(new PhoneDL(rdr["phone"].ToString(), PhoneType.regular));
                        if (!String.IsNullOrEmpty(rdr["organicPhone"].ToString()))
                            tempContractor.phoneNumbers.Add(new PhoneDL(rdr["organicPhone"].ToString(), PhoneType.organic));
                        if (!String.IsNullOrEmpty(rdr["ppcPhone"].ToString()))
                            tempContractor.phoneNumbers.Add(new PhoneDL(rdr["ppcPhone"].ToString(), PhoneType.ppc));
                        if (!String.IsNullOrEmpty(rdr["facebookPhone"].ToString()))
                            tempContractor.phoneNumbers.Add(new PhoneDL(rdr["facebookPhone"].ToString(), PhoneType.facebook));

                        if (rdr["workersCompensation"] != DBNull.Value)
                            tempContractor.workersCompensation = rdr["workersCompensation"].ToString()[0];                            

                        tempContractor.hasLiability = Convert.ToBoolean(rdr["hasliability"]);

                        tempContractor.shortDescription = rdr["shortdesc"].ToString();
                        tempContractor.bestPickText = rdr["bestPickText"].ToString();
                        tempContractor.servicesOffered = rdr["servicesOffered"].ToString();
                        tempContractor.servicesNotOffered = rdr["servicesNotOffered"].ToString();
                        tempContractor.specializations = rdr["specializations"].ToString();
                        tempContractor.minimumJob = rdr["minimumJob"].ToString();
                        tempContractor.warranty = rdr["warranty"].ToString();
                        tempContractor.licenseNumber = rdr["licenseNumber"].ToString();
                        tempContractor.awardsAndCertifications = rdr["awardscertifications"].ToString();
                        tempContractor.organizations = rdr["organizations"].ToString();
                        tempContractor.companyHistory = rdr["companyHistory"].ToString();
                        tempContractor.employeeInformation = rdr["employeeInformation"].ToString();
                        tempContractor.productInformation = rdr["productInformation"].ToString();
                        tempContractor.hrStatus = rdr["hrStatus"].ToString();
                        tempContractor.additionalInformation = rdr["additionalInformation"].ToString();

                        tempContractor.quote = new QuoteDL(rdr["quoteName"].ToString(), rdr["quoteTitle"].ToString(), rdr["quote"].ToString());

                        tempContractor.metaData = new MetaDataDL(rdr["metakey"].ToString().Trim(), rdr["metaDesc"].ToString().Trim(), rdr["metaTitle"].ToString().Trim());
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
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }

            return tempContractor;
        }

        public static MetaDataDL GetMetadataByCategoryArea(int contractorCategoryAreaID)
        {
            MetaDataDL metaData = null;

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr = null;
            try
            {
                conn.Open();

                string strSQL = "SELECT metakey, metadesc, metatitle FROM ContractorCategoryArea WHERE ContractorCategoryAreaID = @ID";

                SqlCommand sqlCommand = new SqlCommand(strSQL, conn);
                sqlCommand.Parameters.Add("ID", SqlDbType.Int).Value = contractorCategoryAreaID;

                rdr = sqlCommand.ExecuteReader();
                if (rdr.HasRows)
                {
                    if (rdr.Read())
                    {
                        string key = "";
                        if (!String.IsNullOrEmpty(rdr["metakey"].ToString().Trim()))
                            key = rdr["metakey"].ToString().Trim();

                        string desc = "";
                        if (!String.IsNullOrEmpty(rdr["metadesc"].ToString().Trim()))
                            desc = rdr["metadesc"].ToString().Trim();

                        string title = "";
                        if (!String.IsNullOrEmpty(rdr["metatitle"].ToString().Trim()))
                            title = rdr["metatitle"].ToString().Trim();

                        metaData = new MetaDataDL(key, desc, title);
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
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }

            return metaData;
        }

        //public static List<ContractorDL> GetContractorsByCategoryArea(string cateId, string areaId)
        //{
        //    List<ContractorDL> contractors = new List<ContractorDL>();

        //    SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
        //    SqlDataReader rdr = null;
        //    try
        //    {
        //        conn.Open();

        //        string strSQL = "SELECT metakey, metadesc, metatitle FROM ContractorCategoryArea WHERE ContractorCategoryAreaID = @ID";

        //        SqlCommand sqlCommand = new SqlCommand(strSQL, conn);
        //        //sqlCommand.Parameters.Add("ID", SqlDbType.Int).Value = contractorCategoryAreaID;

        //        rdr = sqlCommand.ExecuteReader();
        //        if (rdr.HasRows)
        //        {
        //            if (rdr.Read())
        //            {
        //                //string key = "";
        //                //if (!String.IsNullOrEmpty(rdr["metakey"].ToString().Trim()))
        //                //    key = rdr["metakey"].ToString().Trim();

        //                //string desc = "";
        //                //if (!String.IsNullOrEmpty(rdr["metadesc"].ToString().Trim()))
        //                //    desc = rdr["metadesc"].ToString().Trim();

        //                //string title = "";
        //                //if (!String.IsNullOrEmpty(rdr["metatitle"].ToString().Trim()))
        //                //    title = rdr["metatitle"].ToString().Trim();

        //                //metaData = new MetaDataDL(key, desc, title);
        //            }
        //        }

        //    }
        //    finally
        //    {
        //        if (rdr != null)
        //        {
        //            if (!rdr.IsClosed)
        //                rdr.Close();
        //            rdr.Dispose();
        //        }
        //        if (conn != null)
        //        {
        //            if (conn.State == ConnectionState.Open)
        //                conn.Close();
        //            conn.Dispose();
        //        }
        //    }

        //    return contractors;
        //}

        private static ContractorDL GetDB(InfoType infoType, int objectID)
        {
            ContractorDL tempContractor = null;
            string tableName = infoType.ToString();
            string columnName = InfoTypeDL.GetColumnName(infoType);

            SqlConnection conn = new SqlConnection(BWConfig.ConnectionString);
            SqlDataReader rdr = null;

            try
            {
                conn.Open();

                string strSQL = "SELECT * FROM Info I JOIN " + tableName + " C ON (I.infoID = C.InfoID)  WHERE " + columnName + " = @ID";

                SqlCommand sqlCommand = new SqlCommand(strSQL, conn);
                sqlCommand.Parameters.Add("ID", SqlDbType.Int).Value = objectID;

                rdr = sqlCommand.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        string cName = rdr["displayName"].ToString();
                        int cID = Convert.ToInt32(rdr["infoID"]);
                        tempContractor = new ContractorDL(cName, cID);

                        tempContractor.reviewSummary = rdr["reviewSummary"].ToString();
                        tempContractor.urlName = rdr["urlName"].ToString();
                        tempContractor.email = rdr["email"].ToString();

                        if (!String.IsNullOrEmpty(rdr["phone"].ToString()))
                            tempContractor.phoneNumbers.Add(new PhoneDL(rdr["phone"].ToString(), PhoneType.regular));
                        if (!String.IsNullOrEmpty(rdr["organicPhone"].ToString()))
                            tempContractor.phoneNumbers.Add(new PhoneDL(rdr["organicPhone"].ToString(), PhoneType.organic));
                        if (!String.IsNullOrEmpty(rdr["ppcPhone"].ToString()))
                            tempContractor.phoneNumbers.Add(new PhoneDL(rdr["ppcPhone"].ToString(), PhoneType.ppc));
                        if (!String.IsNullOrEmpty(rdr["facebookPhone"].ToString()))
                            tempContractor.phoneNumbers.Add(new PhoneDL(rdr["facebookPhone"].ToString(), PhoneType.facebook));

                        if (rdr["workersCompensation"] != DBNull.Value)
                            tempContractor.workersCompensation = rdr["workersCompensation"].ToString()[0];                            

                        tempContractor.hasLiability = Convert.ToBoolean(rdr["hasliability"]);

                        tempContractor.shortDescription = rdr["shortdesc"].ToString();
                        tempContractor.bestPickText = rdr["bestPickText"].ToString();
                        tempContractor.servicesOffered = rdr["servicesOffered"].ToString();
                        tempContractor.servicesNotOffered = rdr["servicesNotOffered"].ToString();
                        tempContractor.specializations = rdr["specializations"].ToString();
                        tempContractor.minimumJob = rdr["minimumJob"].ToString();
                        tempContractor.warranty = rdr["warranty"].ToString();
                        tempContractor.licenseNumber = rdr["licenseNumber"].ToString();
                        tempContractor.awardsAndCertifications = rdr["awardscertifications"].ToString();
                        tempContractor.organizations = rdr["organizations"].ToString();
                        tempContractor.companyHistory = rdr["companyHistory"].ToString();
                        tempContractor.employeeInformation = rdr["employeeInformation"].ToString();
                        tempContractor.productInformation = rdr["productInformation"].ToString();
                        tempContractor.hrStatus = rdr["hrStatus"].ToString();
                        tempContractor.additionalInformation = rdr["additionalInformation"].ToString();

                        tempContractor.quote = new QuoteDL(rdr["quoteName"].ToString(), rdr["quoteTitle"].ToString(), rdr["quote"].ToString());

                        tempContractor.metaData = new MetaDataDL(rdr["metakey"].ToString().Trim(), rdr["metaDesc"].ToString().Trim(), rdr["metaTitle"].ToString().Trim());
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
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                    conn.Dispose();
                }
            }

            return tempContractor;
        }

    }
}