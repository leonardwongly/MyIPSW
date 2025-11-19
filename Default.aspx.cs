using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;

namespace ipsw
{
    public partial class Default : System.Web.UI.Page
    {
        private static readonly HttpClient client = new HttpClient();

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
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

                    string myJSON = await client.GetStringAsync("https://api.ipsw.me/v4/devices");

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

                    for (int x = 0; x < VersionData.iOSVersions.Length; x++)
                    {
                        ddlVersion.Items.Add(new ListItem(VersionData.iOSVersions[x], VersionData.iOSVersions[x]));
                    }

                    for (int y = 0; y < VersionData.OTAVersions.Length; y++)
                    {
                        ddlVersionOTA.Items.Add(new ListItem(VersionData.OTAVersions[y], VersionData.OTAVersions[y]));
                    }
                }
                catch (Exception ex)
                {
                    Response.Redirect("~/Error.aspx");
                }
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
            lblSelection.Text = ddliPhone.SelectedValue.ToString();
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected <b>" + HttpUtility.HtmlEncode(rblOptions.SelectedItem.ToString()) + " " + HttpUtility.HtmlEncode(ddliPhone.SelectedItem.ToString()) + "</b> with the identifier: ";
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
            ddliPhone.SelectedIndex = 0;
            ddliPod.SelectedIndex = 0;
            ddlWatch.SelectedIndex = 0;
            ddlAppleTV.SelectedIndex = 0;
            ddlMac.SelectedIndex = 0;
            ddlAudioAccessory.SelectedIndex = 0;

            lblSelection.Text = ddliPad.SelectedValue.ToString();
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected <b>" + HttpUtility.HtmlEncode(rblOptions.SelectedItem.ToString()) + " " + HttpUtility.HtmlEncode(ddliPad.SelectedItem.ToString()) + "</b> with the identifier: ";
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
            ddliPhone.SelectedIndex = 0;
            ddliPad.SelectedIndex = 0;
            ddlWatch.SelectedIndex = 0;
            ddlAppleTV.SelectedIndex = 0;
            ddlMac.SelectedIndex = 0;
            ddlAudioAccessory.SelectedIndex = 0;

            lblSelection.Text = ddliPod.SelectedValue.ToString();
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected <b>" + HttpUtility.HtmlEncode(rblOptions.SelectedItem.ToString()) + " " + HttpUtility.HtmlEncode(ddliPod.SelectedItem.ToString()) + "</b> with the identifier: ";
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
            ddliPhone.SelectedIndex = 0;
            ddliPad.SelectedIndex = 0;
            ddliPod.SelectedIndex = 0;
            ddlAppleTV.SelectedIndex = 0;
            ddlMac.SelectedIndex = 0;
            ddlAudioAccessory.SelectedIndex = 0;

            lblSelection.Text = ddlWatch.SelectedValue.ToString();
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected <b>" + HttpUtility.HtmlEncode(rblOptions.SelectedItem.ToString()) + " " + HttpUtility.HtmlEncode(ddlWatch.SelectedItem.ToString()) + "</b> with the identifier: ";
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

            ddliPhone.SelectedIndex = 0;
            ddliPad.SelectedIndex = 0;
            ddliPod.SelectedIndex = 0;
            ddlWatch.SelectedIndex = 0;
            ddlMac.SelectedIndex = 0;
            ddlAppleTV.SelectedIndex = 0;

            lblSelection.Text = ddlAudioAccessory.SelectedValue.ToString();
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected <b>" + HttpUtility.HtmlEncode(rblOptions.SelectedItem.ToString()) + " " + HttpUtility.HtmlEncode(ddlAudioAccessory.SelectedItem.ToString()) + "</b> with the identifier: ";
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
            ddliPhone.SelectedIndex = 0;
            ddliPad.SelectedIndex = 0;
            ddliPod.SelectedIndex = 0;
            ddlWatch.SelectedIndex = 0;
            ddlMac.SelectedIndex = 0;
            ddlAudioAccessory.SelectedIndex = 0;

            lblSelection.Text = ddlAppleTV.SelectedValue.ToString();
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected <b>" + HttpUtility.HtmlEncode(rblOptions.SelectedItem.ToString()) + " " + HttpUtility.HtmlEncode(ddlAppleTV.SelectedItem.ToString()) + "</b> with the identifier: ";
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

            lblSelectionComment.Text = "You have selected <b>" + HttpUtility.HtmlEncode(rblOptions.SelectedItem.ToString()) + " " + HttpUtility.HtmlEncode(ddlMac.SelectedItem.ToString()) + "</b> with the identifier: ";
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

        protected async void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                if (rblOptions.SelectedItem.Value.Equals("Version"))
                {
                    string version = ddlVersion.SelectedItem.ToString();
                    string versionJSON = await client.GetStringAsync("https://api.ipsw.me/v4/ipsw/" + version);
                    dynamic jsonVersionObj = JsonConvert.DeserializeObject(versionJSON);
                    TableHeaderRow thrVersion = new TableHeaderRow();
                    TableHeaderCell tableHeaderIdentifier = new TableHeaderCell();
                    TableHeaderCell tableHeaderBuildID = new TableHeaderCell();
                    TableHeaderCell tableHeaderURL = new TableHeaderCell();
                    TableHeaderCell tableHeaderFileSize = new TableHeaderCell();
                    TableHeaderCell tableHeaderReleasedDate = new TableHeaderCell();
                    TableHeaderCell tableHeaderSigned = new TableHeaderCell();

                    tableHeaderIdentifier.Text = "Identifier";
                    tableHeaderBuildID.Text = "Build ID";
                    tableHeaderURL.Text = "Download Links";
                    tableHeaderFileSize.Text = "File Size";
                    tableHeaderReleasedDate.Text = "Released Date";
                    tableHeaderSigned.Text = "Signed by Apple";

                    thrVersion.Cells.Add(tableHeaderIdentifier);
                    thrVersion.Cells.Add(tableHeaderBuildID);
                    thrVersion.Cells.Add(tableHeaderURL);
                    thrVersion.Cells.Add(tableHeaderFileSize);
                    thrVersion.Cells.Add(tableHeaderReleasedDate);
                    thrVersion.Cells.Add(tableHeaderSigned);
                    tblData.Rows.AddAt(0, thrVersion);
                    lblSelectionComment.Text += "<br/>There are " + jsonVersionObj.Count + " Files";
                    
     
                    for (int i = 0; i < jsonVersionObj.Count; i++)
                    {
                        string identifier = jsonVersionObj[i]["identifier"];
                        string buildID = jsonVersionObj[i]["buildid"];
                        string url = jsonVersionObj[i]["url"];
                        string fileSize = jsonVersionObj[i]["filesize"];
                        string releaseDate = jsonVersionObj[i]["releasedate"];
                        string signed = jsonVersionObj[i]["signed"];

                        HyperLink hyperIdentifier = new HyperLink();
                        HyperLink hyperBuildID = new HyperLink();
                        HyperLink hyperURL = new HyperLink();
                        HyperLink hyperFileSize = new HyperLink();
                        HyperLink hyperReleaseDate = new HyperLink();
                        HyperLink hyperSigned = new HyperLink();

                        hyperIdentifier.ID = "hypIdentifier" + i;
                        hyperBuildID.ID = "hyperBuildID" + i;
                        hyperURL.ID = "hyperURL" + i;
                        hyperFileSize.ID = "hyperFileSize" + i;
                        hyperReleaseDate.ID = "hyperReleaseDate" + i;
                        hyperSigned.ID = "hyperSigned" + i;


                        hyperIdentifier.Text = HttpUtility.HtmlEncode(identifier) + "<br/>";
                        hyperBuildID.Text = HttpUtility.HtmlEncode(buildID) + "<br/>";
                        hyperURL.Text = HttpUtility.HtmlEncode(url) + "<br/>";
                        double fileSizeGB = Double.Parse(fileSize) / 1024 / 1024 / 1024;
                        hyperFileSize.Text = fileSizeGB.ToString("0.##") + " GB";
                        if (!(releaseDate is null))
                        {
                            hyperReleaseDate.Text = HttpUtility.HtmlEncode(releaseDate);
                        }
                        else
                        {
                            hyperReleaseDate.Text = "-";
                        }


                        hyperURL.NavigateUrl = url;

                        string[] links = url.Split('/');
                        int linkInt = links.Length - 1;
                        hyperURL.Text = HttpUtility.HtmlEncode(links[linkInt]) + "<br/>";
                        if (signed == "True")
                        {
                            hyperSigned.Text = "Yes";
                            hyperSigned.Font.Bold = true;
                            hyperSigned.ForeColor = Color.Green;
                            hyperSigned.Font.Size = 14;
                        }
                        else
                        {
                            hyperSigned.Text = "No";
                            hyperSigned.Font.Bold = true;
                            hyperSigned.ForeColor = Color.Red;
                            hyperSigned.Font.Size = 14;
                        }

                        tblData.BorderStyle = BorderStyle.Solid;

                        TableRow tr = new TableRow();
                        TableCell tdIdentifier = new TableCell();
                        TableCell tdBuildID = new TableCell();
                        TableCell tdURL = new TableCell();
                        TableCell tdFileSize = new TableCell();
                        TableCell tdReleaseDate = new TableCell();
                        TableCell tdSigned = new TableCell();

                        tdIdentifier.Controls.Add(hyperIdentifier);
                        tdBuildID.Controls.Add(hyperBuildID);
                        tdURL.Controls.Add(hyperURL);
                        tdFileSize.Controls.Add(hyperFileSize);
                        tdReleaseDate.Controls.Add(hyperReleaseDate);
                        tdSigned.Controls.Add(hyperSigned);

                        tr.Cells.Add(tdIdentifier);
                        tr.Cells.Add(tdBuildID);
                        tr.Cells.Add(tdURL);
                        tr.Cells.Add(tdFileSize);
                        tr.Cells.Add(tdReleaseDate);
                        tr.Cells.Add(tdSigned);
                        tblData.Rows.Add(tr);
                    }
                }

                else if (rblOptions.SelectedItem.Value.Equals("Version (OTA)"))
                {
                    string versionOTA = ddlVersionOTA.SelectedItem.ToString();
                    string versionOTAJSON = await client.GetStringAsync("https://api.ipsw.me/v4/ota/" + versionOTA);
                    dynamic jsonVersionOTAObj = JsonConvert.DeserializeObject(versionOTAJSON);
                    TableHeaderRow thrVersionOTA = new TableHeaderRow();
                    TableHeaderCell tableHeaderIdentifierOTA = new TableHeaderCell();
                    TableHeaderCell tableHeaderBuildIDOTA = new TableHeaderCell();
                    TableHeaderCell tableHeaderURLOTA = new TableHeaderCell();
                    TableHeaderCell tableHeaderFileSizeOTA = new TableHeaderCell();
                    TableHeaderCell tableHeaderReleasedDateOTA = new TableHeaderCell();

                    tableHeaderIdentifierOTA.Text = "Identifier";
                    tableHeaderBuildIDOTA.Text = "Build ID";
                    tableHeaderURLOTA.Text = "Download Links";
                    tableHeaderFileSizeOTA.Text = "File Size";
                    tableHeaderReleasedDateOTA.Text = "Released Date";

                    thrVersionOTA.Cells.Add(tableHeaderIdentifierOTA);
                    thrVersionOTA.Cells.Add(tableHeaderBuildIDOTA);
                    thrVersionOTA.Cells.Add(tableHeaderURLOTA);
                    thrVersionOTA.Cells.Add(tableHeaderFileSizeOTA);
                    thrVersionOTA.Cells.Add(tableHeaderReleasedDateOTA);
                    tblData.Rows.AddAt(0, thrVersionOTA);
                    lblSelectionComment.Text += "<br/>There are " + jsonVersionOTAObj.Count + " Files";

                    for (int i = 0; i < jsonVersionOTAObj.Count; i++)
                    {
                        string identifier = jsonVersionOTAObj[i]["identifier"];
                        string buildID = jsonVersionOTAObj[i]["buildid"];
                        string url = jsonVersionOTAObj[i]["url"];
                        string fileSize = jsonVersionOTAObj[i]["filesize"];
                        string releaseDate = jsonVersionOTAObj[i]["releasedate"];
                        string signedOTA = jsonVersionOTAObj[i]["signed"];

                        HyperLink hyperIdentifierOTA = new HyperLink();
                        HyperLink hyperBuildIDOTA = new HyperLink();
                        HyperLink hyperURLOTA = new HyperLink();
                        HyperLink hyperFileSizeOTA = new HyperLink();
                        HyperLink hyperReleaseDateOTA = new HyperLink();
                        HyperLink hyperSignedOTA = new HyperLink();

                        hyperIdentifierOTA.ID = "hypIdentifier" + i;
                        hyperBuildIDOTA.ID = "hyperBuildID" + i;
                        hyperURLOTA.ID = "hyperURL" + i;
                        hyperFileSizeOTA.ID = "hyperFileSize" + i;
                        hyperReleaseDateOTA.ID = "hyperReleaseDate" + i;
                        hyperSignedOTA.ID = "hyperSigned" + i;

                        hyperIdentifierOTA.Text = HttpUtility.HtmlEncode(identifier) + "<br/>";
                        hyperBuildIDOTA.Text = HttpUtility.HtmlEncode(buildID) + "<br/>";
                        hyperURLOTA.Text = HttpUtility.HtmlEncode(url) + "<br/>";
                        double fileSizeGB = Double.Parse(fileSize) / 1024 / 1024 / 1024;
                        hyperFileSizeOTA.Text = fileSizeGB.ToString("0.##") + " GB";
                        if (!(releaseDate is null))
                        {
                            hyperReleaseDateOTA.Text = HttpUtility.HtmlEncode(releaseDate);
                        }
                        else
                        {
                            hyperReleaseDateOTA.Text = "-";
                        }


                        hyperURLOTA.NavigateUrl = url;

                        string[] links = url.Split('/');
                        int linkInt = links.Length - 1;
                        hyperURLOTA.Text = HttpUtility.HtmlEncode(links[linkInt]) + "<br/>";
                        if (signedOTA == "True")
                        {
                            hyperSignedOTA.Text = "Yes";
                            hyperSignedOTA.Font.Bold = true;
                            hyperSignedOTA.ForeColor = Color.Green;
                            hyperSignedOTA.Font.Size = 14;
                        }
                        else
                        {
                            hyperSignedOTA.Text = "No";
                            hyperSignedOTA.Font.Bold = true;
                            hyperSignedOTA.ForeColor = Color.Red;
                            hyperSignedOTA.Font.Size = 14;
                        }

                        tblData.BorderStyle = BorderStyle.Solid;

                        TableRow tr = new TableRow();
                        TableCell tdIdentifierOTA = new TableCell();
                        TableCell tdBuildIDOTA = new TableCell();
                        TableCell tdURLOTA = new TableCell();
                        TableCell tdFileSizeOTA = new TableCell();
                        TableCell tdReleaseDateOTA = new TableCell();
                        TableCell tdSignedOTA = new TableCell();

                        tdIdentifierOTA.Controls.Add(hyperIdentifierOTA);
                        tdBuildIDOTA.Controls.Add(hyperBuildIDOTA);
                        tdURLOTA.Controls.Add(hyperURLOTA);
                        tdFileSizeOTA.Controls.Add(hyperFileSizeOTA);
                        tdReleaseDateOTA.Controls.Add(hyperReleaseDateOTA);
                        tdSignedOTA.Controls.Add(hyperSignedOTA);

                        tr.Cells.Add(tdIdentifierOTA);
                        tr.Cells.Add(tdBuildIDOTA);
                        tr.Cells.Add(tdURLOTA);
                        tr.Cells.Add(tdFileSizeOTA);
                        tr.Cells.Add(tdReleaseDateOTA);
                        tr.Cells.Add(tdSignedOTA);
                        tblData.Rows.Add(tr);
                    }
                }

                else
                {

                    string identifier = lblSelection.Text;
                    string firmwareType = rblOptions.SelectedItem.ToString();
                    string myJSON = "";

                    if (firmwareType.Equals("Official"))
                    {
                        myJSON = await client.GetStringAsync("https://api.ipsw.me/v4/device/" + identifier + "?type=ipsw");
                    }
                    else if (firmwareType.Equals("OTA"))
                    {
                        myJSON = await client.GetStringAsync("https://api.ipsw.me/v4/device/" + identifier + "?type=ota");
                    }


                    dynamic jsonObj = JsonConvert.DeserializeObject(myJSON);

                    TableHeaderRow thr = new TableHeaderRow();
                    TableHeaderCell headerBuildID = new TableHeaderCell();
                    TableHeaderCell headerLinks = new TableHeaderCell();
                    TableHeaderCell headerReleaseDT = new TableHeaderCell();
                    TableHeaderCell headerFS = new TableHeaderCell();
                    TableHeaderCell headerSigned = new TableHeaderCell();

                    headerBuildID.Text = "Build ID";
                    headerLinks.Text = "Links";
                    headerReleaseDT.Text = "Release Date & Time";
                    headerFS.Text = "File Size";
                    headerSigned.Text = "Signed by Apple";

                    thr.Cells.Add(headerBuildID);
                    thr.Cells.Add(headerLinks);
                    thr.Cells.Add(headerFS);
                    thr.Cells.Add(headerReleaseDT);
                    thr.Cells.Add(headerSigned);
                    tblData.Rows.AddAt(0, thr);
                    lblSelectionComment.Text += "<br/>There are " + jsonObj["firmwares"].Count + " Files";

                    for (int i = 0; i < jsonObj["firmwares"].Count; i++)
                    {
                        string buildid = jsonObj["firmwares"][i]["buildid"].ToString();
                        string url = jsonObj["firmwares"][i]["url"].ToString();
                        string dateReleased = jsonObj["firmwares"][i]["releasedate"].ToString();
                        string fileSize = jsonObj["firmwares"][i]["filesize"].ToString();
                        string signed = jsonObj["firmwares"][i]["signed"].ToString();


                        HyperLink hypName = new HyperLink();
                        HyperLink hyp = new HyperLink();
                        HyperLink hypDateReleased = new HyperLink();
                        HyperLink hypFileSize = new HyperLink();
                        HyperLink hypSigned = new HyperLink();

                        hypName.ID = "hypName" + i;
                        hypName.Text = HttpUtility.HtmlEncode(buildid) + "<br/>";

                        hyp.ID = "hypABD" + i;
                        hyp.NavigateUrl = url;

                        hypFileSize.ID = "hypFileSize" + i;
                        double fileSizeGB = Double.Parse(fileSize) / 1024 / 1024 / 1024;
                        hypFileSize.Text = fileSizeGB.ToString("0.##") + " GB";


                        string[] links = url.Split('/');
                        int linkInt = links.Length - 1;
                        hyp.Text = HttpUtility.HtmlEncode(links[linkInt]) + "<br/>";

                        hypDateReleased.ID = "hypDateReleased" + i;
                        if (!dateReleased.Equals(""))
                        {
                            DateTime dr = Convert.ToDateTime(dateReleased);
                            hypDateReleased.Text = HttpUtility.HtmlEncode(dr.ToLongDateString().ToString() + " " + dr.ToLongTimeString().ToString());
                        }
                        else
                        {
                            hypDateReleased.Text = "-";
                        }

                        hypSigned.ID = "hyperSigned" + i;


                        if (signed == "True")
                        {
                            hypSigned.Text = "Yes";
                            hypSigned.Font.Bold = true;
                            hypSigned.ForeColor = Color.Green;
                            hypSigned.Font.Size = 14;
                        }
                        else
                        {
                            hypSigned.Text = "No";
                            hypSigned.Font.Bold = true;
                            hypSigned.ForeColor = Color.Red;
                            hypSigned.Font.Size = 14;
                        }

                        tblData.BorderStyle = BorderStyle.Solid;

                        TableRow tr = new TableRow();
                        TableCell tdName = new TableCell();
                        TableCell tdLinks = new TableCell();
                        TableCell tdHyperFileSize = new TableCell();
                        TableCell tdDateReleased = new TableCell();
                        TableCell tdSigned = new TableCell();

                        tdName.Controls.Add(hypName);
                        tdLinks.Controls.Add(hyp);
                        tdHyperFileSize.Controls.Add(hypFileSize);
                        tdDateReleased.Controls.Add(hypDateReleased);
                        tdSigned.Controls.Add(hypSigned);

                        tr.Cells.Add(tdName);
                        tr.Cells.Add(tdLinks); ;
                        tr.Cells.Add(tdHyperFileSize);
                        tr.Cells.Add(tdDateReleased);
                        tr.Cells.Add(tdSigned);
                        tblData.Rows.Add(tr);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/Error.aspx");
            }
        }

        protected void ddlVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblSelection.Text = ddlVersion.SelectedValue.ToString();
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected iOS <b>" + HttpUtility.HtmlEncode(rblOptions.SelectedItem.ToString()) + " " + HttpUtility.HtmlEncode(ddlVersion.SelectedItem.ToString()) + "</b>";
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
            lblSelection.Text = ddlVersionOTA.SelectedValue.ToString();
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected OTA <b>" + HttpUtility.HtmlEncode(rblOptions.SelectedItem.ToString()) + " " + HttpUtility.HtmlEncode(ddlVersionOTA.SelectedItem.ToString()) + "</b>";
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
