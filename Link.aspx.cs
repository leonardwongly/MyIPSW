using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace ipsw
{
    public partial class Link : BaseIpswPage
    {
        public class FirmwareLink
        {
            public string Url { get; set; }

            public string FileName
            {
                get
                {
                    if (string.IsNullOrEmpty(Url))
                    {
                        return string.Empty;
                    }

                    string[] parts = Url.Split('/');
                    return parts[parts.Length - 1];
                }
            }
        }

        protected override void ResetResultPanels()
        {
            base.ResetResultPanels();
            rptLinks.Visible = false;
            btnDownloadAll.Visible = false;
            listOfLinks.Text = string.Empty;
        }

        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            ResetResultPanels();

            if (OptionsList.SelectedItem == null)
            {
                return;
            }

            string selectionValue = OptionsList.SelectedItem.Value;
            if (selectionValue.Equals("Version", StringComparison.OrdinalIgnoreCase))
            {
                HandleVersionLinks();
            }
            else if (selectionValue.Equals("Version (OTA)", StringComparison.OrdinalIgnoreCase))
            {
                HandleVersionOtaLinks();
            }
            else
            {
                HandleDeviceLinks();
            }
        }

        private void HandleVersionLinks()
        {
            string version = VersionDropDown.SelectedItem?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(version))
            {
                return;
            }

            if (!TryDownloadString($"https://api.ipsw.me/v4/ipsw/{version}", out string versionJson))
            {
                return;
            }

            dynamic jsonVersionObj = JsonConvert.DeserializeObject(versionJson);
            Dictionary<string, string> urlDictionary = new Dictionary<string, string>();
            double totalBytes = 0;

            for (int i = 0; i < jsonVersionObj.Count; i++)
            {
                string url = jsonVersionObj[i]["url"];
                string fileSize = jsonVersionObj[i]["filesize"];
                if (!urlDictionary.ContainsKey(url))
                {
                    urlDictionary.Add(url, fileSize);
                }
            }

            foreach (KeyValuePair<string, string> item in urlDictionary)
            {
                if (double.TryParse(item.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double bytes))
                {
                    totalBytes += bytes;
                }
            }

            DisplayLinks(urlDictionary.Keys.ToList(), totalBytes);
        }

        private void HandleVersionOtaLinks()
        {
            string versionOTA = VersionOtaDropDown.SelectedItem?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(versionOTA))
            {
                return;
            }

            if (!TryDownloadString($"https://api.ipsw.me/v4/ota/{versionOTA}", out string versionOtaJson))
            {
                return;
            }

            dynamic jsonVersionOTAObj = JsonConvert.DeserializeObject(versionOtaJson);
            Dictionary<string, string> urlDictionary = new Dictionary<string, string>();
            double totalBytes = 0;

            for (int i = 0; i < jsonVersionOTAObj.Count; i++)
            {
                string url = jsonVersionOTAObj[i]["url"];
                string fileSize = jsonVersionOTAObj[i]["filesize"];
                if (!urlDictionary.ContainsKey(url))
                {
                    urlDictionary.Add(url, fileSize);
                }
            }

            foreach (KeyValuePair<string, string> item in urlDictionary)
            {
                if (double.TryParse(item.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out double bytes))
                {
                    totalBytes += bytes;
                }
            }

            DisplayLinks(urlDictionary.Keys.ToList(), totalBytes);
        }

        private void HandleDeviceLinks()
        {
            string identifier = HttpUtility.HtmlDecode(SelectionLabel.Text);
            string firmwareType = OptionsList.SelectedItem?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(identifier) || string.IsNullOrEmpty(firmwareType))
            {
                return;
            }

            string endpointType = firmwareType.Equals("Official", StringComparison.OrdinalIgnoreCase) ? "ipsw" : "ota";
            if (!TryDownloadString($"https://api.ipsw.me/v4/device/{identifier}?type={endpointType}", out string firmwareJson))
            {
                return;
            }

            dynamic jsonObj = JsonConvert.DeserializeObject(firmwareJson);
            List<string> links = new List<string>();
            double totalBytes = 0;

            for (int i = 0; i < jsonObj["firmwares"].Count; i++)
            {
                string url = jsonObj["firmwares"][i]["url"];
                string fileSize = jsonObj["firmwares"][i]["filesize"];
                links.Add(url);
                if (double.TryParse(fileSize, NumberStyles.Any, CultureInfo.InvariantCulture, out double bytes))
                {
                    totalBytes += bytes;
                }
            }

            DisplayLinks(links, totalBytes);
        }

        private void DisplayLinks(IReadOnlyCollection<string> urls, double totalBytes)
        {
            listOfLinks.Text = string.Empty;

            if (urls == null || urls.Count == 0)
            {
                SelectionCommentLabel.Text += "<br/>No files found.";
                return;
            }

            List<FirmwareLink> links = new List<FirmwareLink>();
            StringBuilder rawLinksBuilder = new StringBuilder();
            StringBuilder downloadBuilder = new StringBuilder();

            foreach (string url in urls)
            {
                links.Add(new FirmwareLink { Url = url });
                rawLinksBuilder.Append(HttpUtility.HtmlEncode(url) + "<br/>");
                downloadBuilder.Append(url + ";");
            }

            double fileSizeGB = totalBytes / 1024 / 1024 / 1024;
            SelectionCommentLabel.Text += $"<br/>There are {links.Count} Files<br/>The Total File Size are {fileSizeGB.ToString("0.##", CultureInfo.InvariantCulture)} GB";

            BindLinksToRepeater(links, rawLinksBuilder.ToString(), downloadBuilder.ToString());
        }

        private void BindLinksToRepeater(List<FirmwareLink> links, string rawLinksHtml, string downloadList)
        {
            rptLinks.DataSource = links;
            rptLinks.DataBind();
            UpdateRawLinksLiteral(rawLinksHtml);

            bool hasLinks = links.Count > 0;
            rptLinks.Visible = hasLinks;
            btnDownloadAll.Visible = hasLinks;
            listOfLinks.Text = downloadList;
        }

        private void UpdateRawLinksLiteral(string content)
        {
            RepeaterItem footerItem = rptLinks.Controls.OfType<RepeaterItem>()
                .FirstOrDefault(item => item.ItemType == ListItemType.Footer);

            if (footerItem != null)
            {
                Literal litRawLinks = footerItem.FindControl("litRawLinks") as Literal;
                if (litRawLinks != null)
                {
                    litRawLinks.Text = content;
                }
            }
        }
    }
}
