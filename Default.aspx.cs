using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace ipsw
{
    public partial class Default : BaseIpswPage
    {
        protected void btnRetrieve_Click(object sender, EventArgs e)
        {
            ResetResultPanels();
            tblData.Rows.Clear();

            if (OptionsList.SelectedItem == null)
            {
                return;
            }

            string selectionValue = OptionsList.SelectedItem.Value;
            if (selectionValue.Equals("Version", StringComparison.OrdinalIgnoreCase))
            {
                HandleVersionRetrieval();
            }
            else if (selectionValue.Equals("Version (OTA)", StringComparison.OrdinalIgnoreCase))
            {
                HandleVersionOtaRetrieval();
            }
            else
            {
                HandleDeviceRetrieval();
            }
        }

        private void HandleVersionRetrieval()
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
            AddVersionTableHeader();
            SelectionCommentLabel.Text += $"<br/>There are {jsonVersionObj.Count} Files";

            for (int i = 0; i < jsonVersionObj.Count; i++)
            {
                string identifier = jsonVersionObj[i]["identifier"];
                string buildID = jsonVersionObj[i]["buildid"];
                string url = jsonVersionObj[i]["url"];
                string fileSize = jsonVersionObj[i]["filesize"];
                string releaseDate = jsonVersionObj[i]["releasedate"];
                string signed = jsonVersionObj[i]["signed"];

                HyperLink hyperIdentifier = new HyperLink
                {
                    ID = "hypIdentifier" + i,
                    Text = HttpUtility.HtmlEncode(identifier) + "<br/>"
                };

                HyperLink hyperBuildID = new HyperLink
                {
                    ID = "hyperBuildID" + i,
                    Text = HttpUtility.HtmlEncode(buildID) + "<br/>"
                };

                HyperLink hyperURL = new HyperLink
                {
                    ID = "hyperURL" + i,
                    NavigateUrl = url
                };

                HyperLink hyperFileSize = new HyperLink { ID = "hyperFileSize" + i };
                if (double.TryParse(fileSize, NumberStyles.Any, CultureInfo.InvariantCulture, out double sizeBytes))
                {
                    double fileSizeGb = sizeBytes / 1024 / 1024 / 1024;
                    hyperFileSize.Text = fileSizeGb.ToString("0.##", CultureInfo.InvariantCulture) + " GB";
                }

                HyperLink hyperReleaseDate = new HyperLink { ID = "hyperReleaseDate" + i };
                if (!string.IsNullOrEmpty(releaseDate) && DateTime.TryParse(releaseDate, out DateTime releaseDt))
                {
                    hyperReleaseDate.Text = HttpUtility.HtmlEncode(releaseDt.ToLongDateString() + " " + releaseDt.ToLongTimeString());
                }
                else
                {
                    hyperReleaseDate.Text = "-";
                }

                HyperLink hyperSigned = BuildSignedHyperLink(signed, i);

                string[] links = url.Split('/');
                int linkInt = links.Length - 1;
                hyperURL.Text = HttpUtility.HtmlEncode(links[linkInt]) + "<br/>";

                AddVersionRow(hyperIdentifier, hyperBuildID, hyperURL, hyperFileSize, hyperReleaseDate, hyperSigned);
            }
        }

        private void HandleVersionOtaRetrieval()
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
            TableHeaderRow thrVersionOTA = new TableHeaderRow();
            TableHeaderCell tableHeaderIdentifierOTA = new TableHeaderCell { Text = "Identifier" };
            TableHeaderCell tableHeaderBuildIDOTA = new TableHeaderCell { Text = "Build ID" };
            TableHeaderCell tableHeaderURLOTA = new TableHeaderCell { Text = "Download Links" };
            TableHeaderCell tableHeaderFileSizeOTA = new TableHeaderCell { Text = "File Size" };
            TableHeaderCell tableHeaderReleasedDateOTA = new TableHeaderCell { Text = "Released Date" };
            TableHeaderCell tableHeaderSignedOTA = new TableHeaderCell { Text = "Signed by Apple" };

            thrVersionOTA.Cells.Add(tableHeaderIdentifierOTA);
            thrVersionOTA.Cells.Add(tableHeaderBuildIDOTA);
            thrVersionOTA.Cells.Add(tableHeaderURLOTA);
            thrVersionOTA.Cells.Add(tableHeaderFileSizeOTA);
            thrVersionOTA.Cells.Add(tableHeaderReleasedDateOTA);
            thrVersionOTA.Cells.Add(tableHeaderSignedOTA);
            tblData.Rows.AddAt(0, thrVersionOTA);
            SelectionCommentLabel.Text += $"<br/>There are {jsonVersionOTAObj.Count} Files";

            for (int i = 0; i < jsonVersionOTAObj.Count; i++)
            {
                string identifier = jsonVersionOTAObj[i]["identifier"];
                string buildID = jsonVersionOTAObj[i]["buildid"];
                string url = jsonVersionOTAObj[i]["url"];
                string fileSize = jsonVersionOTAObj[i]["filesize"];
                string releaseDate = jsonVersionOTAObj[i]["releasedate"];
                string signedOTA = jsonVersionOTAObj[i]["signed"];

                HyperLink hyperIdentifierOTA = new HyperLink
                {
                    ID = "hypIdentifier" + i,
                    Text = HttpUtility.HtmlEncode(identifier) + "<br/>"
                };

                HyperLink hyperBuildIDOTA = new HyperLink
                {
                    ID = "hyperBuildID" + i,
                    Text = HttpUtility.HtmlEncode(buildID) + "<br/>"
                };

                HyperLink hyperURLOTA = new HyperLink
                {
                    ID = "hyperURL" + i,
                    NavigateUrl = url
                };

                HyperLink hyperFileSizeOTA = new HyperLink { ID = "hyperFileSize" + i };
                if (double.TryParse(fileSize, NumberStyles.Any, CultureInfo.InvariantCulture, out double fileSizeValue))
                {
                    double fileSizeGB = fileSizeValue / 1024 / 1024 / 1024;
                    hyperFileSizeOTA.Text = fileSizeGB.ToString("0.##", CultureInfo.InvariantCulture) + " GB";
                }

                HyperLink hyperReleaseDateOTA = new HyperLink { ID = "hyperReleaseDate" + i };
                if (!string.IsNullOrEmpty(releaseDate))
                {
                    hyperReleaseDateOTA.Text = HttpUtility.HtmlEncode(releaseDate);
                }
                else
                {
                    hyperReleaseDateOTA.Text = "-";
                }

                HyperLink hyperSignedOTA = BuildSignedHyperLink(signedOTA, i);

                string[] links = url.Split('/');
                int linkInt = links.Length - 1;
                hyperURLOTA.Text = HttpUtility.HtmlEncode(links[linkInt]) + "<br/>";

                AddVersionRow(hyperIdentifierOTA, hyperBuildIDOTA, hyperURLOTA, hyperFileSizeOTA, hyperReleaseDateOTA, hyperSignedOTA);
            }
        }

        private void HandleDeviceRetrieval()
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
            TableHeaderRow thr = new TableHeaderRow();
            TableHeaderCell headerBuildID = new TableHeaderCell { Text = "Build ID" };
            TableHeaderCell headerLinks = new TableHeaderCell { Text = "Links" };
            TableHeaderCell headerReleaseDT = new TableHeaderCell { Text = "Release Date & Time" };
            TableHeaderCell headerFS = new TableHeaderCell { Text = "File Size" };
            TableHeaderCell headerSigned = new TableHeaderCell { Text = "Signed by Apple" };

            thr.Cells.Add(headerBuildID);
            thr.Cells.Add(headerLinks);
            thr.Cells.Add(headerFS);
            thr.Cells.Add(headerReleaseDT);
            thr.Cells.Add(headerSigned);
            tblData.Rows.AddAt(0, thr);
            SelectionCommentLabel.Text += $"<br/>There are {jsonObj["firmwares"].Count} Files";

            for (int i = 0; i < jsonObj["firmwares"].Count; i++)
            {
                string buildid = jsonObj["firmwares"][i]["buildid"].ToString();
                string url = jsonObj["firmwares"][i]["url"].ToString();
                string dateReleased = jsonObj["firmwares"][i]["releasedate"].ToString();
                string fileSize = jsonObj["firmwares"][i]["filesize"].ToString();
                string signed = jsonObj["firmwares"][i]["signed"].ToString();

                HyperLink hypName = new HyperLink
                {
                    ID = "hypName" + i,
                    Text = HttpUtility.HtmlEncode(buildid) + "<br/>"
                };

                HyperLink hyp = new HyperLink
                {
                    ID = "hypABD" + i,
                    NavigateUrl = url
                };

                HyperLink hypDateReleased = new HyperLink { ID = "hypDateReleased" + i };
                if (!string.IsNullOrEmpty(dateReleased) && DateTime.TryParse(dateReleased, out DateTime dr))
                {
                    hypDateReleased.Text = HttpUtility.HtmlEncode(dr.ToLongDateString() + " " + dr.ToLongTimeString());
                }
                else
                {
                    hypDateReleased.Text = "-";
                }

                HyperLink hypFileSize = new HyperLink { ID = "hypFileSize" + i };
                if (double.TryParse(fileSize, NumberStyles.Any, CultureInfo.InvariantCulture, out double fileSizeBytes))
                {
                    double fileSizeGB = fileSizeBytes / 1024 / 1024 / 1024;
                    hypFileSize.Text = fileSizeGB.ToString("0.##", CultureInfo.InvariantCulture) + " GB";
                }

                HyperLink hypSigned = BuildSignedHyperLink(signed, i);

                string[] links = url.Split('/');
                int linkInt = links.Length - 1;
                hyp.Text = HttpUtility.HtmlEncode(links[linkInt]) + "<br/>";

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
                tr.Cells.Add(tdLinks);
                tr.Cells.Add(tdHyperFileSize);
                tr.Cells.Add(tdDateReleased);
                tr.Cells.Add(tdSigned);
                tblData.BorderStyle = BorderStyle.Solid;
                tblData.Rows.Add(tr);
            }
        }

        private void AddVersionTableHeader()
        {
            TableHeaderRow thrVersion = new TableHeaderRow();
            TableHeaderCell tableHeaderIdentifier = new TableHeaderCell { Text = "Identifier" };
            TableHeaderCell tableHeaderBuildID = new TableHeaderCell { Text = "Build ID" };
            TableHeaderCell tableHeaderURL = new TableHeaderCell { Text = "Download Links" };
            TableHeaderCell tableHeaderFileSize = new TableHeaderCell { Text = "File Size" };
            TableHeaderCell tableHeaderReleasedDate = new TableHeaderCell { Text = "Released Date" };
            TableHeaderCell tableHeaderSigned = new TableHeaderCell { Text = "Signed by Apple" };

            thrVersion.Cells.Add(tableHeaderIdentifier);
            thrVersion.Cells.Add(tableHeaderBuildID);
            thrVersion.Cells.Add(tableHeaderURL);
            thrVersion.Cells.Add(tableHeaderFileSize);
            thrVersion.Cells.Add(tableHeaderReleasedDate);
            thrVersion.Cells.Add(tableHeaderSigned);
            tblData.Rows.AddAt(0, thrVersion);
        }

        private void AddVersionRow(HyperLink identifier, HyperLink buildId, HyperLink url, HyperLink fileSize, HyperLink releaseDate, HyperLink signed)
        {
            TableRow tr = new TableRow();
            TableCell tdIdentifier = new TableCell();
            TableCell tdBuildID = new TableCell();
            TableCell tdURL = new TableCell();
            TableCell tdFileSize = new TableCell();
            TableCell tdReleaseDate = new TableCell();
            TableCell tdSigned = new TableCell();

            tdIdentifier.Controls.Add(identifier);
            tdBuildID.Controls.Add(buildId);
            tdURL.Controls.Add(url);
            tdFileSize.Controls.Add(fileSize);
            tdReleaseDate.Controls.Add(releaseDate);
            tdSigned.Controls.Add(signed);

            tblData.BorderStyle = BorderStyle.Solid;
            tr.Cells.Add(tdIdentifier);
            tr.Cells.Add(tdBuildID);
            tr.Cells.Add(tdURL);
            tr.Cells.Add(tdFileSize);
            tr.Cells.Add(tdReleaseDate);
            tr.Cells.Add(tdSigned);
            tblData.Rows.Add(tr);
        }

        private HyperLink BuildSignedHyperLink(string signedValue, int index)
        {
            HyperLink hyperSigned = new HyperLink
            {
                ID = "hyperSigned" + index
            };

            if (signedValue == "True")
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

            return hyperSigned;
        }
    }
}
