namespace cumbria.services.storage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AADUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AADUser()
        {
            AllowedGroups = new HashSet<AllowedGroup>();
        }

        [StringLength(50)]
        public string Id { get; set; }

        [Required]
        [StringLength(255)]
        public string UserDisplayName { get; set; }

        [Required]
        [StringLength(255)]
        public string UserPrincipalName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AllowedGroup> AllowedGroups { get; set; }
    }
}
