using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace Entities
{   [DataContract]
    public class EventParam
    {
        [DataMember]
        public int EventParamId { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string DBExtValue { get; set; }
        [DataMember]
        public int? DBValue { get; set; }
    }
}
