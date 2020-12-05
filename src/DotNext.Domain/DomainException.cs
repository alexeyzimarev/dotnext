using System;

namespace DotNext.Domain {
    public class DomainException : Exception {
        public DomainException(string message) : base(message) { }
    }
}
