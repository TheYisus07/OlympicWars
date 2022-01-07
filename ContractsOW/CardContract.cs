using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContractsOW
{
    [DataContract]
    public class CardContract
    {
        [DataMember]
        public string nameCard { get; set; }
        [DataMember]
        public string attribute { get; set; }
        [DataMember]
        public int damage { get; set; }
        [DataMember]
        public byte[] image { get; set; }
        [DataMember]
        public byte[] attributeImage { get; set; }
    }
}