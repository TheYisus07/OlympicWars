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
    
    public partial class CollectedCard
    {
        public int idCollectedCard { get; set; }
        public string nameDeck { get; set; }
        public string nameCard { get; set; }
    
        public virtual Card Card { get; set; }
        public virtual Deck Deck { get; set; }
    }
}
