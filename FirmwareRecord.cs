namespace ipsw
{
    public sealed class FirmwareRecord
    {
        public string Identifier { get; set; }

        public string BuildId { get; set; }

        public string Url { get; set; }

        public string FileName { get; set; }

        public double FileSizeBytes { get; set; }

        public string ReleaseDateText { get; set; }

        public bool? IsSigned { get; set; }

        public FirmwareRecord()
        {
            Identifier = string.Empty;
            BuildId = string.Empty;
            Url = string.Empty;
            FileName = string.Empty;
            ReleaseDateText = "-";
        }
    }
}
