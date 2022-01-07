using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContractsOW
{
    [DataContract]
    public class PlayerContract
    {

        [DataMember]
        public Guid id { get; set; }

        [DataMember]
        public string nickName { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public byte[] imageProfile { get; set; }
        [DataMember]
        public string state { get; set; }

        [IgnoreDataMember]
        public IPlayerServiceCallback CallbackChannel { get; set; }
    }
}
