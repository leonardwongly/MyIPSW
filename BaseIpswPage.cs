using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace ipsw
{
    public class BaseIpswPage : Page
    {
        private const string DeviceListCacheKey = "DeviceListJson";
        private const string ApiErrorMessage = "Error: Could not connect to the firmware API. Please try again later.";
        private readonly Dictionary<string, Control> _controlCache = new Dictionary<string, Control>(StringComparer.Ordinal);

        protected RadioButtonList OptionsList => GetControl<RadioButtonList>("rblOptions");
        protected Label Step2Label => GetControl<Label>("lblStep2");
        protected Label Step3Label => GetControl<Label>("lblStep3");
        protected Label SelectionLabel => GetControl<Label>("lblSelection");
        protected Label SelectionCommentLabel => GetControl<Label>("lblSelectionComment");
        protected Button RetrieveButton => GetControl<Button>("btnRetrieve");
        protected PlaceHolder ErrorPlaceholder => GetControl<PlaceHolder>("phError");
        protected DropDownList IPhoneDropDown => GetControl<DropDownList>("ddliPhone");
        protected DropDownList IPadDropDown => GetControl<DropDownList>("ddliPad");
        protected DropDownList IPodDropDown => GetControl<DropDownList>("ddliPod");
        protected DropDownList WatchDropDown => GetControl<DropDownList>("ddlWatch");
        protected DropDownList AppleTvDropDown => GetControl<DropDownList>("ddlAppleTV");
        protected DropDownList MacDropDown => GetControl<DropDownList>("ddlMac");
        protected DropDownList AudioAccessoryDropDown => GetControl<DropDownList>("ddlAudioAccessory");
        protected DropDownList VersionDropDown => GetControl<DropDownList>("ddlVersion");
        protected DropDownList VersionOtaDropDown => GetControl<DropDownList>("ddlVersionOTA");
        protected Label ErrorLabel => GetControl<Label>("lblError");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializeControls();
                if (!TryPopulateDevices())
                {
                    HideSelectionControls();
                    return;
                }

                PopulateVersionDropDowns();
            }
        }

        protected virtual void ResetResultPanels()
        {
            ErrorLabel.Visible = false;
            ErrorPlaceholder.Visible = false;
            ErrorLabel.Text = string.Empty;
        }

        protected bool TryDownloadString(string url, out string result)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    result = webClient.DownloadString(url);
                    return true;
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("IPSW API Error: " + ex.Message);
                ShowApiError(ApiErrorMessage);
                result = string.Empty;
                return false;
            }
        }

        protected void rblOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetResultPanels();
            if (OptionsList.SelectedItem == null)
            {
                return;
            }

            string selectedValue = OptionsList.SelectedItem.ToString();
            bool isOfficial = selectedValue.Equals("Official", StringComparison.OrdinalIgnoreCase);
            bool isOta = selectedValue.Equals("OTA", StringComparison.OrdinalIgnoreCase);
            bool isVersion = selectedValue.Equals("Version", StringComparison.OrdinalIgnoreCase);
            bool isVersionOta = selectedValue.Equals("Version (OTA)", StringComparison.OrdinalIgnoreCase);

            Step2Label.Visible = true;
            IPhoneDropDown.Visible = isOfficial || isOta;
            IPadDropDown.Visible = isOfficial || isOta;
            IPodDropDown.Visible = isOfficial || isOta;
            WatchDropDown.Visible = isOta;
            AppleTvDropDown.Visible = isOta;
            MacDropDown.Visible = isOfficial;
            AudioAccessoryDropDown.Visible = isOta;
            VersionDropDown.Visible = isVersion;
            VersionOtaDropDown.Visible = isVersionOta;

            if (!isVersion && !isVersionOta)
            {
                ResetAllVersionSelections();
            }
            else
            {
                ResetDeviceSelections();
            }
        }

        protected void ddliPhone_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleDeviceSelection(IPhoneDropDown);
        }

        protected void ddliPad_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleDeviceSelection(IPadDropDown);
        }

        protected void ddliPod_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleDeviceSelection(IPodDropDown);
        }

        protected void ddlWatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleDeviceSelection(WatchDropDown);
        }

        protected void ddlAudioAccessory_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleDeviceSelection(AudioAccessoryDropDown);
        }

        protected void ddlAppleTV_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleDeviceSelection(AppleTvDropDown);
        }

        protected void ddlMac_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleDeviceSelection(MacDropDown);
        }

        protected void ddlVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetResultPanels();
            SelectionLabel.Text = HttpUtility.HtmlEncode(VersionDropDown.SelectedValue);
            SelectionLabel.Font.Bold = true;
            SelectionLabel.Font.Size = FontUnit.Point(15);

            string safeSelection = HttpUtility.HtmlEncode(OptionsList.SelectedItem?.ToString() ?? string.Empty);
            string safeVersion = HttpUtility.HtmlEncode(VersionDropDown.SelectedItem?.ToString() ?? string.Empty);
            SelectionCommentLabel.Text = $"You have selected iOS <b>{safeSelection} {safeVersion}</b>";

            RetrieveButton.Text = "Retrieve";
            RetrieveButton.Visible = true;

            Step3Label.Visible = true;
            Step3Label.Font.Bold = true;

            Step2Label.Visible = true;
            IPhoneDropDown.Visible = false;
            IPadDropDown.Visible = false;
            IPodDropDown.Visible = false;
            WatchDropDown.Visible = false;
            AppleTvDropDown.Visible = false;
            MacDropDown.Visible = false;
            AudioAccessoryDropDown.Visible = false;
            VersionDropDown.Visible = true;
            VersionOtaDropDown.Visible = false;
        }

        protected void ddlVersionOTA_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetResultPanels();
            SelectionLabel.Text = HttpUtility.HtmlEncode(VersionOtaDropDown.SelectedValue);
            SelectionLabel.Font.Bold = true;
            SelectionLabel.Font.Size = FontUnit.Point(15);

            string safeSelection = HttpUtility.HtmlEncode(OptionsList.SelectedItem?.ToString() ?? string.Empty);
            string safeVersion = HttpUtility.HtmlEncode(VersionOtaDropDown.SelectedItem?.ToString() ?? string.Empty);
            SelectionCommentLabel.Text = $"You have selected OTA <b>{safeSelection} {safeVersion}</b>";

            RetrieveButton.Text = "Retrieve";
            RetrieveButton.Visible = true;

            Step3Label.Visible = true;
            Step3Label.Font.Bold = true;

            Step2Label.Visible = true;
            IPhoneDropDown.Visible = false;
            IPadDropDown.Visible = false;
            IPodDropDown.Visible = false;
            WatchDropDown.Visible = false;
            AppleTvDropDown.Visible = false;
            MacDropDown.Visible = false;
            AudioAccessoryDropDown.Visible = false;
            VersionDropDown.Visible = false;
            VersionOtaDropDown.Visible = true;
        }

        private void InitializeControls()
        {
            RetrieveButton.Visible = false;
            Step2Label.Visible = false;
            Step3Label.Visible = false;
            IPhoneDropDown.Visible = false;
            IPadDropDown.Visible = false;
            IPodDropDown.Visible = false;
            WatchDropDown.Visible = false;
            AppleTvDropDown.Visible = false;
            MacDropDown.Visible = false;
            AudioAccessoryDropDown.Visible = false;
            VersionDropDown.Visible = false;
            VersionOtaDropDown.Visible = false;
            SelectionLabel.Text = string.Empty;
            SelectionCommentLabel.Text = string.Empty;
            ErrorLabel.Visible = false;
            ErrorPlaceholder.Visible = false;
            ErrorLabel.Text = string.Empty;
            OptionsList.ClearSelection();
        }

        private bool TryPopulateDevices()
        {
            dynamic jsonObj = HttpRuntime.Cache.Get(DeviceListCacheKey);
            if (jsonObj == null)
            {
                if (!TryDownloadString("https://api.ipsw.me/v4/devices", out string jsonResponse))
                {
                    return false;
                }

                jsonObj = JsonConvert.DeserializeObject(jsonResponse);
                HttpRuntime.Cache.Insert(DeviceListCacheKey, jsonObj, null, DateTime.Now.AddHours(12), Cache.NoSlidingExpiration);
            }

            PopulateDeviceDropDowns(jsonObj);
            return true;
        }

        private void PopulateDeviceDropDowns(dynamic jsonObj)
        {
            ResetDeviceDropDown(IPhoneDropDown);
            ResetDeviceDropDown(IPadDropDown);
            ResetDeviceDropDown(IPodDropDown);
            ResetDeviceDropDown(WatchDropDown);
            ResetDeviceDropDown(AppleTvDropDown);
            ResetDeviceDropDown(MacDropDown);
            ResetDeviceDropDown(AudioAccessoryDropDown);

            for (int i = 0; i < jsonObj.Count; i++)
            {
                string name = jsonObj[i]["name"].ToString();
                string identifier = jsonObj[i]["identifier"].ToString();
                ListItem listItem = new ListItem(name, identifier);

                if (identifier.Contains("iPhone"))
                {
                    IPhoneDropDown.Items.Add(listItem);
                }
                else if (identifier.Contains("iPad"))
                {
                    IPadDropDown.Items.Add(listItem);
                }
                else if (identifier.Contains("Watch"))
                {
                    WatchDropDown.Items.Add(listItem);
                }
                else if (identifier.Contains("AudioAccessory"))
                {
                    AudioAccessoryDropDown.Items.Add(listItem);
                }
                else if (identifier.Contains("iPod"))
                {
                    IPodDropDown.Items.Add(listItem);
                }
                else if (identifier.Contains("AppleTV"))
                {
                    AppleTvDropDown.Items.Add(listItem);
                }
                else if (identifier.Contains("Mac"))
                {
                    MacDropDown.Items.Add(listItem);
                }
            }
        }

        private void PopulateVersionDropDowns()
        {
            PopulateVersionDropDown(VersionDropDown, "IosVersions", "18.0");
            PopulateVersionDropDown(VersionOtaDropDown, "OtaVersions", "14.0");
        }

        private void PopulateVersionDropDown(DropDownList dropDown, string settingKey, string defaultValue)
        {
            string versionsValue = ConfigurationManager.AppSettings[settingKey] ?? defaultValue;
            string[] versions = versionsValue
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim())
                .ToArray();

            Array.Reverse(versions);

            ListItem placeholder = null;
            if (dropDown.Items.Count > 0)
            {
                placeholder = new ListItem(dropDown.Items[0].Text, dropDown.Items[0].Value)
                {
                    Selected = true
                };
            }

            dropDown.Items.Clear();
            if (placeholder != null)
            {
                dropDown.Items.Add(placeholder);
            }

            foreach (string version in versions)
            {
                dropDown.Items.Add(new ListItem(version, version));
            }
        }

        private void HandleDeviceSelection(DropDownList activeDropDown)
        {
            ResetResultPanels();
            if (OptionsList.SelectedItem == null || activeDropDown.SelectedIndex <= 0)
            {
                return;
            }

            ResetOtherDropDowns(activeDropDown);

            SelectionLabel.Text = HttpUtility.HtmlEncode(activeDropDown.SelectedValue);
            SelectionLabel.Font.Bold = true;
            SelectionLabel.Font.Size = FontUnit.Point(15);

            string selectionText = OptionsList.SelectedItem?.ToString() ?? string.Empty;
            string safeSelection = HttpUtility.HtmlEncode(selectionText);
            string safeDevice = HttpUtility.HtmlEncode(activeDropDown.SelectedItem?.ToString() ?? string.Empty);
            SelectionCommentLabel.Text = $"You have selected <b>{safeSelection} {safeDevice}</b> with the identifier: ";

            RetrieveButton.Text = "Retrieve";
            RetrieveButton.Visible = true;

            Step3Label.Visible = true;
            Step3Label.Font.Bold = true;

            if (selectionText.Equals("Official", StringComparison.OrdinalIgnoreCase))
            {
                Step2Label.Visible = true;
                IPhoneDropDown.Visible = true;
                IPadDropDown.Visible = true;
                IPodDropDown.Visible = true;
                WatchDropDown.Visible = false;
                AppleTvDropDown.Visible = false;
                MacDropDown.Visible = true;
                AudioAccessoryDropDown.Visible = false;
                VersionDropDown.Visible = false;
                VersionOtaDropDown.Visible = false;
            }
            else if (selectionText.Equals("OTA", StringComparison.OrdinalIgnoreCase))
            {
                Step2Label.Visible = true;
                IPhoneDropDown.Visible = true;
                IPadDropDown.Visible = true;
                IPodDropDown.Visible = true;
                WatchDropDown.Visible = true;
                AppleTvDropDown.Visible = true;
                MacDropDown.Visible = false;
                AudioAccessoryDropDown.Visible = true;
                VersionDropDown.Visible = false;
                VersionOtaDropDown.Visible = false;
            }
        }

        private void ResetDeviceDropDown(DropDownList dropDown)
        {
            if (dropDown.Items.Count > 0)
            {
                string placeholderText = dropDown.Items[0].Text;
                string placeholderValue = dropDown.Items[0].Value;
                dropDown.Items.Clear();
                dropDown.Items.Add(new ListItem(placeholderText, placeholderValue)
                {
                    Selected = true
                });
            }
            else
            {
                dropDown.Items.Clear();
            }
        }

        private void ResetOtherDropDowns(DropDownList activeDropDown)
        {
            List<DropDownList> dropDowns = new List<DropDownList>
            {
                IPhoneDropDown,
                IPadDropDown,
                IPodDropDown,
                WatchDropDown,
                AppleTvDropDown,
                MacDropDown,
                AudioAccessoryDropDown
            };

            foreach (DropDownList dropDown in dropDowns)
            {
                if (!ReferenceEquals(dropDown, activeDropDown) && dropDown.Items.Count > 0)
                {
                    dropDown.SelectedIndex = 0;
                }
            }
        }

        private void ResetAllVersionSelections()
        {
            if (VersionDropDown.Items.Count > 0)
            {
                VersionDropDown.SelectedIndex = 0;
            }

            if (VersionOtaDropDown.Items.Count > 0)
            {
                VersionOtaDropDown.SelectedIndex = 0;
            }
        }

        private void ResetDeviceSelections()
        {
            ResetDropDownSelection(IPhoneDropDown);
            ResetDropDownSelection(IPadDropDown);
            ResetDropDownSelection(IPodDropDown);
            ResetDropDownSelection(WatchDropDown);
            ResetDropDownSelection(AppleTvDropDown);
            ResetDropDownSelection(MacDropDown);
            ResetDropDownSelection(AudioAccessoryDropDown);
        }

        private void ResetDropDownSelection(DropDownList dropDown)
        {
            if (dropDown.Items.Count > 0)
            {
                dropDown.SelectedIndex = 0;
            }
        }

        private void ShowApiError(string message)
        {
            ErrorLabel.Text = message;
            ErrorLabel.Visible = true;
            ErrorPlaceholder.Visible = true;
        }

        private void HideSelectionControls()
        {
            OptionsList.Visible = false;
            Step2Label.Visible = false;
            Step3Label.Visible = false;
            IPhoneDropDown.Visible = false;
            IPadDropDown.Visible = false;
            IPodDropDown.Visible = false;
            WatchDropDown.Visible = false;
            AppleTvDropDown.Visible = false;
            MacDropDown.Visible = false;
            AudioAccessoryDropDown.Visible = false;
            VersionDropDown.Visible = false;
            VersionOtaDropDown.Visible = false;
            RetrieveButton.Visible = false;
            SelectionLabel.Visible = false;
            SelectionCommentLabel.Visible = false;
        }

        private TControl GetControl<TControl>(string id) where TControl : Control
        {
            if (_controlCache.TryGetValue(id, out Control cachedControl))
            {
                return cachedControl as TControl;
            }

            Control control = FindControlRecursive(this, id);
            if (control == null)
            {
                throw new InvalidOperationException($"Unable to locate control with ID '{id}'.");
            }

            _controlCache[id] = control;
            return control as TControl;
        }

        private Control FindControlRecursive(Control root, string id)
        {
            if (root == null)
            {
                return null;
            }

            Control control = root.FindControl(id);
            if (control != null)
            {
                return control;
            }

            foreach (Control child in root.Controls)
            {
                Control result = FindControlRecursive(child, id);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
