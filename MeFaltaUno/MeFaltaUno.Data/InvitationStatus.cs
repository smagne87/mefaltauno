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
    
    public partial class InvitationStatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvitationStatus()
        {
            this.Invitation = new HashSet<Invitation>();
        }
    
        public int IdInvitationStatus { get; set; }
        public string InvitationStatusName { get; set; }
        public string InvitationStatusDescription { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invitation> Invitation { get; set; }
    }
}
