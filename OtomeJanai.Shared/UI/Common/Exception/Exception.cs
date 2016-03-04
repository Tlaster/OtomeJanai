using System;
using System.Collections.Generic;
using System.Text;

namespace OtomeJanai.Shared.UI.Common.Exception
{
    public class InvalidElementException : System.Exception
    {
        public InvalidElementException() { }
        public InvalidElementException(string message) : base(message) { }
        public InvalidElementException(string message, System.Exception inner) : base(message, inner) { }
    }
    
    public class InvalidPropertyException : System.Exception
    {
        public InvalidPropertyException() { }
        public InvalidPropertyException(string message) : base(message) { }
        public InvalidPropertyException(string message, System.Exception inner) : base(message, inner) { }
    }

    public class UnsupportedPropertyTypeException : System.Exception
    {
        public UnsupportedPropertyTypeException() { }
        public UnsupportedPropertyTypeException(string message) : base(message) { }
        public UnsupportedPropertyTypeException(string message, System.Exception inner) : base(message, inner) { }
    }
    
    public class InvalidPropertyValueException : System.Exception
    {
        public InvalidPropertyValueException() { }
        public InvalidPropertyValueException(string message) : base(message) { }
        public InvalidPropertyValueException(string message, System.Exception inner) : base(message, inner) { }
    }
    
    public class InvalidColorNameException : System.Exception
    {
        public InvalidColorNameException() { }
        public InvalidColorNameException(string message) : base(message) { }
        public InvalidColorNameException(string message, System.Exception inner) : base(message, inner) { }
    }

}
