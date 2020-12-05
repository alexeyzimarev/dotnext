using System;

namespace DotNext.Application {
    public class ApplicationException : Exception {
        public ApplicationException(string message) : base(message) { }
    }
}
