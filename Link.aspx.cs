using System;
using System.Collections.Generic;
using System.IO;
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
                    if (string.IsNullOrWhiteSpace(Url))
                    {
                        return string.Empty;
                    }

                    Uri uri;
                    if (Uri.TryCreate(Url, UriKind.Absolute, out uri))
                    {
                        string fileName = Path.GetFileName(uri.AbsolutePath);
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            return fileName;
                        }
                    }

                    return Url;
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

            DisplayAggregatedLinks($"https://api.ipsw.me/v4/ipsw/{version}", FirmwareAggregationMode.VersionIpsw);
        }

        private void HandleVersionOtaLinks()
        {
            string versionOTA = VersionOtaDropDown.SelectedItem?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(versionOTA))
            {
                return;
            }

            DisplayAggregatedLinks($"https://api.ipsw.me/v4/ota/{versionOTA}", FirmwareAggregationMode.VersionOta);
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
            DisplayAggregatedLinks($"https://api.ipsw.me/v4/device/{identifier}?type={endpointType}", FirmwareAggregationMode.Device);
        }

        private void DisplayAggregatedLinks(string endpointUrl, FirmwareAggregationMode mode)
        {
            if (!TryDownloadString(endpointUrl, out string json))
            {
                return;
            }

            FirmwareAggregationResult result = FirmwareAggregationHelper.Aggregate(json, mode);
            DisplayLinks(result);
        }

        private void DisplayLinks(FirmwareAggregationResult result)
        {
            listOfLinks.Text = string.Empty;

            if (result == null || result.Count == 0)
            {
                SelectionCommentLabel.Text += "<br/>No files found.";
                return;
            }

            List<FirmwareLink> links = new List<FirmwareLink>();
            StringBuilder rawLinksBuilder = new StringBuilder();
            StringBuilder downloadBuilder = new StringBuilder();

            foreach (FirmwareLinkInfo link in result.Links)
            {
                links.Add(new FirmwareLink { Url = link.Url });
                rawLinksBuilder.Append(HttpUtility.HtmlEncode(link.Url)).Append("<br/>");
                downloadBuilder.Append(link.Url).Append(';');
            }

            double fileSizeGB = result.TotalSizeBytes / 1024d / 1024d / 1024d;
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
