using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ContractsOW
{
    [DataContract]
    public class ProfieImageContract
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public byte[] imageProfile { get; set; }
    }
}
