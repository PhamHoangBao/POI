using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace POI.repository.Entities
{
    public partial class POIContext : DbContext
    {
        public POIContext()
        {
        }

        public POIContext(DbContextOptions<POIContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<DesHashtag> DesHashtags { get; set; }
        public virtual DbSet<Destination> Destinations { get; set; }
        public virtual DbSet<DestinationType> DestinationTypes { get; set; }
        public virtual DbSet<Hashtag> Hashtags { get; set; }
        public virtual DbSet<Poi> Pois { get; set; }
        public virtual DbSet<Poiblog> Poiblogs { get; set; }
        public virtual DbSet<Poitype> Poitypes { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Trip> Trips { get; set; }
        public virtual DbSet<TripDestination> TripDestinations { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Visit> Visits { get; set; }
        public virtual DbSet<Vote> Votes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-U0JF1V7J\\HOANGBAO;User ID=sa;Password=hoangbao;Initial Catalog=POI");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("Blog");

                entity.Property(e => e.BlogId)
                    .HasColumnName("BlogID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Content).IsRequired();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Blog_User");
            });

            modelBuilder.Entity<DesHashtag>(entity =>
            {
                entity.ToTable("DesHashtag");

                entity.Property(e => e.DesHashtagId)
                    .HasColumnName("DesHashtagID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.DestinationId).HasColumnName("DestinationID");

                entity.Property(e => e.HashtagId).HasColumnName("HashtagID");

                entity.HasOne(d => d.Destination)
                    .WithMany(p => p.DesHashtags)
                    .HasForeignKey(d => d.DestinationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DesHashtag_Destination");

                entity.HasOne(d => d.Hashtag)
                    .WithMany(p => p.DesHashtags)
                    .HasForeignKey(d => d.HashtagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DesHashtag_Hashtag");
            });

            modelBuilder.Entity<Destination>(entity =>
            {
                entity.ToTable("Destination");

                entity.Property(e => e.DestinationId)
                    .HasColumnName("DestinationID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Coordinate)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.DestinationName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.DestinationTypeId).HasColumnName("DestinationTypeID");

                entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");

                entity.HasOne(d => d.DestinationType)
                    .WithMany(p => p.Destinations)
                    .HasForeignKey(d => d.DestinationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Destination_DestinationType");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Destinations)
                    .HasForeignKey(d => d.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Destination_Province");
            });

            modelBuilder.Entity<DestinationType>(entity =>
            {
                entity.ToTable("DestinationType");

                entity.Property(e => e.DestinationTypeId)
                    .HasColumnName("DestinationTypeID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Hashtag>(entity =>
            {
                entity.ToTable("Hashtag");

                entity.Property(e => e.HashtagId)
                    .HasColumnName("HashtagID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ShortName).HasMaxLength(200);
            });

            modelBuilder.Entity<Poi>(entity =>
            {
                entity.ToTable("POI");

                entity.Property(e => e.PoiId)
                    .HasColumnName("PoiID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Coordinate)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.DestinationId).HasColumnName("DestinationID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PoiTypeId).HasColumnName("PoiTypeID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.PoiType)
                    .WithMany(p => p.Pois)
                    .HasForeignKey(d => d.PoiTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POI_POIType");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Pois)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_POI_User");
            });

            modelBuilder.Entity<Poiblog>(entity =>
            {
                entity.ToTable("POIBlog");

                entity.Property(e => e.PoiblogId)
                    .HasColumnName("POIBlogID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.BlogId).HasColumnName("BlogID");

                entity.Property(e => e.PoiId).HasColumnName("PoiID");

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.Poiblogs)
                    .HasForeignKey(d => d.BlogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POIBlog_Blog");

                entity.HasOne(d => d.Poi)
                    .WithMany(p => p.Poiblogs)
                    .HasForeignKey(d => d.PoiId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POIBlog_POI");
            });

            modelBuilder.Entity<Poitype>(entity =>
            {
                entity.ToTable("POIType");

                entity.Property(e => e.PoitypeId)
                    .HasColumnName("POITypeID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.ToTable("Province");

                entity.Property(e => e.ProvinceId)
                    .HasColumnName("ProvinceID")
                    .HasDefaultValueSql("(newid())"); ;

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.ToTable("Trip");

                entity.Property(e => e.TripId)
                    .HasColumnName("TripID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.TripName)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Trips)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trip_User");
            });

            modelBuilder.Entity<TripDestination>(entity =>
            {
                entity.ToTable("TripDestination");

                entity.Property(e => e.TripDestinationId)
                    .HasColumnName("TripDestinationID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.DestinationId).HasColumnName("DestinationID");

                entity.Property(e => e.TripId).HasColumnName("TripID");

                entity.HasOne(d => d.Destination)
                    .WithMany(p => p.TripDestinations)
                    .HasForeignKey(d => d.DestinationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TripDestination_Destination");

                entity.HasOne(d => d.Trip)
                    .WithMany(p => p.TripDestinations)
                    .HasForeignKey(d => d.TripId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TripDestination_Trip");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Role");
            });

            modelBuilder.Entity<Visit>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Visit");

                entity.Property(e => e.PoiId).HasColumnName("PoiID");

                entity.Property(e => e.TripId).HasColumnName("TripID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.VisitDate).HasColumnType("datetime");

                entity.Property(e => e.VisitId)
                    .HasColumnName("VisitID")
                    .HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Trip)
                    .WithMany()
                    .HasForeignKey(d => d.TripId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Visit_Trip");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Visit_User");
            });

            modelBuilder.Entity<Vote>(entity =>
            {
                entity.ToTable("Vote");

                entity.Property(e => e.VoteId)
                    .HasColumnName("VoteID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.BlogId).HasColumnName("BlogID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.Votes)
                    .HasForeignKey(d => d.BlogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vote_Blog");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Votes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vote_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
