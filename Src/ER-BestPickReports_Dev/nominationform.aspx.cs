using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace ER_BestPickReports_Dev
{
    public partial class nominationform : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Type.SelectedValue != "")
            {
                if (Type.SelectedValue == "Business Owner")
                {
                    BusinessPanel.Visible = true;
                    SelectPanel.Visible = false;
                    ButtonPanel.Visible = true;
                }
                else if (Type.SelectedValue == "Homeowner")
                {
                    SelectPanel.Visible = false;
                    ButtonPanel.Visible = true;
                    HomePanel.Visible = true;
                }
                else
                {
                    SelectPanel.Visible = true;
                    ButtonPanel.Visible = false;
                    HomePanel.Visible = false;
                }
            }
        }

        protected void GetForm(object sender, EventArgs e)
        {
            
        }

        protected void Form_Click(object sender, EventArgs e)
        {
            bool error = false;

            string strname = "";
            string strbusiness = "";
            string address = "";
            string citystatezip = "";
            string phone = "";
            string contact = "";
            string website = "";
            string email = "";
            string services = "";

            if (Page.IsValid)
            {
                if (Type.SelectedValue == "Business Owner")
                {
                    //error checking
                    if (BYourName.Text.Trim() == "")
                    {
                        BYourNameReq.Visible = true;
                        error = true;
                    }
                    else
                    {
                        BYourNameReq.Visible = false;
                    }

                    if (BName.Text.Trim() == "")
                    {
                        BNameReq.Visible = true;
                        error = true;
                    }
                    else
                    {
                        BNameReq.Visible = false;
                    }

                    if (BAddress.Text.Trim() == "")
                    {
                        BAddressReq.Visible = true;
                        error = true;
                    }
                    else
                    {
                        BAddressReq.Visible = false;
                    }

                    if (BCityStateZip.Text.Trim() == "")
                    {
                        BCityStateZipReq.Visible = true;
                        error = true;
                    }
                    else
                    {
                        BCityStateZipReq.Visible = false;
                    }

                    if (BPhone.Text.Trim() == "")
                    {
                        BPhoneReq.Visible = true;
                        error = true;
                    }
                    else
                    {
                        BPhone.Visible = false;
                    }

                    if (BEmail.Text.Trim() == "" || !Global.IsEmail(BEmail.Text.Trim()))
                    {
                        BEmailReq.Visible = true;
                        error = true;
                    }
                    else
                    {
                        BEmailReq.Visible = false;
                    }

                    if (BServices.Text.Trim() == "")
                    {
                        BServicesReq.Visible = true;
                        error = true;
                    }
                    else
                    {
                        BServicesReq.Visible = false;
                    }

                    //Set variables
                    strname = BYourName.Text.Trim();
                    strbusiness = BName.Text.Trim();
                    address = BAddress.Text.Trim();
                    citystatezip = BCityStateZip.Text.Trim();
                    phone = BPhone.Text.Trim();
                    website = BWebsite.Text.Trim();
                    email = BEmail.Text.Trim();
                    services = BServices.Text.Trim();
                }
                else if (Type.SelectedValue == "Homeowner")
                {
                    //error checking
                    if (HYourName.Text.Trim() == "")
                    {
                        HYourNameReq.Visible = true;
                        error = true;
                    }
                    else
                    {
                        HYourNameReq.Visible = false;
                    }

                    if (HName.Text.Trim() == "")
                    {
                        HNameReq.Visible = true;
                        error = true;
                    }
                    else
                    {
                        HNameReq.Visible = false;
                    }

                    //Set variables
                    strname = HYourName.Text.Trim();
                    strbusiness = HName.Text.Trim();
                    contact = HContact.Text.Trim();
                    phone = HPhone.Text.Trim();
                    website = HWebsite.Text.Trim();
                    services = HServices.Text.Trim();
                }

                if (error)
                    return;

                //Customize below for the body of the email
                string body = "<font face=Arial size=2>You have received a nomination from the Best Pick Reports website.<br>Details are as follows:<br><br>";
                body += "Nomination Type: " + Type.SelectedValue.ToString() + "<br><br>";
                body += "Name: " + strname + "<br>";
                body += "Business Name: " + strbusiness + "<br>";
                if (address != "")
                    body += "Business Address: " + address + "<br>";
                if (citystatezip != "")
                    body += "Business City, State and Zip Code: " + citystatezip + "<br>";
                if (contact != "")
                    body += "Business Contact Name: " + contact + "<br>";
                body += "Business Phone: " + phone + "<br>";
                body += "Business Website: " + website + "<br>";
                if (email != "")
                    body += "Business Email Address: " + email + "<br>";
                body += "Services Provided: " + services + "<br>";
                body += "</font>";

                Global.SendEmailNotification("nominations@ebscoresearch.com", "New Business Nomination", "nominations@bestpickreports.com", body, true);

                ShowForm.Visible = false;
                FormSuccess.Visible = true;
            }
        }
    }
}