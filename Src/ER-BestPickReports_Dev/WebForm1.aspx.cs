using System;
using System.Net;
using System.Xml;

namespace ER_BestPickReports_Dev
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Get IP Address
            string ipaddress;
            ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipaddress == "" || ipaddress == null)
                ipaddress = Request.ServerVariables["REMOTE_ADDR"];

            // If you run this project on your local host (your computer) then you need to uncomment the next line to avoid getting the UNKOWN IP address.
            // Otherwise, the next line should be commented out if you are running this project on a web server.
            ipaddress = "74.125.45.100";	

            // Lookup geographic location using IP address
            XmlTextReader XmlRdr = GetLocation(ipaddress);

            if (XmlRdr != null)
            {

                // retrieve geolocation information from XML and display in web form
                while (XmlRdr.Read())
                {

                    if (XmlRdr.Name.ToString() == "cityName") lblCity.Text = XmlRdr.ReadString().Trim();
                    if (XmlRdr.Name.ToString() == "regionName") lblRegion.Text = XmlRdr.ReadString().Trim();
                    if (XmlRdr.Name.ToString() == "countryName") lblCountry.Text = XmlRdr.ReadString().Trim();
                    if (XmlRdr.Name.ToString() == "countryCode") lblCountryCode.Text = XmlRdr.ReadString().Trim();
                    if (XmlRdr.Name.ToString() == "zipCode") lblZip.Text = XmlRdr.ReadString().Trim();
                    if (XmlRdr.Name.ToString() == "longitude") lblLong.Text = XmlRdr.ReadString().Trim();
                    if (XmlRdr.Name.ToString() == "latitude") lblLat.Text = XmlRdr.ReadString().Trim();
                    lblIP.Text = ipaddress;
                }

                XmlRdr.Close();
                myLat.Text = lblLat.Text;
                myLong.Text = lblLong.Text;
            }
            else
            {
                lblIP.Text = "Unknown";
            }

        }

        protected XmlTextReader GetLocation(string ipaddress)
        {

            // Register at ipinfodb.com for a free key and put it here
            string myKey = "27f5c129d2d926e633f832dd7323f2145e0f3aecb515a931b9a836ddd17261bf";

            //Create a WebRequest
            WebRequest rssReq = WebRequest.Create("http://api.ipinfodb.com/v3/ip-city/?key=" + myKey + "&ip=" + ipaddress + "&format=xml");

            //Create a Proxy
            WebProxy px = new WebProxy("http://api.ipinfodb.com/v3/ip-city/?key=" + myKey + "&ip=" + ipaddress + "&format=xml", true);

            //Assign the proxy to the WebRequest
            rssReq.Proxy = px;

            //Set the timeout in Seconds for the WebRequest
            rssReq.Timeout = 2000;
            try
            {
                //Get the WebResponse 
                WebResponse rep = rssReq.GetResponse();

                //Read the Response in a XMLTextReader
                XmlTextReader xtr = new XmlTextReader(rep.GetResponseStream());
                return xtr;

            }
            catch
            {
                return null;
            }
        }
    }
}