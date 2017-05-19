//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MeFaltaUno.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Invitation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invitation()
        {
            this.ChatRoom = new HashSet<ChatRoom>();
            this.RatePlayer = new HashSet<RatePlayer>();
            this.Player1 = new HashSet<Player>();
        }
    
        public int IdInvitation { get; set; }
        public System.DateTime InvitationCreatedDate { get; set; }
        public System.DateTime InvitationModifiedDate { get; set; }
        public System.DateTime InvitationDate { get; set; }
        public string Comments { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChatRoom> ChatRoom { get; set; }
        public virtual InvitationStatus InvitationStatus { get; set; }
        public virtual Player Player { get; set; }
        public virtual Stadium Stadium { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RatePlayer> RatePlayer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Player> Player1 { get; set; }
    }
}