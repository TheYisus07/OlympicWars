using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContractsOW
{
    [DataContract]
    public class DeckContract
    {
        [DataMember]
        public string deckName { get; set; }

        [DataMember]
        public string type { get; set; }

        [DataMember]
        public string playerName { get; set; }
    }
}
