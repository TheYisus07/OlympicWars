using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContractsOW
{
    [DataContract]
    public class GameContract
    {
        [DataMember]
        public int idGame { get; set; }

        [DataMember]
        public string playerName { get; set; }

        [DataMember]
        public string duration { get; set; }

        [DataMember]
        public DateTime dateGame { get; set; }

        [DataMember]
        public string stateGame { get; set; }

        [DataMember]
        public string player1 { get; set; }

        [DataMember]
        public string statePlayer1 { get; set; }

        [DataMember]
        public string player2 { get; set; }

        [DataMember]
        public string statePlayer2 { get; set; }

    }
}
