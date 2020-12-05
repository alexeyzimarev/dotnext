namespace DotNext.Domain {
    public record RoomId {
        public string Value { get; }

        public RoomId(string value) {
            if (string.IsNullOrWhiteSpace(value)) throw new DomainException("Invalid room id");

            Value = value;
        }

        public override string ToString() => Value;
    }
}
