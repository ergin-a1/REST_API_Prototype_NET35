using System;

namespace APIPrototype
{
    public class TenantNotFoundException:Exception
    {
        private string _message;

        private System.Net.HttpStatusCode _statusCode;

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

        public System.Net.HttpStatusCode statusCode
        {
            get
            {
                return System.Net.HttpStatusCode.NotFound;
            }
        }

        public TenantNotFoundException(string message)
        {
            this.message = message;
        }

    }
}
