namespace DotNext.Lib {
    public record EventMetadata {
        public string ClrType       { get; set; }
        public string CorrelationId { get; set; }
        public string CausationId   { get; set; }
    }
}
