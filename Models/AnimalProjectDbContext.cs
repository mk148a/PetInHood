using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;

namespace AnimalProtect.Models
{
    public partial class AnimalProjectDbContext : DbContext
    {
        public AnimalProjectDbContext()
        {
        }

        public AnimalProjectDbContext(DbContextOptions<AnimalProjectDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<CampaignPost> CampaignPosts { get; set; }
        public virtual DbSet<CommentPost> CommentPosts { get; set; }
        public virtual DbSet<Image> Images { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=AnimalProjectDb;User ID=sa;Password=1234567890aA;Min Pool Size=5;Max Pool Size=300;Pooling=true;;Min Pool Size=5;Max Pool Size=300;Pooling=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");
          
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.username);

                entity.ToTable("users");

                entity.Property(e => e.username)
                    .HasColumnName("username")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.id)
                    .HasColumnName("id")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.mobileId)
                    .HasColumnName("mobileId")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.eMailAddress)
                   .IsRequired()
                   .HasColumnName("email")
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.badge)
                    .HasColumnName("badge")                  
                  .IsUnicode(false);
                entity.Property(e => e.firebaseToken)                
                  .HasColumnName("firebaseToken")
                  .IsUnicode(false);
                entity.Property(e => e.followers)
                    .HasConversion(v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries))
                 .HasColumnName("followers")
                 .IsUnicode(false);
                entity.Property(e => e.following)
                    .HasConversion(v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries))
               .HasColumnName("following")
               .IsUnicode(false);
                entity.Property(e => e.petCoin)
           
           .HasColumnName("petCoin")
           .IsUnicode(false);
                entity.Property(e => e.postIds)
                    .HasConversion(v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries))
         .HasColumnName("postIds")
         .IsUnicode(false);
                entity.Property(e => e.profilePhoto)
                    .HasColumnName("profilePhoto")
  .IsUnicode(false);
            });
            modelBuilder.Entity<CampaignPost>(entity =>
            {

                entity.HasKey(e => e.id);

                entity.ToTable("CampaignPosts");

                entity.Property(e => e.createdAt)
                    .HasColumnName("createdAt")
                  ;
                entity.Property(e => e.commentsCount)
                  .HasColumnName("commentsCount")
                ;

                entity.Property(e => e.creatorUsername)
                    .HasColumnName("creatorUsername")
                    ;

                entity.Property(e => e.description)
                    .HasColumnName("description")
                    .HasMaxLength(4000)
                  ;

                entity.Property(e => e.donatedMoney)                   
                    .HasColumnName("donatedMoney")
                    ;
                entity.Property(e => e.id)
                   .IsRequired()
                   .HasColumnName("id")
                  ;
                entity.Property(e => e.isDonationactive)                
                  .HasColumnName("isDonationactive")
                 ;
                entity.Property(e => e.likedUserscount)                 
                  .HasColumnName("likedUserscount")
                 ;
                entity.Property(e => e.offeredMoney)               
                  .HasColumnName("offeredMoney")
                 ;
                entity.Property(e => e.photoOfcreator)
                    .HasColumnName("photoOfcreator")
               ;
                entity.Property(e => e.photos)
                    .HasConversion(v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries))
                 .IsUnicode(false)
                .HasColumnName("photos")
                ;
                entity.Property(e => e.tag)
                    .HasMaxLength(50)
                .HasColumnName("tag")
               ;
                entity.Property(e => e.location)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("location")
               ;

            });
            modelBuilder.Entity<CommentPost>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                  .IsRequired()
                  .HasColumnName("id")
                 ;

                entity.ToTable("CommentPosts");

                entity.Property(e => e.createdAt)
                    .HasColumnName("createdAt")
                  ;
                entity.Property(e => e.campaignPostid)
                  .HasColumnName("campaignPostid")
                ;

                entity.Property(e => e.creatorUsername)
                    .HasColumnName("creatorUsername")
                    ;

                entity.Property(e => e.description)
                    .HasColumnName("description")
                    .HasMaxLength(4000)
                  ;

                entity.Property(e => e.likedUserscount)
                    .HasColumnName("likedUserscount")
                    ;
                entity.Property(e => e.mentionedId)
                   .IsRequired()
                   .HasColumnName("mentionedId")
                  ;     
            });
            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.id)
                    .IsRequired()
                    .HasColumnName("Id")
                    ;

                entity.ToTable("Images");

                entity.Property(e => e.path)
                    .HasColumnName("Path")
                    ;
                entity.Property(e => e.sha)
                    .HasColumnName("Sha")
                    ;
            });
        }
    }
}
