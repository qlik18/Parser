using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace Entities
{
    [DataContract]
    public class EventParamModeler
    {
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public int EventParamId { get; set; }
        [DataMember]
        public int EventId { get; set; }
        [DataMember]
        public int ParamGroupId { get; set; }
        [DataMember]
        public int Width { get; set; }
        [DataMember]
        public int Height { get; set; }
        [DataMember]
        public int Top { get; set; }
        [DataMember]
        public int Left { get; set; }
        [DataMember]
        public int LabelTop { get; set; }
        [DataMember]
        public int LabelLeft { get; set; }
        [DataMember]
        public String Details { get; set; }
        [DataMember]
        public String TechName { get; set; }
        [DataMember]
        public String FriendlyName { get; set; }
        [DataMember]
        public bool isRequired { get; set; }
        [DataMember]
        public int BoundEventParamId { get; set; }
        [DataMember]
        public EventParam BoundEventParam { get; set; }
        [DataMember]
        public int SoudceId { get; set; }
        [DataMember]
        public String DSName { get; set; } 
        [DataMember]
        public String Content { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public String DBTypeName { get; set; }
       
    }
}
