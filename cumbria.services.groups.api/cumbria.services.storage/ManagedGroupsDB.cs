namespace cumbria.services.storage
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ManagedGroupsDB : DbContext
    {
        public ManagedGroupsDB()
            : base("name=ManagedGroupsDB")
        {
        }

        public virtual DbSet<AADUser> AADUsers { get; set; }
        public virtual DbSet<AllowedGroup> AllowedGroups { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AADUser>()
                .HasMany(e => e.AllowedGroups)
                .WithMany(e => e.AADUsers)
                .Map(m => m.ToTable("GroupMemberships").MapLeftKey("UserId").MapRightKey("GroupId"));

            modelBuilder.Entity<Category>()
                .HasMany(e => e.AllowedGroups)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);
        }
    }
}
