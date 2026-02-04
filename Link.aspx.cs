using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ipsw
{
    public partial class Link : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
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

                    string myJSON = await IpswApiCache.GetDevicesJsonAsync();

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

                    foreach (string v in VersionData.iOSVersions)
                    {
                        ddlVersion.Items.Add(new ListItem(v, v));
                    }

                    foreach (string v in VersionData.OTAVersions)
                    {
                        ddlVersionOTA.Items.Add(new ListItem(v, v));
                    }
                }
                catch (Exception)
                {
                    Response.Redirect("~/Error.aspx");
                }
            }
        }

        protected void rblOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDownloadAll.Visible = false; 
            tbData.Visible = false;
            tbData.Text = "";
            listOfLinks.Text = "";
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
            UpdateSelection(ddliPhone);
        }

        protected void ddliPad_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelection(ddliPad);
        }

        protected void ddliPod_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelection(ddliPod);
        }

        protected void ddlWatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelection(ddlWatch);
        }

        protected void ddlAudioAccessory_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelection(ddlAudioAccessory);
        }

        protected void ddlAppleTV_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelection(ddlAppleTV);
        }

        protected void ddlMac_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelection(ddlMac);
        }

        private void UpdateSelection(DropDownList ddl)
        {
            tbData.Visible = false;
            tbData.Text = "";
            listOfLinks.Text = "";
            
            // Reset other dropdowns
            if (ddl != ddliPhone) ddliPhone.SelectedIndex = 0;
            if (ddl != ddliPad) ddliPad.SelectedIndex = 0;
            if (ddl != ddliPod) ddliPod.SelectedIndex = 0;
            if (ddl != ddlWatch) ddlWatch.SelectedIndex = 0;
            if (ddl != ddlAppleTV) ddlAppleTV.SelectedIndex = 0;
            if (ddl != ddlMac) ddlMac.SelectedIndex = 0;
            if (ddl != ddlAudioAccessory) ddlAudioAccessory.SelectedIndex = 0;

            lblSelection.Text = HttpUtility.HtmlEncode(ddl.SelectedValue);
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected <b>" + HttpUtility.HtmlEncode(rblOptions.SelectedItem.ToString()) + " " + HttpUtility.HtmlEncode(ddl.SelectedItem.ToString()) + "</b> with the identifier: ";
            btnRetrieve.Text = "Retrieve";
            btnRetrieve.Visible = true;

            lblStep3.Visible = true;
            lblStep3.Font.Bold = true;

            // Logic to show/hide dropdowns based on rblOptions is repeated here in original, 
            // but it seems redundant if rblOptions_SelectedIndexChanged handles it. 
            // However, keeping original logic structure for safety, but simplified.
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
                tbData.Text = "";
                listOfLinks.Text = "";
                btnDownloadAll.Visible = false;

                if (rblOptions.SelectedItem.Value.Equals("Version"))
                {
                    string version = ddlVersion.SelectedItem.ToString();
                    string versionJSON = await IpswApiCache.GetIpswByVersionJsonAsync(version);
                    dynamic jsonVersionObj = JsonConvert.DeserializeObject(versionJSON);
                    double originalFileSize = 0.00;
                    Dictionary<string, string> urlArray = new Dictionary<string, string>();
                    for (int i = 0; i < jsonVersionObj.Count; i++)
                    {
                        string url = jsonVersionObj[i]["url"];
                        string fileSize = jsonVersionObj[i]["filesize"];
                        if (!urlArray.ContainsKey(url))
                        {
                            urlArray.Add(url, fileSize);
                        }
                    }

                    var htmlBuilder = new StringBuilder();
                    var listBuilder = new StringBuilder();

                    foreach (KeyValuePair<string, string> item in urlArray)
                    {
                        string urlTitle = IpswUtilities.GetFileNameFromUrl(item.Key);

                        htmlBuilder.Append("<a href=\"")
                            .Append(HttpUtility.HtmlAttributeEncode(item.Key))
                            .Append("\" target=\"_blank\">")
                            .Append(HttpUtility.HtmlEncode(urlTitle))
                            .Append("</a><br/>");
                        listBuilder.Append(HttpUtility.HtmlEncode(item.Key)).Append(";");
                        originalFileSize += IpswUtilities.ParseFileSizeBytes(item.Value);
                    }

                    htmlBuilder.Append("<br/><br/><h4>URL in Text Format</h4><br/>");

                    foreach (KeyValuePair<string, string> item in urlArray)
                    {
                        htmlBuilder.Append(HttpUtility.HtmlEncode(item.Key)).Append("<br/>");
                    }

                    tbData.Text = htmlBuilder.ToString();
                    listOfLinks.Text = listBuilder.ToString();
                    lblSelectionComment.Text = "<br/>There are " + urlArray.Count +
                                               " Files<br/>The Total File Size are " + IpswUtilities.FormatGigabytes(originalFileSize);
                }
                else if (rblOptions.SelectedItem.Value.Equals("Version (OTA)"))
                {
                    string versionOTA = ddlVersionOTA.SelectedItem.ToString();
                    string versionOTAJSON = await IpswApiCache.GetOtaByVersionJsonAsync(versionOTA);
                    dynamic jsonVersionOTAObj = JsonConvert.DeserializeObject(versionOTAJSON);
                    double originalFileSize = 0.00;
                    Dictionary<string, string> otaFiles = new Dictionary<string, string>();
                    for (int i = 0; i < jsonVersionOTAObj.Count; i++)
                    {
                        string url = jsonVersionOTAObj[i]["url"];
                        string fileSize = jsonVersionOTAObj[i]["filesize"];
                        if (!otaFiles.ContainsKey(url))
                        {
                            otaFiles.Add(url, fileSize);
                        }
                    }

                    var htmlBuilder = new StringBuilder();
                    foreach (KeyValuePair<string, string> item in otaFiles)
                    {
                        htmlBuilder.Append(HttpUtility.HtmlEncode(item.Key)).Append("<br/>");
                        originalFileSize += IpswUtilities.ParseFileSizeBytes(item.Value);
                    }

                    tbData.Text = htmlBuilder.ToString();
                    lblSelectionComment.Text = "<br/>There are " + otaFiles.Count +
                                               " Files<br/>The Total File Size are " + IpswUtilities.FormatGigabytes(originalFileSize);
                }
                else
                {
                    string identifier = lblSelection.Text;
                    string firmwareType = rblOptions.SelectedItem.ToString();
                    string myJSON = "";

                    if (firmwareType.Equals("Official"))
                    {
                        myJSON = await IpswApiCache.GetDeviceFirmwareJsonAsync(identifier, "ipsw");
                    }
                    else if (firmwareType.Equals("OTA"))
                    {
                        myJSON = await IpswApiCache.GetDeviceFirmwareJsonAsync(identifier, "ota");
                    }

                    dynamic jsonObj = JsonConvert.DeserializeObject(myJSON);
                    double originalFileSize = 0.00;
                    var firmwareFiles = new List<KeyValuePair<string, string>>();
                    for (int i = 0; i < jsonObj["firmwares"].Count; i++)
                    {
                        firmwareFiles.Add(new KeyValuePair<string, string>(
                            jsonObj["firmwares"][i]["url"].ToString(),
                            jsonObj["firmwares"][i]["filesize"].ToString()));
                    }

                    var htmlBuilder = new StringBuilder();
                    foreach (KeyValuePair<string, string> item in firmwareFiles)
                    {
                        htmlBuilder.Append(HttpUtility.HtmlEncode(item.Key)).Append("<br/>");
                        originalFileSize += IpswUtilities.ParseFileSizeBytes(item.Value);
                    }

                    tbData.Text = htmlBuilder.ToString();
                    lblSelectionComment.Text = "<br/>There are " + firmwareFiles.Count +
                                               " Files<br/>The Total File Size are " + IpswUtilities.FormatGigabytes(originalFileSize);
                }
                tbData.Visible = true;
                btnDownloadAll.Visible = true;
            }
            catch (Exception)
            {
                Response.Redirect("~/Error.aspx");
            }
        }

        protected void ddlVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbData.Visible = false;
            tbData.Text = "";
            listOfLinks.Text = "";
            lblSelection.Text = HttpUtility.HtmlEncode(ddlVersion.SelectedValue);
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
            tbData.Visible = false;
            tbData.Text = "";
            listOfLinks.Text = "";
            lblSelection.Text = HttpUtility.HtmlEncode(ddlVersionOTA.SelectedValue);
            lblSelection.Font.Bold = true;
            lblSelection.Font.Size = 15;

            lblSelectionComment.Text = "You have selected OTA <b>" + HttpUtility.HtmlEncode(rblOptions.SelectedItem.ToString()) + " " + HttpUtility.HtmlEncode(ddlVersionOTA.SelectedItem.ToString());
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
