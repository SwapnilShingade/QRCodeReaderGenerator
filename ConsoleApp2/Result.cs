namespace QRCodeReader
{
    public class Result
    {
        public string Type { get; set; }
        public Symbol[] Symbol { get; set; }
    }

    public class Symbol
    {
        public int Sequence { get; set; }
        public string Data { get; set; }

        public string Error { get; set; }

    }

    public enum ValidExtension
    {
        jpeg,
        jpg,
        png,
        gif
    }
}
