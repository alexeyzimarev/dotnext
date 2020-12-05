using System.Collections.Generic;

// ReSharper disable ReturnTypeCanBeEnumerable.Global

namespace DotNext.Lib {
    public abstract class Aggregate {
        protected void Apply(object evt) {
            _changes.Add(evt);
            When(evt);
        }
        
        public void Load(IEnumerable<object> events) {
            foreach (var @event in events) {
                _existing.Add(@event);
                When(@event);
                Version++;
            }
        }

        protected abstract void When(object evt);

        public IReadOnlyCollection<object> Changes => _changes.AsReadOnly();

        protected IReadOnlyCollection<object> Existing => _existing.AsReadOnly();

        public void ClearChanges() => _changes.Clear();

        public abstract string GetId();

        public int Version { get; private set; } = -1;

        readonly List<object> _existing = new();
        readonly List<object> _changes = new();
    }
}
