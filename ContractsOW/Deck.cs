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
    
    public partial class Deck
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Deck()
        {
            this.CollectedCards = new HashSet<CollectedCard>();
        }
    
        public string deckName { get; set; }
        public string type { get; set; }
        public string playerName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollectedCard> CollectedCards { get; set; }
        public virtual Player Player { get; set; }
    }
}
