using System.Runtime.Serialization;


namespace APIPrototype
{
    /// <summary>
    /// Represents Rest Errors
    /// </summary>
    [DataContract(Name = "error")]
    public class  RestError
    {
        [DataMember(Name = "messages", Order = 1)]
        public string messages { get; set; }

        [DataMember(Name = "code", Order = 2)]
        public string code { get; set; }

        public RestError(string messages, string code)
        {
            this.messages = messages;
            this.code = code;
        }

    }
}
