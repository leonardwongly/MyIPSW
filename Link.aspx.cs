using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ipsw
{
    public partial class Link : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tbData.Visible = false;
                tbData.Text = "";
                btnRetrieve.Visible = false;
                lblStep2.Visible = false;
                lblStep3.Visible = false;
                ddliPhone.Visible = false;
                ddliPad.Visible = false;
                ddliPod.Visible = false;
                ddlWatch.Visible = false;
                ddlAppleTV.Visible = false;
                ddlMac.Visible = false;
                ddlAudioAccessory.Visible = false;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = false;


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
                    else if (identifier.Contains("Mac"))
                    {
                        ddlMac.Items.Add(new ListItem(name, identifier));
                    }
                }

                string[] iOSVersion = new string[] { "1.0", "1.0.1", "1.0.2", "1.1", "1.1.1", "1.1.2", "1.1.3", "1.1.4", "1.1.5", //Checked
                                                         "2.0", "2.0.1", "2.0.2", "2.1", "2.1.1", "2.2", "2.2.1", //checked
                                                         "3.0", "3.0.1", "3.1", "3.1.1", "3.1.2", "3.1.3", "3.2", "3.2.1", "3.2.2", //checked
                                                         "4.0", "4.0.1", "4.0.2", "4.1", "4.2", "4.2.1", "4.2.6", "4.2.7", "4.2.8", "4.2.9", "4.2.10", "4.3", "4.3.1", "4.3.2", "4.3.3", "4.3.4", "4.3.5", "4.4", "4.5", "4.6", //checked
                                                         "5.0", "5.0.1", "5.1", "5.1.1", //checked
                                                         "6.0", "6.0.1", "6.0.2", "6.1", "6.1.1", "6.1.2", "6.1.3", "6.1.4", "6.1.5", "6.1.6",//checked
                                                         "7.0", "7.0.1", "7.0.2", "7.0.3", "7.0.4", "7.0.5", "7.0.6", "7.1", "7.1.1", "7.1.2",//checked
                                                         "8.0", "8.0.1", "8.0.2", "8.1", "8.1.1", "8.1.2", "8.1.3", "8.2", "8.3", "8.4", "8.4.1", "8.4.2","8.4.3", //checked
                                                         "9.0", "9.0.1", "9.0.2", "9.1", "9.1.1", "9.2", "9.2.1", "9.2.2", "9.3", "9.3.1", "9.3.2", "9.3.3", "9.3.4", "9.3.5", "9.3.6",//checked
                                                         "10.0", "10.0.1", "10.0.2", "10.0.3", "10.1", "10.1.1", "10.2", "10.2.1", "10.2.2", "10.3", "10.3.1", "10.3.2", "10.3.3", "10.3.4",
                                                         "11.0", "11.0.1", "11.0.2", "11.0.3", "11.1", "11.1.1", "11.1.2", "11.2", "11.2.1", "11.2.2", "11.2.5", "11.2.6", "11.3", "11.3.1", "11.4", "11.4.1",
                                                         "12.0", "12.0.1", "12.1", "12.1.1", "12.1.2", "12.1.3", "12.1.4", "12.2", "12.2.1", "12.3", "12.3.1", "12.3.2", "12.4", "12.4.1", "12.4.2", "12.4.3", "12.4.4", "12.4.5", "12.4.6", "12.4.7", "12.4.8", "12.4.9", "12.5","12.5.1","12.5.2","12.5.3","12.5.4","12.5.5","12.5.6","12.5.7",
                                                         "13.0", "13.1", "13.1.1", "13.1.2", "13.1.3", "13.2", "13.2.2", "13.2.3", "13.3", "13.3.1", "13.4", "13.4.1", "13.4.6", "13.4.8", "13.5", "13.5.1", "13.6", "13.6.1","13.7",
                                                         "14.0","14.0.1","14.1","14.2","14.2.1","14.3", "14.4", "14.4.1", "14.4.2","14.5","14.5.1", "14.6","14.7", "14.7.1","14.8",
                                                         "15.0","15.0.1","15.0.2","15.1","15.1.1","15.2","15.2.1","15.3","15.3.1","15.4","15.4.1","15.5","15.6","15.6.1","15.7","15.7.1","15.7.2","15.7.3","15.7.4","15.7.5","15.7.6","15.7.7","15.7.8","15.7.9",
                                                         "16.0","16.0.1","16.0.2","16.0.3","16.1","16.1.1","16.1.2","16.2","16.3","16.3.1","16.4","16.4.1","16.5","16.5.1","16.6","16.6.1","16.7","16.7.1","16.7.2",
                                                         "17.0","17.0.1","17.0.2","17.0.3","17.1"
                     };

                string[] OTAVersion = new string[] { "2.0", "2.0.1", "2.1", "2.2", "2.2.1", "2.2.2",
                                                         "3.0",  "3.1", "3.1.1", "3.1.3", "3.2", "3.2.2", "3.2.3",
                                                         "4.0", "4.0.1",  "4.1", "4.2", "4.2.2", "4.2.3", "4.3", "4.3.1", "4.3.2",
                                                         "5.0", "5.0.1", "5.1", "5.1.1", "5.1.2", "5.1.3", "5.2", "5.2.1", "5.3", "5.3.1", "5.3.2", "5.3.3", "5.3.4", "5.3.5", "5.3.6", "5.3.7", "5.3.9",
                                                         "6.0", "6.0.1", "6.1", "6.1.1",  "6.1.3", "6.1.4", "6.1.5", "6.1.6", "6.3",
                                                         "7.0", "7.0.1", "7.0.2", "7.0.3", "7.0.4", "7.0.5", "7.0.6", "7.1", "7.1.1", "7.1.2",
                                                         "8.0", "8.0.1", "8.0.2", "8.1", "8.1.1", "8.1.2", "8.1.3", "8.2", "8.2.1", "8.3", "8.4", "8.4.1", "8.4.2", "8.4.3", "8.4.4",
                                                         "9.0", "9.0.1", "9.0.2", "9.1", "9.2", "9.2.1", "9.2.2", "9.3", "9.3.1", "9.3.2", "9.3.3", "9.3.4", "9.3.5", "9.3.6",
                                                         "10.0", "10.0.1", "10.0.2", "10.0.3", "10.1", "10.1.1", "10.2", "10.2.1", "10.2.2", "10.3", "10.3.1", "10.3.2", "10.3.3", "10.3.4", "9.9.10.0.1", "9.9.10.0.3", "9.9.10.1", "9.9.10.1.1", "9.9.10.2", "9.9.10.2.1", "9.9.10.3", "9.9.10.3.1", "9.9.10.3.2", "9.9.10.3.3", "9.9.10.3.4",
                                                         "11.0", "11.0.1", "11.0.2", "11.0.3", "11.1", "11.1.1", "11.1.2", "11.2", "11.2.1", "11.2.2", "11.2.5", "11.2.6", "11.3", "11.3.1", "11.4", "11.4.1", "9.9.11.0", "9.9.11.0.1", "9.9.11.0.3", "9.9.11.1", "9.9.11.1.1", "9.9.11.2", "9.9.11.2.1", "9.9.11.2.2", "9.9.11.2.5", "9.9.11.2.6", "9.9.11.3", "9.9.11.3.1", "9.9.11.4", "9.9.11.4.1",
                                                         "12.0", "12.0.1", "12.1", "12.1.1", "12.1.2", "12.1.3", "12.2", "12.2.1", "12.3", "12.3.1", "12.4", "12.4.1","12.4.7","9.9.12.0", "9.9.12.0.1", "9.9.12.1", "9.9.12.1.1", "9.9.12.1.2", "9.9.12.1.3","9.9.12.1.4", "9.9.12.2", "9.9.12.5",  "9.9.12.3", "9.9.12.3.1", "9.9.12.4", "9.9.12.4.1", "9.9.12.4.2", "9.9.12.4.3", "9.9.12.4.4", "9.9.12.4.5", "9.9.12.4.6","9.9.12.4.8", "9.9.12.4.9", "9.9.12.5", "9.9.12.5.1","9.9.12.5.2","9.9.12.5.3","9.9.12.5.4","9.9.12.5",
                                                         "13.0", "13.1", "13.2", "13.3", "13.3.1", "13.4", "13.4.1", "13.4.5", "13.4.6", "13.4.8", "13.5", "13.6", "9.9.13.0", "9.9.13.1", "9.9.13.1.1", "9.9.13.1.2", "9.9.13.1.3", "9.9.13.2", "9.9.13.2.1", "9.9.13.2.2", "9.9.13.2.3", "9.9.13.3", "9.9.13.3.1", "9.9.13.4", "9.9.13.4.1","9.9.13.4.5", "9.9.13.4.6", "9.9.13.5", "9.9.13.5.1", "9.9.13.5.5", "9.9.13.6", "9.9.13.6.1", "9.9.13.7",
                                                         "14.0","9.9.14.0","14.0.1","9.9.14.0.1","14.0.2","9.9.14.1","9.9.14.2","9.9.14.2.1","9.9.14.3", "9.9.14.4", "9.9.14.4.1", "9.9.14.4.2","9.9.14.5","9.9.14.5.1","9.9.14.6","9.9.14.7","9.9.14.7.1","9.9.14.8","9.9.14.8.1",
                                                         "9.9.15.0","9.9.15.0.1","9.9.15.0.2","9.9.15.1","9.9.15.1.1","9.9.15.2"
                     };

                for (int x = 0; x < iOSVersion.Length; x++)
                {
                    ddlVersion.Items.Add(new ListItem(iOSVersion[x], iOSVersion[x]));
                }

                for (int y = 0; y < OTAVersion.Length; y++)
                {
                    ddlVersionOTA.Items.Add(new ListItem(OTAVersion[y], OTAVersion[y]));
                }
            }
        }

        protected void rblOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDownloadAll.Visible = false; 
            tbData.Visible = false;
            tbData.Text = "";
            if (rblOptions.SelectedItem.ToString().Equals("Official"))
            {
                lblStep2.Visible = true;
                ddliPhone.Visible = true;
                ddliPad.Visible = true;
                ddliPod.Visible = true;
                ddlWatch.Visible = false;
                ddlAppleTV.Visible = false;
                ddlMac.Visible = true;
                ddlAudioAccessory.Visible = false;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = false;
            }
            else if (rblOptions.SelectedItem.ToString().Equals("OTA"))
            {
                lblStep2.Visible = true;
                ddliPhone.Visible = true;
                ddliPad.Visible = true;
                ddliPod.Visible = true;
                ddlWatch.Visible = true;
                ddlAppleTV.Visible = true;
                ddlMac.Visible = false;
                ddlAudioAccessory.Visible = true;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = false;
            }
            else if (rblOptions.SelectedItem.ToString().Equals("Version"))
            {
                lblStep2.Visible = true;
                ddliPhone.Visible = false;
                ddliPad.Visible = false;
                ddliPod.Visible = false;
                ddlWatch.Visible = false;
                ddlAppleTV.Visible = false;
                ddlMac.Visible = false;
                ddlAudioAccessory.Visible = false;
                ddlVersion.Visible = true;
                ddlVersionOTA.Visible = false;
            }
            else if (rblOptions.SelectedItem.ToString().Equals("Version (OTA)"))
            {
                lblStep2.Visible = true;
                ddliPhone.Visible = false;
                ddliPad.Visible = false;
                ddliPod.Visible = false;
                ddlWatch.Visible = false;
                ddlAppleTV.Visible = false;
                ddlMac.Visible = false;
                ddlAudioAccessory.Visible = false;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = true;
            }

        }

        protected void ddliPhone_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbData.Visible = false;
            tbData.Text = "";
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
                ddlMac.Visible = true;
                ddlAudioAccessory.Visible = false;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = false;
            }
            else if (rblOptions.SelectedItem.ToString().Equals("OTA"))
            {
                lblStep2.Visible = true;
                ddliPhone.Visible = true;
                ddliPad.Visible = true;
                ddliPod.Visible = true;
                ddlWatch.Visible = true;
                ddlAppleTV.Visible = true;
                ddlMac.Visible = false;
                ddlAudioAccessory.Visible = true;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = false;
            }

            ddliPad.SelectedIndex = 0;
            ddliPod.SelectedIndex = 0;
            ddlWatch.SelectedIndex = 0;
            ddlAppleTV.SelectedIndex = 0;
            ddlMac.SelectedIndex = 0;
            ddlAudioAccessory.SelectedIndex = 0;
        }

        protected void ddliPad_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbData.Visible = false;
            tbData.Text = "";
            ddliPhone.SelectedIndex = 0;
            ddliPod.SelectedIndex = 0;
            ddlWatch.SelectedIndex = 0;
            ddlAppleTV.SelectedIndex = 0;
            ddlMac.SelectedIndex = 0;
            ddlAudioAccessory.SelectedIndex = 0;

            lblSelection.Text = ddliPad.SelectedValue.ToString();
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected <b>" + rblOptions.SelectedItem.ToString() + " " + ddliPad.SelectedItem.ToString() + "</b> with the identifier: ";
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
                ddlMac.Visible = true;
                ddlAudioAccessory.Visible = false;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = false;
            }
            else if (rblOptions.SelectedItem.ToString().Equals("OTA"))
            {
                lblStep2.Visible = true;
                ddliPhone.Visible = true;
                ddliPad.Visible = true;
                ddliPod.Visible = true;
                ddlWatch.Visible = true;
                ddlAppleTV.Visible = true;
                ddlMac.Visible = false;
                ddlAudioAccessory.Visible = true;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = false;
            }


        }

        protected void ddliPod_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbData.Visible = false;
            tbData.Text = "";
            ddliPhone.SelectedIndex = 0;
            ddliPad.SelectedIndex = 0;
            ddlWatch.SelectedIndex = 0;
            ddlAppleTV.SelectedIndex = 0;
            ddlMac.SelectedIndex = 0;
            ddlAudioAccessory.SelectedIndex = 0;

            lblSelection.Text = ddliPod.SelectedValue.ToString();
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected <b>" + rblOptions.SelectedItem.ToString() + " " + ddliPod.SelectedItem.ToString() + "</b> with the identifier: ";
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
                ddlMac.Visible = true;
                ddlAudioAccessory.Visible = false;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = false;
            }
            else if (rblOptions.SelectedItem.ToString().Equals("OTA"))
            {
                lblStep2.Visible = true;
                ddliPhone.Visible = true;
                ddliPad.Visible = true;
                ddliPod.Visible = true;
                ddlWatch.Visible = true;
                ddlAppleTV.Visible = true;
                ddlMac.Visible = false;
                ddlAudioAccessory.Visible = true;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = false;
            }



        }

        protected void ddlWatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbData.Visible = false;
            tbData.Text = "";
            ddliPhone.SelectedIndex = 0;
            ddliPad.SelectedIndex = 0;
            ddliPod.SelectedIndex = 0;
            ddlAppleTV.SelectedIndex = 0;
            ddlMac.SelectedIndex = 0;
            ddlAudioAccessory.SelectedIndex = 0;

            lblSelection.Text = ddlWatch.SelectedValue.ToString();
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected <b>" + rblOptions.SelectedItem.ToString() + " " + ddlWatch.SelectedItem.ToString() + "</b> with the identifier: ";
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
                ddlMac.Visible = true;
                ddlAudioAccessory.Visible = false;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = false;
            }
            else if (rblOptions.SelectedItem.ToString().Equals("OTA"))
            {
                lblStep2.Visible = true;
                ddliPhone.Visible = true;
                ddliPad.Visible = true;
                ddliPod.Visible = true;
                ddlWatch.Visible = true;
                ddlAppleTV.Visible = true;
                ddlMac.Visible = false;
                ddlAudioAccessory.Visible = true;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = false;
            }

        }

        protected void ddlAudioAccessory_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbData.Visible = false;
            tbData.Text = "";
            ddliPhone.SelectedIndex = 0;
            ddliPad.SelectedIndex = 0;
            ddliPod.SelectedIndex = 0;
            ddlWatch.SelectedIndex = 0;
            ddlMac.SelectedIndex = 0;
            ddlAppleTV.SelectedIndex = 0;

            lblSelection.Text = ddlAudioAccessory.SelectedValue.ToString();
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected <b>" + rblOptions.SelectedItem.ToString() + " " + ddlAudioAccessory.SelectedItem.ToString() + "</b> with the identifier: ";
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
                ddlMac.Visible = true;
                ddlAudioAccessory.Visible = false;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = false;
            }
            else if (rblOptions.SelectedItem.ToString().Equals("OTA"))
            {
                lblStep2.Visible = true;
                ddliPhone.Visible = true;
                ddliPad.Visible = true;
                ddliPod.Visible = true;
                ddlWatch.Visible = true;
                ddlAppleTV.Visible = true;
                ddlMac.Visible = false;
                ddlAudioAccessory.Visible = true;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = false;
            }


        }

        protected void ddlAppleTV_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbData.Visible = false;
            tbData.Text = "";
            ddliPhone.SelectedIndex = 0;
            ddliPad.SelectedIndex = 0;
            ddliPod.SelectedIndex = 0;
            ddlWatch.SelectedIndex = 0;
            ddlMac.SelectedIndex = 0;
            ddlAudioAccessory.SelectedIndex = 0;

            lblSelection.Text = ddlAppleTV.SelectedValue.ToString();
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected <b>" + rblOptions.SelectedItem.ToString() + " " + ddlAppleTV.SelectedItem.ToString() + "</b> with the identifier: ";
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
                ddlMac.Visible = true;
                ddlAudioAccessory.Visible = false;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = false;
            }
            else if (rblOptions.SelectedItem.ToString().Equals("OTA"))
            {
                lblStep2.Visible = true;
                ddliPhone.Visible = true;
                ddliPad.Visible = true;
                ddliPod.Visible = true;
                ddlWatch.Visible = true;
                ddlAppleTV.Visible = true;
                ddlMac.Visible = false;
                ddlAudioAccessory.Visible = true;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = false;
            }
        }

        protected void ddlMac_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddliPhone.SelectedIndex = 0;
            ddliPad.SelectedIndex = 0;
            ddliPod.SelectedIndex = 0;
            ddlWatch.SelectedIndex = 0;
            ddlAppleTV.SelectedIndex = 0;
            ddlAudioAccessory.SelectedIndex = 0;

            lblSelection.Text = ddlMac.SelectedValue.ToString();
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected <b>" + rblOptions.SelectedItem.ToString() + " " + ddlMac.SelectedItem.ToString() + "</b> with the identifier: ";
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
                ddlMac.Visible = true;
                ddlAudioAccessory.Visible = false;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = false;
            }
            else if (rblOptions.SelectedItem.ToString().Equals("OTA"))
            {
                lblStep2.Visible = true;
                ddliPhone.Visible = true;
                ddliPad.Visible = true;
                ddliPod.Visible = true;
                ddlWatch.Visible = true;
                ddlAppleTV.Visible = true;
                ddlMac.Visible = false;
                ddlAudioAccessory.Visible = true;
                ddlVersion.Visible = false;
                ddlVersionOTA.Visible = false;
            }
        }

        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            if (rblOptions.SelectedItem.Value.Equals("Version"))
            {
                string version = ddlVersion.SelectedItem.ToString();
                string versionJSON = "";

                WebClient webClientVersion = new WebClient();
                versionJSON = webClientVersion.DownloadString("https://api.ipsw.me/v4/ipsw/" + version);
                dynamic jsonVersionObj = JsonConvert.DeserializeObject(versionJSON);
                double fileSizeGB = 0.00;
                double originalFileSize = 0.00;
                // Create a dictionary to store url and file size
                Dictionary<string, string> urlArray = new Dictionary<string, string>();
                for (int i = 0; i < jsonVersionObj.Count; i++)
                {
                    string url = jsonVersionObj[i]["url"];
                    string fileSize = jsonVersionObj[i]["filesize"];
                    // Store in dictionary. This will automatically handle duplicate urls.
                    if (!urlArray.ContainsKey(url))
                    {
                        urlArray.Add(url, fileSize);
                    }
                   
                }

                foreach (KeyValuePair<string, string> item in urlArray)
                {

                    string[] urlFullTitle = item.Key.ToString().Split('/');
                    string urlTitle = urlFullTitle[urlFullTitle.Length - 1];

                    tbData.Text += "<a href=\"" + item.Key.ToString() + "\" target=\"_blank\">" + urlTitle + "</a><br/>";
                    listOfLinks.Text += item.Key.ToString()+ ";";
                    originalFileSize += Double.Parse(item.Value);
                }

                fileSizeGB = originalFileSize / 1024 / 1024 / 1024;
                lblSelectionComment.Text = "<br/>There are " + urlArray.Count +
                                           " Files<br/>The Total File Size are " + fileSizeGB.ToString("0.##") + " GB";
                
            }

            else if (rblOptions.SelectedItem.Value.Equals("Version (OTA)"))
            {
                string versionOTA = ddlVersionOTA.SelectedItem.ToString();
                string versionOTAJSON = "";

                WebClient webClientVersionOTA = new WebClient();
                versionOTAJSON = webClientVersionOTA.DownloadString("https://api.ipsw.me/v4/ota/" + versionOTA);
                dynamic jsonVersionOTAObj = JsonConvert.DeserializeObject(versionOTAJSON);
                double fileSizeGB = 0.00;
                double originalFileSize = 0.00;
                ArrayList otaLinks = new ArrayList();
                ArrayList otaFS = new ArrayList();
                for (int i = 0; i < jsonVersionOTAObj.Count; i++)
                {
                    if (!otaLinks.Contains(jsonVersionOTAObj[i]["url"]))
                    {
                        otaLinks.Add(jsonVersionOTAObj[i]["url"]);
                        otaFS.Add(jsonVersionOTAObj[i]["filesize"]);
                    }
                }

                for (int j = 0; j < otaLinks.Count; j++)
                {
                    string url = otaLinks[j].ToString();
                    string fileSize = otaFS[j].ToString();

                    tbData.Text += url + "<br/>";
                    originalFileSize += Double.Parse(fileSize);
                }

                fileSizeGB = originalFileSize / 1024 / 1024 / 1024;
                lblSelectionComment.Text = "<br/>There are " + otaLinks.Count +
                                           " Files<br/>The Total File Size are " + fileSizeGB.ToString("0.##") + " GB";
            }

            else
            {

                string identifier = lblSelection.Text;
                string firmwareType = rblOptions.SelectedItem.ToString();
                string myJSON = "";

                WebClient webClient = new WebClient();
                if (firmwareType.Equals("Official"))
                {
                    myJSON = webClient.DownloadString("https://api.ipsw.me/v4/device/" + identifier + "?type=ipsw");
                }
                else if (firmwareType.Equals("OTA"))
                {
                    myJSON = webClient.DownloadString("https://api.ipsw.me/v4/device/" + identifier + "?type=ota");
                }

                dynamic jsonObj = JsonConvert.DeserializeObject(myJSON);
                double fileSizeGB = 0.00;
                double originalFileSize = 0.00;
                ArrayList otaLinks = new ArrayList();
                ArrayList otaFS = new ArrayList();
                for (int i = 0; i < jsonObj["firmwares"].Count; i++)
                {
                    otaLinks.Add(jsonObj["firmwares"][i]["url"]);
                    otaFS.Add(jsonObj["firmwares"][i]["filesize"]);

                }

                for (int j = 0; j < otaLinks.Count; j++)
                {
                    string url = otaLinks[j].ToString();
                    string fileSize = otaFS[j].ToString();

                    tbData.Text += url + "<br/>";
                   
                    originalFileSize += Double.Parse(fileSize);
                }

                fileSizeGB = originalFileSize / 1024 / 1024 / 1024;
                lblSelectionComment.Text = "<br/>There are " + otaLinks.Count +
                                           " Files<br/>The Total File Size are " + fileSizeGB.ToString("0.##") + " GB";
            }
            tbData.Visible = true;
            btnDownloadAll.Visible = true;
        }

        protected void ddlVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbData.Visible = false;
            tbData.Text = "";
            lblSelection.Text = ddlVersion.SelectedValue.ToString();
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected iOS <b>" + rblOptions.SelectedItem.ToString() + " " + ddlVersion.SelectedItem.ToString() + "</b>";
            btnRetrieve.Text = "Retrieve";
            btnRetrieve.Visible = true;

            lblStep3.Visible = true;
            lblStep3.Font.Bold = true;

            lblStep2.Visible = true;
            ddliPhone.Visible = false;
            ddliPad.Visible = false;
            ddliPod.Visible = false;
            ddlWatch.Visible = false;
            ddlAppleTV.Visible = false;
            ddlMac.Visible = false;
            ddlAudioAccessory.Visible = false;
            ddlVersion.Visible = true;
            ddlVersionOTA.Visible = false;
        }

        protected void ddlVersionOTA_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbData.Visible = false;
            tbData.Text = "";
            lblSelection.Text = ddlVersionOTA.SelectedValue.ToString();
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected OTA <b>" + rblOptions.SelectedItem.ToString() + " " + ddlVersionOTA.SelectedItem.ToString();
            btnRetrieve.Text = "Retrieve";
            btnRetrieve.Visible = true;

            lblStep3.Visible = true;
            lblStep3.Font.Bold = true;

            lblStep2.Visible = true;
            ddliPhone.Visible = false;
            ddliPad.Visible = false;
            ddliPod.Visible = false;
            ddlWatch.Visible = false;
            ddlAppleTV.Visible = false;
            ddlMac.Visible = false;
            ddlAudioAccessory.Visible = false;
            ddlVersion.Visible = false;
            ddlVersionOTA.Visible = true;
        }

    }
}