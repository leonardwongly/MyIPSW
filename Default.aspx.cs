using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ipsw
{
    public partial class Default : Page
    {
        private const string DefaultResultSummary = "No results yet. Complete the steps and select Retrieve.";
        private static readonly IFirmwareQueryService FirmwareQueryService = new FirmwareQueryService();
        private bool isNextExperienceEnabled = true;

        protected async void Page_Load(object sender, EventArgs e)
        {
            isNextExperienceEnabled = IsNextExperienceEnabledForRequest();
            ApplyRolloutMode();

            if (IsPostBack)
            {
                return;
            }

            InitializePageDefaults();

            try
            {
                await PopulateSelectionControlsAsync().ConfigureAwait(true);
                var validation = SelectionStateValidator.ParseFromQuery(Request.QueryString, GetKnownDeviceIdentifiers());

                ApplySelectionState(validation.State);
                if (!isNextExperienceEnabled)
                {
                    ShowStatus("You are in the phased rollout control group. Table view is enabled. Add ?ui=next to preview links mode.", "info", false);
                }

                if (validation.Warnings.Count > 0)
                {
                    ShowStatus(string.Join(" ", validation.Warnings), "warning", false);
                }

                if (validation.State.IsReadyToRetrieve)
                {
                    await RetrieveAndRenderAsync(validation.State, false).ConfigureAwait(true);
                }
                else
                {
                    UpdateSelectionSummary(validation.State);
                }
            }
            catch
            {
                ShowStatus("We could not load firmware metadata right now. Please retry in a moment.", "danger", true);
                hfTelemetryStatus.Value = "error";
            }
        }

        protected void rblOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearResults();
            ClearStatus();

            var state = BuildStateFromControls();
            SetSourceVisibility(state.Source);
            ClearTargetSelections();
            btnRetrieve.Visible = false;
            lblStep3.Visible = false;
            UpdateSelectionSummary(state);
        }

        protected void ddliPhone_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleDeviceSelectionChanged(ddliPhone);
        }

        protected void ddliPad_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleDeviceSelectionChanged(ddliPad);
        }

        protected void ddliPod_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleDeviceSelectionChanged(ddliPod);
        }

        protected void ddlWatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleDeviceSelectionChanged(ddlWatch);
        }

        protected void ddlAudioAccessory_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleDeviceSelectionChanged(ddlAudioAccessory);
        }

        protected void ddlAppleTV_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleDeviceSelectionChanged(ddlAppleTV);
        }

        protected void ddlMac_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleDeviceSelectionChanged(ddlMac);
        }

        protected void ddlVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearResults();
            ClearStatus();
            ddlVersionOTA.SelectedIndex = 0;

            var state = BuildStateFromControls();
            btnRetrieve.Visible = ddlVersion.SelectedIndex > 0;
            lblStep3.Visible = ddlVersion.SelectedIndex > 0;
            UpdateSelectionSummary(state);
        }

        protected void ddlVersionOTA_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearResults();
            ClearStatus();
            ddlVersion.SelectedIndex = 0;

            var state = BuildStateFromControls();
            btnRetrieve.Visible = ddlVersionOTA.SelectedIndex > 0;
            lblStep3.Visible = ddlVersionOTA.SelectedIndex > 0;
            UpdateSelectionSummary(state);
        }

        protected async void rblResultMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var state = BuildStateFromControls();
            UpdateSelectionSummary(state);

            if (!state.IsReadyToRetrieve)
            {
                return;
            }

            await RetrieveAndRenderAsync(state, false).ConfigureAwait(true);
        }

        protected async void btnRetrieve_Click(object sender, EventArgs e)
        {
            var state = BuildStateFromControls();
            await RetrieveAndRenderAsync(state, true).ConfigureAwait(true);
        }

        protected async void btnRetry_Click(object sender, EventArgs e)
        {
            var state = BuildStateFromControls();
            if (!state.IsReadyToRetrieve)
            {
                ShowStatus("Select a source and target before retrying.", "warning", false);
                return;
            }

            await RetrieveAndRenderAsync(state, true).ConfigureAwait(true);
        }

        private async Task RetrieveAndRenderAsync(SelectionState state, bool showLoadingStatus)
        {
            if (!isNextExperienceEnabled)
            {
                state.ResultMode = ResultMode.Table;
            }

            var knownDevices = GetKnownDeviceIdentifiers();
            string validationMessage;
            if (!SelectionStateValidator.ValidateSelection(state, knownDevices, out validationMessage))
            {
                ShowStatus(validationMessage, "warning", false);
                hfTelemetryStatus.Value = "error";
                return;
            }

            ClearResults();
            UpdateSelectionSummary(state);

            if (showLoadingStatus)
            {
                ShowStatus("Loading firmware results...", "info", false);
            }

            btnRetrieve.Enabled = false;

            try
            {
                var records = await FirmwareQueryService.GetFirmwareRecordsAsync(state).ConfigureAwait(true);
                if (state.ResultMode == ResultMode.Links)
                {
                    RenderLinks(records);
                }
                else
                {
                    RenderTable(records);
                }

                var resultCount = records.Count;
                hfResultCount.Value = resultCount.ToString(CultureInfo.InvariantCulture);

                if (resultCount == 0)
                {
                    lblResultSummary.Text = HttpUtility.HtmlEncode("No files matched this selection.");
                    ShowStatus("No firmware files matched your selection. Try another source or version.", "warning", false);
                    hfTelemetryStatus.Value = "empty";
                    return;
                }

                var totalBytes = records.Sum(r => r.FileSizeBytes);
                lblResultSummary.Text = HttpUtility.HtmlEncode(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Showing {0} file(s). Total size: {1}.",
                        resultCount,
                        IpswUtilities.FormatGigabytes(totalBytes)));

                ShowStatus("Results loaded successfully.", "success", false);
                hfTelemetryStatus.Value = "success";
            }
            catch
            {
                ClearResults();
                ShowStatus("We could not retrieve firmware right now. Please retry.", "danger", true);
                hfTelemetryStatus.Value = "error";
            }
            finally
            {
                btnRetrieve.Enabled = true;
            }
        }

        private void RenderTable(IReadOnlyList<FirmwareRecord> records)
        {
            tblData.Rows.Clear();
            tblData.Visible = true;
            lblLinkResults.Visible = false;
            lblLinkResults.Text = string.Empty;
            btnDownloadAll.Visible = false;
            hfDownloadLinks.Value = string.Empty;

            var header = new TableHeaderRow();
            header.Cells.Add(new TableHeaderCell { Text = "Identifier" });
            header.Cells.Add(new TableHeaderCell { Text = "Build ID" });
            header.Cells.Add(new TableHeaderCell { Text = "Download" });
            header.Cells.Add(new TableHeaderCell { Text = "File Size" });
            header.Cells.Add(new TableHeaderCell { Text = "Released" });
            header.Cells.Add(new TableHeaderCell { Text = "Signed" });
            tblData.Rows.Add(header);

            foreach (var record in records)
            {
                var row = new TableRow();

                row.Cells.Add(new TableCell { Text = HttpUtility.HtmlEncode(record.Identifier) });
                row.Cells.Add(new TableCell { Text = HttpUtility.HtmlEncode(record.BuildId) });

                var linkCell = new TableCell();
                string safeUrl;
                if (TryNormalizeHttpUrl(record.Url, out safeUrl))
                {
                    var link = new HyperLink
                    {
                        NavigateUrl = safeUrl,
                        Target = "_blank",
                        Text = HttpUtility.HtmlEncode(string.IsNullOrWhiteSpace(record.FileName) ? safeUrl : record.FileName)
                    };
                    link.Attributes["rel"] = "noopener";
                    linkCell.Controls.Add(link);
                }
                else
                {
                    linkCell.Text = "-";
                }

                row.Cells.Add(linkCell);
                row.Cells.Add(new TableCell { Text = HttpUtility.HtmlEncode(IpswUtilities.FormatGigabytes(record.FileSizeBytes)) });
                row.Cells.Add(new TableCell { Text = HttpUtility.HtmlEncode(string.IsNullOrWhiteSpace(record.ReleaseDateText) ? "-" : record.ReleaseDateText) });

                var signedCell = new TableCell();
                if (!record.IsSigned.HasValue)
                {
                    signedCell.Text = "-";
                }
                else if (record.IsSigned.Value)
                {
                    signedCell.Text = "Yes";
                    signedCell.ForeColor = System.Drawing.Color.Green;
                    signedCell.Font.Bold = true;
                }
                else
                {
                    signedCell.Text = "No";
                    signedCell.ForeColor = System.Drawing.Color.Red;
                    signedCell.Font.Bold = true;
                }

                row.Cells.Add(signedCell);
                tblData.Rows.Add(row);
            }
        }

        private void RenderLinks(IReadOnlyList<FirmwareRecord> records)
        {
            tblData.Rows.Clear();
            tblData.Visible = false;
            lblLinkResults.Visible = true;

            var html = new StringBuilder();
            var downloadLinks = new List<string>();

            foreach (var record in records)
            {
                string safeUrl;
                if (!TryNormalizeHttpUrl(record.Url, out safeUrl))
                {
                    continue;
                }

                downloadLinks.Add(safeUrl);
                html.Append("<a href=\"")
                    .Append(HttpUtility.HtmlAttributeEncode(safeUrl))
                    .Append("\" target=\"_blank\" rel=\"noopener\">")
                    .Append(HttpUtility.HtmlEncode(string.IsNullOrWhiteSpace(record.FileName) ? safeUrl : record.FileName))
                    .Append("</a><br/>");
            }

            if (downloadLinks.Count > 0)
            {
                html.Append("<hr/><strong>Raw URLs</strong><br/>");
                foreach (var link in downloadLinks)
                {
                    html.Append(HttpUtility.HtmlEncode(link)).Append("<br/>");
                }
            }

            hfDownloadLinks.Value = string.Join("\n", downloadLinks);
            btnDownloadAll.Visible = downloadLinks.Count > 0;
            lblLinkResults.Text = html.ToString();
        }

        private void HandleDeviceSelectionChanged(DropDownList selectedDropDown)
        {
            ClearResults();
            ClearStatus();

            ResetDeviceDropdownsExcept(selectedDropDown);

            var state = BuildStateFromControls();
            var hasSelection = selectedDropDown.SelectedIndex > 0;
            btnRetrieve.Visible = hasSelection;
            lblStep3.Visible = hasSelection;

            UpdateSelectionSummary(state);
        }

        private void UpdateSelectionSummary(SelectionState state)
        {
            if (state == null || !state.Source.HasValue)
            {
                lblSelectionComment.Text = HttpUtility.HtmlEncode("Choose a firmware source to begin.");
                lblSelection.Text = string.Empty;
                return;
            }

            var sourceText = GetSourceDisplayName(state.Source.Value);
            var modeText = state.ResultMode == ResultMode.Links ? "Links" : "Table";

            if (!state.HasTargetSelection)
            {
                lblSelectionComment.Text = HttpUtility.HtmlEncode(
                    string.Format(CultureInfo.InvariantCulture, "Source: {0}. Output: {1}. Select a target to continue.", sourceText, modeText));
                lblSelection.Text = string.Empty;
                return;
            }

            if (state.Source.Value == FirmwareSource.Version || state.Source.Value == FirmwareSource.VersionOta)
            {
                lblSelectionComment.Text = HttpUtility.HtmlEncode(
                    string.Format(CultureInfo.InvariantCulture, "Source: {0}. Version selected:", sourceText));
                lblSelection.Text = HttpUtility.HtmlEncode(state.Version);
                return;
            }

            var selectedText = GetSelectedDeviceDisplayText(state.DeviceIdentifier);
            lblSelectionComment.Text = HttpUtility.HtmlEncode(
                string.Format(CultureInfo.InvariantCulture, "Source: {0}. Device selected:", sourceText));
            lblSelection.Text = HttpUtility.HtmlEncode(
                string.Format(CultureInfo.InvariantCulture, "{0} ({1})", selectedText, state.DeviceIdentifier));
        }

        private string GetSelectedDeviceDisplayText(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return string.Empty;
            }

            var controls = GetDeviceDropDowns();
            foreach (var control in controls)
            {
                var item = control.Items.FindByValue(identifier);
                if (item != null)
                {
                    return item.Text;
                }
            }

            return identifier;
        }

        private void InitializePageDefaults()
        {
            btnRetrieve.Visible = false;
            btnDownloadAll.Visible = false;
            btnRetry.Visible = false;

            lblStep2.Visible = false;
            lblStep3.Visible = false;

            HideAllSelectionControls();
            ClearResults();
            ClearStatus();

            if (rblResultMode.Items.Count > 0)
            {
                rblResultMode.SelectedValue = "table";
            }

            ApplyRolloutMode();

            hfTelemetryStatus.Value = "none";
            hfResultCount.Value = "0";
        }

        private async Task PopulateSelectionControlsAsync()
        {
            ResetSelectableItems(ddliPhone);
            ResetSelectableItems(ddliPad);
            ResetSelectableItems(ddliPod);
            ResetSelectableItems(ddlWatch);
            ResetSelectableItems(ddlAppleTV);
            ResetSelectableItems(ddlMac);
            ResetSelectableItems(ddlAudioAccessory);
            ResetSelectableItems(ddlVersion);
            ResetSelectableItems(ddlVersionOTA);

            var devicesJson = await IpswApiCache.GetDevicesJsonAsync().ConfigureAwait(true);
            var devices = JArray.Parse(devicesJson);

            foreach (var device in devices)
            {
                var name = ReadSafeToken(device, "name", 100);
                var identifier = ReadSafeToken(device, "identifier", 64);
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(identifier))
                {
                    continue;
                }

                if (identifier.IndexOf("iPhone", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ddliPhone.Items.Add(new ListItem(name, identifier));
                }
                else if (identifier.IndexOf("iPad", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ddliPad.Items.Add(new ListItem(name, identifier));
                }
                else if (identifier.IndexOf("iPod", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ddliPod.Items.Add(new ListItem(name, identifier));
                }
                else if (identifier.IndexOf("Watch", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ddlWatch.Items.Add(new ListItem(name, identifier));
                }
                else if (identifier.IndexOf("AppleTV", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ddlAppleTV.Items.Add(new ListItem(name, identifier));
                }
                else if (identifier.IndexOf("AudioAccessory", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ddlAudioAccessory.Items.Add(new ListItem(name, identifier));
                }
                else if (identifier.IndexOf("Mac", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ddlMac.Items.Add(new ListItem(name, identifier));
                }
            }

            foreach (var version in VersionData.iOSVersions)
            {
                ddlVersion.Items.Add(new ListItem(version, version));
            }

            foreach (var version in VersionData.OTAVersions)
            {
                ddlVersionOTA.Items.Add(new ListItem(version, version));
            }
        }

        private void ApplySelectionState(SelectionState state)
        {
            if (state == null)
            {
                return;
            }

            if (!isNextExperienceEnabled)
            {
                state.ResultMode = ResultMode.Table;
            }

            if (state.Source.HasValue)
            {
                SelectListItemByValue(rblOptions, state.ToSourceQueryValue());
            }

            SelectListItemByValue(rblResultMode, state.ToResultModeQueryValue());
            SetSourceVisibility(state.Source);

            if (!state.Source.HasValue)
            {
                return;
            }

            if (state.Source.Value == FirmwareSource.Version)
            {
                SelectListItemByValue(ddlVersion, state.Version);
            }
            else if (state.Source.Value == FirmwareSource.VersionOta)
            {
                SelectListItemByValue(ddlVersionOTA, state.Version);
            }
            else
            {
                SelectDeviceByIdentifier(state.DeviceIdentifier);
            }

            btnRetrieve.Visible = state.HasTargetSelection;
            lblStep3.Visible = state.HasTargetSelection;
            UpdateSelectionSummary(state);
        }

        private void SelectDeviceByIdentifier(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return;
            }

            foreach (var dropDown in GetDeviceDropDowns())
            {
                if (SelectListItemByValue(dropDown, identifier))
                {
                    ResetDeviceDropdownsExcept(dropDown);
                    return;
                }
            }
        }

        private bool SelectListItemByValue(ListControl control, string value)
        {
            if (control == null || string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var item = control.Items.FindByValue(value);
            if (item == null)
            {
                return false;
            }

            control.ClearSelection();
            item.Selected = true;
            return true;
        }

        private SelectionState BuildStateFromControls()
        {
            var state = new SelectionState();

            FirmwareSource parsedSource;
            if (SelectionStateValidator.TryParseSource(rblOptions.SelectedValue, out parsedSource))
            {
                state.Source = parsedSource;
            }

            ResultMode parsedMode;
            if (SelectionStateValidator.TryParseResultMode(rblResultMode.SelectedValue, out parsedMode))
            {
                state.ResultMode = parsedMode;
            }

            if (!isNextExperienceEnabled)
            {
                state.ResultMode = ResultMode.Table;
            }

            if (!state.Source.HasValue)
            {
                return state;
            }

            if (state.Source.Value == FirmwareSource.Version)
            {
                if (ddlVersion.SelectedIndex > 0)
                {
                    state.Version = ddlVersion.SelectedValue;
                }
            }
            else if (state.Source.Value == FirmwareSource.VersionOta)
            {
                if (ddlVersionOTA.SelectedIndex > 0)
                {
                    state.Version = ddlVersionOTA.SelectedValue;
                }
            }
            else
            {
                foreach (var control in GetDeviceDropDowns())
                {
                    if (control.Visible && control.SelectedIndex > 0)
                    {
                        state.DeviceIdentifier = control.SelectedValue;
                        break;
                    }
                }
            }

            state.IsUiNext = string.Equals(Request.QueryString["ui"], "next", StringComparison.OrdinalIgnoreCase);
            return state;
        }

        private void SetSourceVisibility(FirmwareSource? source)
        {
            HideAllSelectionControls();
            lblStep2.Visible = source.HasValue;

            if (!source.HasValue)
            {
                lblStep2.Text = "Step 2";
                return;
            }

            lblStep2.Text = "Step 2";
            switch (source.Value)
            {
                case FirmwareSource.Official:
                    lblStep2.Text = "Choose Device (Official)";
                    ddliPhone.Visible = true;
                    ddliPad.Visible = true;
                    ddliPod.Visible = true;
                    ddlMac.Visible = true;
                    break;
                case FirmwareSource.Ota:
                    lblStep2.Text = "Choose Device (OTA)";
                    ddliPhone.Visible = true;
                    ddliPad.Visible = true;
                    ddliPod.Visible = true;
                    ddlWatch.Visible = true;
                    ddlAppleTV.Visible = true;
                    ddlAudioAccessory.Visible = true;
                    break;
                case FirmwareSource.Version:
                    lblStep2.Text = "Choose iOS Version";
                    ddlVersion.Visible = true;
                    break;
                case FirmwareSource.VersionOta:
                    lblStep2.Text = "Choose OTA Version";
                    ddlVersionOTA.Visible = true;
                    break;
            }
        }

        private void HideAllSelectionControls()
        {
            ddliPhone.Visible = false;
            ddliPad.Visible = false;
            ddliPod.Visible = false;
            ddlWatch.Visible = false;
            ddlAppleTV.Visible = false;
            ddlMac.Visible = false;
            ddlAudioAccessory.Visible = false;
            ddlVersion.Visible = false;
            ddlVersionOTA.Visible = false;
        }

        private void ClearTargetSelections()
        {
            foreach (var control in GetDeviceDropDowns())
            {
                control.SelectedIndex = 0;
            }

            ddlVersion.SelectedIndex = 0;
            ddlVersionOTA.SelectedIndex = 0;
        }

        private void ResetDeviceDropdownsExcept(DropDownList keep)
        {
            foreach (var control in GetDeviceDropDowns())
            {
                if (!ReferenceEquals(control, keep))
                {
                    control.SelectedIndex = 0;
                }
            }
        }

        private IEnumerable<DropDownList> GetDeviceDropDowns()
        {
            yield return ddliPhone;
            yield return ddliPad;
            yield return ddliPod;
            yield return ddlWatch;
            yield return ddlAppleTV;
            yield return ddlMac;
            yield return ddlAudioAccessory;
        }

        private HashSet<string> GetKnownDeviceIdentifiers()
        {
            var identifiers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var control in GetDeviceDropDowns())
            {
                foreach (ListItem item in control.Items)
                {
                    if (item == null || string.IsNullOrWhiteSpace(item.Value) || item.Value.StartsWith("Select", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    identifiers.Add(item.Value);
                }
            }

            return identifiers;
        }

        private static string ReadSafeToken(JToken token, string fieldName, int maxLength)
        {
            if (token == null)
            {
                return string.Empty;
            }

            var raw = token[fieldName] == null ? string.Empty : token[fieldName].ToString();
            if (string.IsNullOrWhiteSpace(raw))
            {
                return string.Empty;
            }

            var trimmed = raw.Trim();
            if (trimmed.Length > maxLength)
            {
                trimmed = trimmed.Substring(0, maxLength);
            }

            return trimmed;
        }

        private static bool TryNormalizeHttpUrl(string url, out string safeUrl)
        {
            safeUrl = string.Empty;
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            Uri parsed;
            if (!Uri.TryCreate(url, UriKind.Absolute, out parsed))
            {
                return false;
            }

            if (!string.Equals(parsed.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(parsed.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            safeUrl = parsed.ToString();
            return true;
        }

        private static string GetSourceDisplayName(FirmwareSource source)
        {
            switch (source)
            {
                case FirmwareSource.Official:
                    return "Official";
                case FirmwareSource.Ota:
                    return "OTA";
                case FirmwareSource.Version:
                    return "Version";
                case FirmwareSource.VersionOta:
                    return "Version (OTA)";
                default:
                    return string.Empty;
            }
        }

        private void ShowStatus(string message, string level, bool showRetry)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                ClearStatus();
                return;
            }

            pnlStatus.Visible = true;
            lblStatus.Text = HttpUtility.HtmlEncode(message);
            btnRetry.Visible = showRetry;

            var normalized = string.IsNullOrWhiteSpace(level) ? "info" : level.Trim().ToLowerInvariant();
            switch (normalized)
            {
                case "success":
                    pnlStatus.CssClass = "alert alert-success d-flex justify-content-between align-items-center";
                    break;
                case "warning":
                    pnlStatus.CssClass = "alert alert-warning d-flex justify-content-between align-items-center";
                    break;
                case "danger":
                    pnlStatus.CssClass = "alert alert-danger d-flex justify-content-between align-items-center";
                    break;
                default:
                    pnlStatus.CssClass = "alert alert-info d-flex justify-content-between align-items-center";
                    break;
            }
        }

        private void ClearStatus()
        {
            pnlStatus.Visible = false;
            lblStatus.Text = string.Empty;
            btnRetry.Visible = false;
            pnlStatus.CssClass = "alert alert-info d-flex justify-content-between align-items-center";
        }

        private void ClearResults()
        {
            tblData.Rows.Clear();
            tblData.Visible = false;
            lblLinkResults.Text = string.Empty;
            lblLinkResults.Visible = false;
            btnDownloadAll.Visible = false;
            hfDownloadLinks.Value = string.Empty;
            hfResultCount.Value = "0";
            hfTelemetryStatus.Value = "none";
            lblResultSummary.Text = HttpUtility.HtmlEncode(DefaultResultSummary);
        }

        private static void ResetSelectableItems(ListControl control)
        {
            if (control == null)
            {
                return;
            }

            while (control.Items.Count > 1)
            {
                control.Items.RemoveAt(control.Items.Count - 1);
            }

            if (control.Items.Count > 0)
            {
                control.SelectedIndex = 0;
            }
        }

        private void ApplyRolloutMode()
        {
            if (rblResultMode == null || rblResultMode.Items.Count == 0)
            {
                return;
            }

            if (isNextExperienceEnabled)
            {
                rblResultMode.Enabled = true;
                return;
            }

            rblResultMode.Enabled = false;
            SelectListItemByValue(rblResultMode, "table");
        }

        private bool IsNextExperienceEnabledForRequest()
        {
            if (string.Equals(Request.QueryString["ui"], "next", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            var rolloutPercent = ReadAppSettingInt("UiRedesignRolloutPercent", 100, 0, 100);
            if (rolloutPercent >= 100)
            {
                return true;
            }

            if (rolloutPercent <= 0)
            {
                return false;
            }

            var seed = (Request.UserHostAddress ?? string.Empty) + "|" + (Request.UserAgent ?? string.Empty);
            var hash = ComputeStableHash(seed);
            if (hash == int.MinValue)
            {
                hash = 0;
            }

            var bucket = Math.Abs(hash % 100);
            return bucket < rolloutPercent;
        }

        private static int ReadAppSettingInt(string key, int fallback, int min, int max)
        {
            var setting = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(setting))
            {
                return fallback;
            }

            int parsed;
            if (!int.TryParse(setting, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsed))
            {
                return fallback;
            }

            if (parsed < min)
            {
                return min;
            }

            if (parsed > max)
            {
                return max;
            }

            return parsed;
        }

        private static int ComputeStableHash(string value)
        {
            unchecked
            {
                var hash = 17;
                if (value == null)
                {
                    return hash;
                }

                for (var i = 0; i < value.Length; i++)
                {
                    hash = (hash * 31) + value[i];
                }

                return hash;
            }
        }
    }
}
