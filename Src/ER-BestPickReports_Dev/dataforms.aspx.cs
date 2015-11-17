using System;
using System.Web;
using ER_BestPickReports_Dev.App_Code.Models;

namespace ER_BestPickReports_Dev
{
    public partial class dataforms : BasePage
    {
        int pagecityid = 0;
        int pageareaid = 0;
        string formtype = "0";

        private AppCookies bprPreferences = AppCookies.CreateInstance();

        protected void Page_Load(object sender, EventArgs e)
        {
            formtype = (HttpContext.Current.Items["type"].ToString() == "" || HttpContext.Current.Items["type"] == null) ? "" : HttpContext.Current.Items["type"].ToString();

            pagecityid = bprPreferences.CityId;
            pageareaid = bprPreferences.AreaId;

            if (formtype == "1")
            {
                NewsletterForm.Visible = true;
                GuideForm.Visible = false;
                PageHeading.Text = "Sign Up For Monthly Home Care Tips And Articles";
            }
            else if (formtype == "2")
            {
                NewsletterForm.Visible = false;
                GuideForm.Visible = true;
                PageHeading.Text = "Request a 2015 Guide";
            }

            //Hide form errors and success on load
            NewsletterErrorPanel.Visible = false;
            FormSuccess.Visible = false;
            GuideErrorPanel.Visible = false;

        }

        protected void Newsletter_Submit_Click(object sender, EventArgs e)
        {
            //validate form
            if (Newsletter_FirstName.Text.Trim() == "" || Newsletter_LastName.Text.Trim() == "" || Newsletter_EMail.Text.Trim() == "")
            {
                NewsletterError.Text = "Please fill out the form completely.";
                NewsletterErrorPanel.Attributes.Add("style", "color: Red");
                NewsletterErrorPanel.Visible = true;
                return;
            }

            if (!Global.IsEmail(Newsletter_EMail.Text.Trim()))
            {
                NewsletterError.Text = "Please enter a valid email address.";
                NewsletterErrorPanel.Attributes.Add("style", "color: Red");
                NewsletterErrorPanel.Visible = true;
                return;
            }

            Global.SaveFormData(Newsletter_FirstName.Text.Trim(), Newsletter_MI.Text.Trim(), Newsletter_LastName.Text.Trim(), Newsletter_EMail.Text.Trim(), "", "", "", "", false, false, 1, pageareaid, pagecityid);

            //Clear form
            Newsletter_FirstName.Text = "";
            Newsletter_LastName.Text = "";
            Newsletter_MI.Text = "";
            Newsletter_EMail.Text = "";

            SuccessText.Text = "Thank you for signing up!";
            NewsletterForm.Visible = false;
            GuideForm.Visible = false;
            FormSuccess.Visible = true;
        }

        protected void Guide_Submit_Click(object sender, EventArgs e)
        {
            //validate form
            if (Guide_FirstName.Text.Trim() == "" || Guide_LastName.Text.Trim() == "" || Guide_Email.Text.Trim() == "" || Guide_Address.Text.Trim() == "" || Guide_City.Text.Trim() == "" || Guide_State.Text.Trim() == "" || Guide_Zip.Text.Trim() == "")
            {
                GuideError.Text = "Please fill out the form completely.";
                GuideErrorPanel.Attributes.Add("style", "color: Red");
                GuideErrorPanel.Visible = true;
                return;
            }

            if (!Global.IsEmail(Guide_Email.Text.Trim()))
            {
                GuideError.Text = "Please enter a valid email address.";
                GuideErrorPanel.Attributes.Add("style", "color: Red");
                GuideErrorPanel.Visible = true;
                return;
            }

            Global.SaveFormData(Guide_FirstName.Text.Trim(), Guide_MI.Text.Trim(), Guide_LastName.Text.Trim(), Guide_Email.Text.Trim(), Guide_Address.Text.Trim(), Guide_City.Text.Trim(), Guide_State.Text.Trim(), Guide_Zip.Text.Trim(), Guide_Updates.Checked, false, 2, pageareaid, pagecityid);

            //Clear form
            Guide_FirstName.Text = "";
            Guide_MI.Text = "";
            Guide_LastName.Text = "";
            Guide_Email.Text = "";
            Guide_Address.Text = "";
            Guide_City.Text = "";
            Guide_State.Text = "";
            Guide_Zip.Text = "";
            Guide_Updates.Checked = false;
            //Guide_FutureEditions.Checked = false;

            SuccessText.Text = "Thank you for requesting a guide.";
            NewsletterForm.Visible = false;
            GuideForm.Visible = false;
            FormSuccess.Visible = true;
        }
    }
}