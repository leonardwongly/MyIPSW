using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;
using System.Data;
using Newtonsoft.Json;
using System.Web.Script;
using System.Web.Script.Serialization;
using System.IO;
using System.Collections;

namespace MyIPSW
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnRetrieve.Visible = false;
            lblStep2.Visible = false;
            lblStep3.Visible = false;
            ddliPhone.Visible = false;
            ddliPad.Visible = false;
            ddliPod.Visible = false;
            ddlWatch.Visible = false;
            ddlAppleTV.Visible = false;
            ddlAudioAccessory.Visible = false;

            WebClient webClient = new WebClient();
            string myJSON = webClient.DownloadString("https://api.ipsw.me/v4/devices");

            dynamic jsonObj = JsonConvert.DeserializeObject(myJSON);
            for (int i = 0; i < jsonObj.Count; i++)
            {

                string name = jsonObj[i]["name"].ToString();
                string identifier = jsonObj[i]["identifier"].ToString();
                if (identifier.Contains("iPhone"))
                {
                    ddliPhone.Items.Add(new ListItem(name, identifier));
                }
                else if (identifier.Contains("iPad"))
                {
                    ddliPad.Items.Add(new ListItem(name, identifier));
                }
                else if (identifier.Contains("Watch"))
                {
                    ddlWatch.Items.Add(new ListItem(name, identifier));
                }
                else if (identifier.Contains("AudioAccessory")) //HomePod
                {
                    ddlAudioAccessory.Items.Add(new ListItem(name, identifier));
                }
                else if (identifier.Contains("iPod"))
                {
                    ddliPod.Items.Add(new ListItem(name, identifier));
                }
                else if (identifier.Contains("AppleTV"))
                {
                    ddlAppleTV.Items.Add(new ListItem(name, identifier));
                }

                
            }
            
        }

        protected void ddliPhone_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSelection.Text = ddliPhone.SelectedValue.ToString();
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected <b>" + rblOptions.SelectedItem.ToString() + " " + ddliPhone.SelectedItem.ToString() + "</b> with the identifier: ";
            btnRetrieve.Text = "Retrieve";
            btnRetrieve.Visible = true;

            lblStep3.Visible = true;
            lblStep3.Font.Bold = true;

            if (rblOptions.SelectedItem.ToString().Equals("Official"))
            {
                lblStep2.Visible = true;
                ddliPhone.Visible = true;
                ddliPad.Visible = true;
                ddliPod.Visible = true;
                ddlWatch.Visible = false;
                ddlAppleTV.Visible = false;
                ddlAudioAccessory.Visible = false;
            }
            else if (rblOptions.SelectedItem.ToString().Equals("OTA"))
            {
                lblStep2.Visible = true;
                ddliPhone.Visible = true;
                ddliPad.Visible = true;
                ddliPod.Visible = true;
                ddlWatch.Visible = true;
                ddlAppleTV.Visible = true;
                ddlAudioAccessory.Visible = true;
            }
        }

        protected void rblOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblOptions.SelectedItem.ToString().Equals("Official"))
            {
                lblStep2.Visible = true;
                ddliPhone.Visible = true;
                ddliPad.Visible = true;
                ddliPod.Visible = true;
                ddlWatch.Visible = false;
                ddlAppleTV.Visible = false;
                ddlAudioAccessory.Visible = false;
            }
            else if (rblOptions.SelectedItem.ToString().Equals("OTA"))
            {
                lblStep2.Visible = true;
                ddliPhone.Visible = true;
                ddliPad.Visible = true;
                ddliPod.Visible = true;
                ddlWatch.Visible = true;
                ddlAppleTV.Visible = true;
                ddlAudioAccessory.Visible = true;
            }
        }
    }
}