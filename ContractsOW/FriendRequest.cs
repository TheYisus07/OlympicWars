//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ContractsOW
{
    using System;
    using System.Collections.Generic;
    
    public partial class FriendRequest
    {
        public int idFriendRequest { get; set; }
        public string nicknamePlayer { get; set; }
        public string nicknameFriend { get; set; }
        public string stateRequest { get; set; }
    
        public virtual Player Player { get; set; }
    }
}