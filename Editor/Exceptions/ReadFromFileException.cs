using System;

namespace Editor.Exceptions
{
    public class ReadFromFileException : Exception
    {
        public ReadFromFileException()
        {

        }
        public ReadFromFileException(string message) : base(message) { }
    }
}
