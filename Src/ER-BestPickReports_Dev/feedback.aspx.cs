using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ER_BestPickReports_Dev
{
    public partial class feedback : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ServerValidation(object source, ServerValidateEventArgs args)
        {
            try
            {
                // Get the control used to perform this Postback
                Control b = GetPostbackControl(this);

                // Only validate if a specific control was used for postback
                if (b != null && b.ID == "SubmitButton")
                {
                    // Perform required field validation
                    if (args.Value.Trim() == "")
                    {
                        args.IsValid = false;
                        return;
                    }
                }

                // Call this for other "ancillary" controls that performed Postback (modal controls)
                args.IsValid = true;
            }
            catch (Exception)
            {
                args.IsValid = false;
            }
        }

        /// <summary>
        /// Retrieves the control that caused the page Pastback
        /// </summary>
        protected System.Web.UI.Control GetPostbackControl(System.Web.UI.Page pageReference)
        {
            System.Web.UI.Control returnValue = null;

            if (pageReference.IsPostBack)
            {
                // Attempt to find the name of the postback control in the hidden __EVENTTARGET field...
                object eventTarget = pageReference.Request.Form["__EVENTTARGET"];

                // If __EVENTTARGET is not null or an empty string, return the control with that name...
                if ((eventTarget != null) && (eventTarget.ToString().Trim().Length > 0))
                {
                    return pageReference.FindControl(eventTarget.ToString().Trim());
                }

                // That will not work if the postback is caused by a standard Buttons.
                // For those, we need to retrieve the control from the Page's Form collection.

                foreach (string keyName in pageReference.Request.Form)
                {
                    System.Web.UI.Control currentControl = pageReference.FindControl(keyName);

                    // If found, and if it is a button, we're done...
                    if ((currentControl != null) && (currentControl is Button))
                    {
                        returnValue = currentControl;
                        break;
                    }
                }
            }

            return returnValue;
        }

        protected void Feedback_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                CaptchNote.Text = "Invalid Code!";
                CaptchNote.Visible = true;
                return;
            }

            string subject = "Best Pick Reports Feedback Submission";

            //Get Radio button values
            string overall = "";
            if (OverallA.Checked == true)
            {
                overall = "A";
            }
            else if (OverallB.Checked == true)
            {
                overall = "B";
            }
            else if (OverallC.Checked == true)
            {
                overall = "C";
            }
            else if (OverallD.Checked == true)
            {
                overall = "D";
            }
            else if (OverallF.Checked == true)
            {
                overall = "F";
            }
            else if (OverallNA.Checked == true)
            {
                overall = "N/A";
            }

            string work = "";
            if (QualityA.Checked == true)
            {
                work = "A";
            }
            else if (QualityB.Checked == true)
            {
                work = "B";
            }
            else if (QualityC.Checked == true)
            {
                work = "C";
            }
            else if (QualityD.Checked == true)
            {
                work = "D";
            }
            else if (QualityF.Checked == true)
            {
                work = "F";
            }
            else if (QualityNA.Checked == true)
            {
                work = "N/A";
            }

            string timeliness = "";
            if (TimelinessA.Checked == true)
            {
                timeliness = "A";
            }
            else if (TimelinessB.Checked == true)
            {
                timeliness = "B";
            }
            else if (TimelinessC.Checked == true)
            {
                timeliness = "C";
            }
            else if (TimelinessD.Checked == true)
            {
                timeliness = "D";
            }
            else if (TimelinessF.Checked == true)
            {
                timeliness = "F";
            }
            else if (TimelinessNA.Checked == true)
            {
                timeliness = "N/A";
            }

            string courteous = "";
            if (CourteousA.Checked == true)
            {
                courteous = "A";
            }
            else if (CourteousB.Checked == true)
            {
                courteous = "B";
            }
            else if (CourteousC.Checked == true)
            {
                courteous = "C";
            }
            else if (CourteousD.Checked == true)
            {
                courteous = "D";
            }
            else if (CourteousF.Checked == true)
            {
                courteous = "F";
            }
            else if (CourteousNA.Checked == true)
            {
                courteous = "N/A";
            }

            string neat = "";
            if (NeatA.Checked == true)
            {
                neat = "A";
            }
            else if (NeatB.Checked == true)
            {
                neat = "B";
            }
            else if (NeatC.Checked == true)
            {
                neat = "C";
            }
            else if (NeatD.Checked == true)
            {
                neat = "D";
            }
            else if (NeatF.Checked == true)
            {
                neat = "F";
            }
            else if (NeatNA.Checked == true)
            {
                neat = "N/A";
            }

            string price = "";
            if (PriceHigh.Checked == true)
            {
                price = "High";
            }
            else if (PriceAvg.Checked == true)
            {
                price = "Average";
            }
            else if (PriceLow.Checked == true)
            {
                price = "Low";
            }
            else if (PriceNA.Checked == true)
            {
                price = "N/A";
            }

            //Customize below for the body of the email
            string body = "<font face=Arial size=2>You have received feedback from the Best Pick Reports website.<br>Details are as follows:<br><br><strong>Company Information</strong><br><br>";
            body += "Company Name: " + CompanyName.Text.Trim() + "<br>";
            body += "Company Phone: " + CompanyPhone.Text.Trim() + "<br>";
            body += "Company Location: " + CompanyLocation.Text.Trim() + "<br>";
            body += "Company Type: " + CompanyType.Text.Trim() + "<br>";
            body += "Work/Job Description: " + Description.Text.Trim() + "<br><br><strong>Company Ratings</strong><br><br>";

            body += "Overall: " + overall + "<br>";
            body += "Work Quality: " + work + "<br>";
            body += "Timeliness: " + timeliness + "<br>";
            body += "Courteous: " + courteous + "<br>";
            body += "Neat & Tidy: " + neat + "<br>";
            body += "Price: " + price + "<br><br>Comments: " + Comments.Text.Trim() + "<br><br><strong>Personal Information</strong><br><br>";

            body += "Name: " + Name.Text.Trim() + "<br>";
            body += "Phone: " + Phone.Text.Trim() + "<br>";
            body += "City: " + City.Text.Trim() + "<br>";
            body += "State: " + State.Text.Trim() + "<br>";
            body += "Zip Code: " + Zip.Text.Trim() + "<br>";
            body += "Email Address: " + Email.Text.Trim() + "<br><br>";
            body += "</font>";

            Global.SendEmailNotification("research@ebscoresearch.com", subject, "feedback@bestpickreports.com", body, true);

            ShowForm.Visible = false;
            FormSuccess.Visible = true;
        }
    }
}