using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace APIPrototype
{
    /// <summary>
    /// Represents ResultSet
    /// </summary>
    [DataContract(Name = "result")]
    public class Result
    {
        [DataMember(Name = "tenantid", Order = 1)]
        public long tenantId { get; set; }

        [DataMember(Name = "resultset", Order = 2, IsRequired = false,EmitDefaultValue=false)]
        public Object resultSet { get; set; }

        [DataMember(Name = "error", Order = 3, IsRequired = false, EmitDefaultValue = false)]
        public RestError error { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Result()
        {
        }
    }
}
