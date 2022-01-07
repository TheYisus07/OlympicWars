using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContractsOW
{
    [DataContract]
    public class FriendContract
    {
        [DataMember]
        public int idFriend { get; set; }

        [DataMember]
        public string nicknamePlayer { get; set; }

        [DataMember]
        public string nicknameFriend { get; set; }

        [DataMember]
        public string stateRequest { get; set; }

    }
}
