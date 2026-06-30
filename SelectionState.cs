using System;

namespace ipsw
{
    public enum FirmwareSource
    {
        Official,
        Ota,
        Version,
        VersionOta
    }

    public enum ResultMode
    {
        Table,
        Links
    }

    public sealed class SelectionState
    {
        public FirmwareSource? Source { get; set; }

        public string DeviceIdentifier { get; set; }

        public string Version { get; set; }

        public ResultMode ResultMode { get; set; }

        public bool IsUiNext { get; set; }

        public SelectionState()
        {
            ResultMode = ResultMode.Table;
            DeviceIdentifier = string.Empty;
            Version = string.Empty;
        }

        public bool HasTargetSelection
        {
            get
            {
                if (!Source.HasValue)
                {
                    return false;
                }

                if (Source.Value == FirmwareSource.Version || Source.Value == FirmwareSource.VersionOta)
                {
                    return !string.IsNullOrWhiteSpace(Version);
                }

                return !string.IsNullOrWhiteSpace(DeviceIdentifier);
            }
        }

        public bool IsReadyToRetrieve
        {
            get
            {
                return Source.HasValue && HasTargetSelection;
            }
        }

        public string ToSourceQueryValue()
        {
            if (!Source.HasValue)
            {
                return string.Empty;
            }

            switch (Source.Value)
            {
                case FirmwareSource.Official:
                    return "official";
                case FirmwareSource.Ota:
                    return "ota";
                case FirmwareSource.Version:
                    return "version";
                case FirmwareSource.VersionOta:
                    return "version_ota";
                default:
                    return string.Empty;
            }
        }

        public string ToResultModeQueryValue()
        {
            return ResultMode == ResultMode.Links ? "links" : "table";
        }
    }

    public sealed class SelectionValidationResult
    {
        public SelectionValidationResult(SelectionState state)
        {
            State = state ?? throw new ArgumentNullException(nameof(state));
            Warnings = new System.Collections.Generic.List<string>();
        }

        public SelectionState State { get; }

        public System.Collections.Generic.List<string> Warnings { get; }
    }
}
