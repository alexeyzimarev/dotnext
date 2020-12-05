namespace DotNext.Lib {
    public class Checkpoint : Document {
        public Checkpoint(string id) : base(id) { }
        public long? Position { get; set; }
    }
}
