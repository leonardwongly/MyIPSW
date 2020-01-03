using Newtonsoft.Json;
using System;
using System.Net;
using System.Web.UI.WebControls;

namespace MyIPSWMinimal
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

            if (!IsPostBack)
            {
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

            ddliPad.SelectedIndex = 0;
            ddliPod.SelectedIndex = 0;
            ddlWatch.SelectedIndex = 0;
            ddlAppleTV.SelectedIndex = 0;
            ddlAudioAccessory.SelectedIndex = 0;
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

        protected void ddliPad_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddliPhone.SelectedIndex = 0;
            ddliPod.SelectedIndex = 0;
            ddlWatch.SelectedIndex = 0;
            ddlAppleTV.SelectedIndex = 0;
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

        protected void ddliPod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddliPhone.SelectedIndex = 0;
            ddliPad.SelectedIndex = 0;
            ddlWatch.SelectedIndex = 0;
            ddlAppleTV.SelectedIndex = 0;
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

        protected void ddlWatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddliPhone.SelectedIndex = 0;
            ddliPad.SelectedIndex = 0;
            ddliPod.SelectedIndex = 0;
            ddlAppleTV.SelectedIndex = 0;
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

        protected void ddlAudioAccessory_SelectedIndexChanged(object sender, EventArgs e)
        {

            ddliPhone.SelectedIndex = 0;
            ddliPad.SelectedIndex = 0;
            ddliPod.SelectedIndex = 0;
            ddlWatch.SelectedIndex = 0;
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

        protected void ddlAppleTV_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddliPhone.SelectedIndex = 0;
            ddliPad.SelectedIndex = 0;
            ddliPod.SelectedIndex = 0;
            ddlWatch.SelectedIndex = 0;
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

        protected void btnRetrieve_Click(object sender, EventArgs e)
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
            TableHeaderRow thr = new TableHeaderRow();
            TableHeaderCell headerTableCell0 = new TableHeaderCell();
            TableHeaderCell headerTableCell2 = new TableHeaderCell();
            TableHeaderCell headerTableCell3 = new TableHeaderCell();

            headerTableCell0.Text = "Build ID";
            headerTableCell2.Text = "Links";
            headerTableCell3.Text = "Release Date & Time";

            thr.Cells.Add(headerTableCell0);
            thr.Cells.Add(headerTableCell2);
            thr.Cells.Add(headerTableCell3);
            tblData.Rows.AddAt(0, thr);
            for (int i = 0; i < jsonObj["firmwares"].Count; i++)
            {
                string buildid = jsonObj["firmwares"][i]["buildid"].ToString();
                string url = jsonObj["firmwares"][i]["url"].ToString();
                string dateReleased = jsonObj["firmwares"][i]["releasedate"].ToString();


                HyperLink hypName = new HyperLink();
                HyperLink hyp = new HyperLink();
                HyperLink hypDateReleased = new HyperLink();

                hypName.ID = "hypName" + i;
                hypName.Text = buildid + "<br/>";

                hyp.ID = "hypABD" + i;
                hyp.NavigateUrl = url;

                string[] links = url.Split('/');
                int linkInt = links.Length - 1;
                hyp.Text = links[linkInt] + "<br/>";

                hypDateReleased.ID = "hypDateReleased" + i;
                hypDateReleased.Text = dateReleased.ToString();

                tblData.BorderStyle = BorderStyle.Solid;

                TableRow tr = new TableRow();
                TableCell tdName = new TableCell();
                TableCell tdLinks = new TableCell();
                TableCell tdDateReleased = new TableCell();

                tdName.Controls.Add(hypName);
                tdLinks.Controls.Add(hyp);
                tdDateReleased.Controls.Add(hypDateReleased);

                tr.Cells.Add(tdName);
                tr.Cells.Add(tdLinks); ;
                tr.Cells.Add(tdDateReleased);
                tblData.Rows.Add(tr);
            }
        }
    }
}