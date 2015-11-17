using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using ER_BestPickReports_Dev.App_Code;
using System.Data.SqlClient;
using ER_BestPickReports_Dev.Helpers;

namespace ER_BestPickReports_Dev
{
    [ServiceContract(Namespace = "ER_BestPickReports_Dev")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BlogService
    {

        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        public string DoWork(EmailFormContract emailForm)
        {
            // Add your operation implementation here
            return "Hello";
        }

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        protected BlogServiceResponse NewsletterSubscribe(NewsletterContract subscribeRequest)
        {
            var response = new BlogServiceResponse();

            if (String.IsNullOrEmpty(subscribeRequest.Email.Trim()))
            {
                response.Message = "Please fill out the form completely.";
                response.Success = false;
                return response;
            }

            if (!Global.IsEmail(subscribeRequest.Email.Trim()))
            {
                response.Message = "Please enter a valid email address.";
                response.Success = false;
                return response;
            }

            Global.SaveFormData("", "", "", subscribeRequest.Email.Trim(), "", "", "", "", false, false, 1, subscribeRequest.AreaId, subscribeRequest.CityId);
            response.Message = "Thank you for signing up!";
            return response;
        }

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        public BlogServiceResponse SubmitEmailForm(EmailFormContract emailForm)
        {
            string sql = "";
            var dataAccessHelper = new DataAccessHelper();

            bool error = false;

            #region Error Checking

            bool isFirstNameValid = true,
                isLastNameValid = true,
                isStreetAddressValid = true,
                isZipValid = true,
                isEmailValid = true,
                isPrimaryPhoneValid = true,
                isWorkTypeValid = true,
                isMessageValid = true;

            if (String.IsNullOrEmpty(emailForm.FirstName))
                isFirstNameValid = false;
            if (String.IsNullOrEmpty(emailForm.LastName))
                isLastNameValid = false;
            if (String.IsNullOrEmpty(emailForm.StreetAddress))
                isStreetAddressValid = false;
            if (String.IsNullOrEmpty(emailForm.ZipCode) || !Global.IsZipCode(emailForm.ZipCode))
                isZipValid = false;
            if (String.IsNullOrEmpty(emailForm.Email) || !Global.IsEmail(emailForm.Email))
                isEmailValid = false;
            if (String.IsNullOrEmpty(emailForm.PrimaryPhone) || !Global.IsPhoneNumber(emailForm.PrimaryPhone))
                isPrimaryPhoneValid = false;
            if (String.IsNullOrEmpty(emailForm.TypeOfWork))
                isWorkTypeValid = false;
            if (String.IsNullOrEmpty(emailForm.Message))
                isMessageValid = false;

            error =
                !(isFirstNameValid && isLastNameValid && isStreetAddressValid && isZipValid && isEmailValid &&
                  isPrimaryPhoneValid && isWorkTypeValid && isMessageValid);

            #endregion

            if (!error)
            {
                string contractorMsg = "";
                contractorMsg += "<span style=\"font-family:Arial; font-size:12px;\"><strong>Name: </strong>" +
                                 emailForm.FirstName + " " + emailForm.LastName + "<br/><strong>Address: </strong>" +
                                 emailForm.StreetAddress + "<br/><strong>City: </strong>" + emailForm.City +
                                 "<br/><strong>Zip Code: </strong>" + emailForm.ZipCode +
                                 "<br/><strong>Email: </strong>" + emailForm.Email + "<br>";
                contractorMsg += "<strong>Phone: </strong>" + emailForm.PrimaryPhone + "<br/><strong>Phone 2: </strong>" +
                                 emailForm.AlternatePhone + "<br/><strong>Work Type: </strong>" + emailForm.TypeOfWork +
                                 "<br/><strong>Message: </strong>" + emailForm.Message;
                contractorMsg += "</span>";
                //string adminMsg = "<font face='Arial' size='2'>Someone showed interest in <strong>" + ContractorName.Value + "</strong><br/><br/>" + contractorMsg + "</font>";

                string subjectMsg = "";

                //Parse comma separated list of emails - check if we are sending to multiples first
                if (emailForm.ContractorEmails != null && !BWSession.emailStartedAndSent)
                {
                    BWSession.emailStartedAndSent = true;

                    string localconid = "";

                    try
                    {
                        foreach (string strid in emailForm.ContractorEmails)
                        {
                            subjectMsg = "Best Pick Reports Website Lead";

                            //Lookup emails from contractorcategoryareaid
                            InfoLevel conInfoLevel = InfoLevel.Area;
                            bool isPrimary = false;
                            // if contractorCategoryId is Primary
                            DataRow conRow = dataAccessHelper.FindInfoRecord(InfoType.ContractorCategoryArea, int.Parse(strid), ref conInfoLevel, ref isPrimary);
                            if (conRow != null)
                            {
                                //Lookup contractor id
                                sql =
                                    "SELECT ContractorID FROM ContractorCategoryInfo WHERE ContractorCategoryID = @CCID";
                                object o = dataAccessHelper.Data.ExecuteScalar(sql,
                                    new SqlParameter("@CCID", conRow["contractorcategoryid"].ToString()));
                                if (o != null)
                                    localconid = o.ToString();

                                string[] emails = conRow["email"].ToString().Trim().Split(',');
                                foreach (string stremail in emails)
                                {
                                    if (Global.IsEmail(stremail.Trim()))
                                    {
                                        try
                                        {
                                            // Send email to contractor
                                            //!!!!HARD CODED FOR TLC DECKS
                                            if (localconid == "215")
                                                subjectMsg = "Home Reports – Dispatch 6106";

                                            Global.SendEmailNotification(stremail.Trim(), subjectMsg,
                                                ConfigurationManager.AppSettings["EmailNotification"], contractorMsg,
                                                true);
                                        }
                                        catch (Exception)
                                        {
                                            // TODO: log error in database that email couldn't be sent???
                                        }
                                    }
                                }

                                // commit contractorMsg to database
                                string insertsql =
                                    "INSERT INTO EmailData (DateSent, AreaID, CityID, CategoryID, ContractorID, FirstName, LastName, Address, City, Zip, Email, Phone, Phone2, WorkType, Message, IsPPC) " +
                                    "VALUES (@DATESENT, @AREAID, @CITYID, @CATEGORYID, @CONTRACTORID, @FIRSTNAME, @LASTNAME, @ADDRESS, @CITY, @ZIP, @EMAIL, @PHONE, @PHONE2, @WORKTYPE, @MESSAGE, @PPC)";
                                dataAccessHelper.Data.ExecuteNonQuery(insertsql,
                                    new SqlParameter("@DATESENT", DateTime.Now),
                                    new SqlParameter("@AREAID", emailForm.AreaId), //TODO get rest of the values
                                    new SqlParameter("@CITYID", emailForm.CityId),
                                    new SqlParameter("@CATEGORYID", emailForm.CategoryId),
                                    new SqlParameter("@CONTRACTORID", int.Parse(localconid)),
                                    new SqlParameter("@FIRSTNAME", emailForm.FirstName),
                                    new SqlParameter("@LASTNAME", emailForm.LastName),
                                    new SqlParameter("@ADDRESS", emailForm.StreetAddress),
                                    new SqlParameter("@CITY", emailForm.City),
                                    new SqlParameter("@ZIP", emailForm.ZipCode.Trim()),
                                    new SqlParameter("@EMAIL", emailForm.Email),
                                    new SqlParameter("@PHONE", emailForm.PrimaryPhone),
                                    new SqlParameter("@PHONE2", emailForm.AlternatePhone),
                                    new SqlParameter("@WORKTYPE", emailForm.TypeOfWork),
                                    new SqlParameter("@PPC", emailForm.IsPpc),
                                    new SqlParameter("@MESSAGE", emailForm.Message.Trim()));
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        BWSession.emailStartedAndSent = false;
                    }
                }
                /*
                //Don't need to use separated fields for single and multiple contractors
                else if (ContractorEmail.Value.Trim() != "" && !BWSession.emailStartedAndSent)
                {
                    string[] emails = ContractorEmail.Value.Trim().Split(',');
                    foreach (string stremail in emails)
                    {
                        subjectMsg = "Best Pick Reports Website Lead";

                        if (Global.IsEmail(stremail.Trim()))
                        {
                            try
                            {
                                // Send email to contractor
                                //!!!!HARD CODED FOR TLC DECKS
                                if (EmailContractorID.Value == "215")
                                    subjectMsg = "Home Reports – Dispatch 6106";

                                Global.SendEmailNotification(stremail.Trim(), subjectMsg, ConfigurationManager.AppSettings["EmailNotification"], contractorMsg, true);
                                BWSession.emailStartedAndSent = true;
                            }
                            catch (Exception)
                            {
                                // TODO: log error in database that email couldn't be sent???
                            }
                        }
                    }

                    // commit contractorMsg to database
                    string insertsql = "INSERT INTO EmailData (DateSent, AreaID, CityID, CategoryID, ContractorID, FirstName, LastName, Address, City, Zip, Email, Phone, Phone2, WorkType, Message, IsPPC) " +
                        "VALUES (@DATESENT, @AREAID, @CITYID, @CATEGORYID, @CONTRACTORID, @FIRSTNAME, @LASTNAME, @ADDRESS, @CITY, @ZIP, @EMAIL, @PHONE, @PHONE2, @WORKTYPE, @MESSAGE, @PPC)";
                    basePage.Data.ExecuteNonQuery(insertsql,
                        new SqlParameter("@DATESENT", DateTime.Now),
                        new SqlParameter("@AREAID", areaID),
                        new SqlParameter("@CITYID", cityID),
                        new SqlParameter("@CATEGORYID", catid),
                        new SqlParameter("@CONTRACTORID", int.Parse(EmailContractorID.Value)),
                        new SqlParameter("@FIRSTNAME", name),
                        new SqlParameter("@LASTNAME", lastname),
                        new SqlParameter("@ADDRESS", addr),
                        new SqlParameter("@CITY", City.Text.Trim()),
                        new SqlParameter("@ZIP", Zip.Text.Trim()),
                        new SqlParameter("@EMAIL", email),
                        new SqlParameter("@PHONE", phone),
                        new SqlParameter("@PHONE2", phone2),
                        new SqlParameter("@WORKTYPE", worktype),
                        new SqlParameter("@PPC", isppc),
                        new SqlParameter("@MESSAGE", Message.Text.Trim()));
                }
                */

                //// Hide the email form and show the "Email Sent" div
                //ModalEmailForm.Visible = false;
                //ReqPanel.Visible = false;
                //EmailComplete.Visible = true;
                return new BlogServiceResponse(true, "Email sent.");
            }
            else
            {
                //// If this style isn't set, the modal popup won't display
                //ModalEmailForm.Style.Value = "display:block";
                //ReqPanel.Visible = true;
                //SendEmail.Show();
                return new BlogServiceResponse(false, "Email send failed.");
            }
        }

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        protected BlogServiceResponse SubmitNewsletterSignUpForm(NewsletterSignUpFormContract subscribeRequest)
        {
            var response = new BlogServiceResponse();

            bool error = false;

            #region Error Checking
            bool isFirstNameValid = true,
                isLastNameValid = true,
                isEmailValid = true;

            if (String.IsNullOrEmpty(subscribeRequest.FirstName))
                isFirstNameValid = false;
            if (String.IsNullOrEmpty(subscribeRequest.LastName))
                isLastNameValid = false;
            if (String.IsNullOrEmpty(subscribeRequest.Email) || !Global.IsEmail(subscribeRequest.Email))
                isEmailValid = false;

            error = !(isFirstNameValid && isLastNameValid  && isEmailValid);
            #endregion

            if (!error)
            {
                Global.SaveFormData(subscribeRequest.FirstName, "", subscribeRequest.LastName, subscribeRequest.Email.Trim(), "", "", "", "", false, false, 1, subscribeRequest.AreaId, subscribeRequest.CityId);
                response.Message = "Thank you for signing up!";
                return response;
            }
            else
            {
                response.Message = "Please fill out the form completely.";
                response.Success = false;
                return response;
            }
        }

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        protected BlogServiceResponse RequestTopicForm(RequestTopicFormContract topicRequest)
        {
            bool error = false;
            string name = topicRequest.FirstName.Trim();
            string lastname = topicRequest.LastName.Trim();
            string address = topicRequest.StreetAddress.Trim();
            string city = topicRequest.City.Trim();
            string state = topicRequest.State.Trim();
            string zip = topicRequest.ZipCode.Trim();
            string email = topicRequest.Email.Trim();
            bool isReceiveUpdates = topicRequest.IsReceiveUpdates;
            int areaId = topicRequest.AreaId;
            int cityId = topicRequest.CityId;

            #region Error Checking

            bool isFirstNameValid = true,
                isLastNameValid = true,
                isStreetAddressValid = true,
                isZipValid = true,
                isEmailValid = true;

            if (String.IsNullOrEmpty(name))
                isFirstNameValid = false;
            if (String.IsNullOrEmpty(lastname))
                isLastNameValid = false;
            if (String.IsNullOrEmpty(address))
                isStreetAddressValid = false;
            if (String.IsNullOrEmpty(zip) || !Global.IsZipCode(zip))
                isZipValid = false;
            if (String.IsNullOrEmpty(email) || !Global.IsEmail(email))
                isEmailValid = false;

            error = !(isFirstNameValid && isLastNameValid && isStreetAddressValid && isZipValid && isEmailValid);
            #endregion Error Checking

            if (!error)
            {
                //Parse comma separated list of emails - check if we are sending to multiples first
                if (!BWSession.emailStartedAndSent)
                {
                    try
                    {
                        BWSession.emailStartedAndSent = true;
                        Global.SaveFormData(name, "", lastname, email, address, city, state, zip, isReceiveUpdates, false, 2, areaId, cityId);
                    }
                    catch (Exception)
                    {
                        // TODO: log error in database that email couldn't be sent???
                    }
                    finally
                    {
                        BWSession.emailStartedAndSent = false;
                    }
                }

                return new BlogServiceResponse(true, "Email sent.");
            }
            else
            {
                return new BlogServiceResponse(false, "Email send failed.");
            }
        }


        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        protected BlogServiceResponse ChangeZipCoded(string zipCode)
        {
            bool wasLocationChanged = LocationHelper.TryChangeLocationByZip(zipCode);

            if (wasLocationChanged)
            {
                return new BlogServiceResponse(true, "Location was changed.");
            }
            else
            {
                return new BlogServiceResponse(false, "Location wasn't changed.");
            }
        }

        // Add more operations here and mark them with [OperationContract]
    }

    [DataContract]
    public class EmailFormContract
    {
        [DataMember(Name = "contractorEmails")]
        public List<string> ContractorEmails = new List<string>();

        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "streetAddress")]
        public string StreetAddress { get; set; }

        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "zipCode")]
        public string ZipCode { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "primaryPhone")]
        public string PrimaryPhone { get; set; }

        [DataMember(Name = "alternatePhone")]
        public string AlternatePhone { get; set; }

        [DataMember(Name = "typeOfWork")]
        public string TypeOfWork { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
        
        [DataMember(Name = "cityId")]
        public int CityId { get; set; }

        [DataMember(Name = "areaId")]
        public int AreaId { get; set; }

        [DataMember(Name = "categoryId")]
        public int CategoryId { get; set; }

        [DataMember(Name = "isPpc")]
        public bool IsPpc { get; set; }
    }

    [DataContract]
    public class NewsletterContract
    {
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "cityId")]
        public int CityId { get; set; }

        [DataMember(Name = "areaId")]
        public int AreaId { get; set; }
    }

    [DataContract]
    public class RequestTopicFormContract
    {
        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "streetAddress")]
        public string StreetAddress { get; set; }

        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "zipCode")]
        public string ZipCode { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "isReceiveUpdates")]
        public bool IsReceiveUpdates { get; set; }

        [DataMember(Name = "cityId")]
        public int CityId { get; set; }

        [DataMember(Name = "areaId")]
        public int AreaId { get; set; }
    }

    [DataContract]
    public class NewsletterSignUpFormContract
    {
        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "cityId")]
        public int CityId { get; set; }

        [DataMember(Name = "areaId")]
        public int AreaId { get; set; }
    }

    [DataContract]
    public class BlogServiceResponse
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; }
        
        [DataMember(Name = "message")]
        public string Message { get; set; }

        public BlogServiceResponse()
        {
            Success = true;
        }

        public BlogServiceResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}