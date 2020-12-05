namespace DotNext.Lib {
    public abstract class Document {
        protected Document() { }

        protected Document(string id) => Id = id;

        public string Id { get; set; }
    }
}
