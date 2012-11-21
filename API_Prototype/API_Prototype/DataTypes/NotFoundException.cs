using System;

namespace APIPrototype
{
    public class NotFoundException:Exception
    {
        private string _message;

        public string message
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
            }
        }

        private System.Net.HttpStatusCode _statusCode;

        public System.Net.HttpStatusCode statusCode
        {
            get
            {
                return System.Net.HttpStatusCode.NotFound;
            }
        }

        public NotFoundException(string message)
        {
            this.message = message;
        }

    }
}
