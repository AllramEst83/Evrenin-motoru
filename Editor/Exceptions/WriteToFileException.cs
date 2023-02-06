using System;

namespace Editor.Exceptions
{
    public class WriteToFileException : Exception
    {
        public WriteToFileException() { }

        public WriteToFileException(string message) : base(message) { }
    }
}
