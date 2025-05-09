﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SNM.LinkedIn.Infrastructure.DataContext;

#nullable disable

namespace SNM.LinkedIn.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231013091032_CreateLinkedin")]
    partial class CreateLinkedin
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("LinkedInPost", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("LinkedInChannelId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Message")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("PostId")
                        .HasColumnType("char(36)");

                    b.Property<string>("PostUrn")
                        .HasColumnType("longtext");

                    b.Property<int>("PublicationStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("ScheduleTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("LinkedInChannelId");

                    b.HasIndex("PostId");

                    b.ToTable("LinkedInPost");
                });

            modelBuilder.Entity("SNM.LinkedIn.Domain.Entities.Brand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CoverPhoto")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("DisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("Photo")
                        .HasColumnType("longtext");

                    b.Property<string>("TimeZone")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Brand");
                });

            modelBuilder.Entity("SNM.LinkedIn.Domain.Entities.Channel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("BrandId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ChannelProfileId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ChannelTypeId")
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("Photo")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("ChannelProfileId");

                    b.HasIndex("ChannelTypeId");

                    b.ToTable("Channel");
                });

            modelBuilder.Entity("SNM.LinkedIn.Domain.Entities.ChannelProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("AccessToken")
                        .HasColumnType("longtext");

                    b.Property<string>("CoverPhoto")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Headline")
                        .HasColumnType("longtext");

                    b.Property<string>("Icon")
                        .HasColumnType("longtext");

                    b.Property<string>("ProfileLink")
                        .HasColumnType("longtext");

                    b.Property<string>("ProfileUserId")
                        .HasColumnType("longtext");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("longtext");

                    b.Property<string>("RefreshTokenExpiresIn")
                        .HasColumnType("longtext");

                    b.Property<string>("Scope")
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.Property<string>("expires_in")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("ChannelProfile");
                });

            modelBuilder.Entity("SNM.LinkedIn.Domain.Entities.ChannelType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Icon")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("ChannelType");
                });

            modelBuilder.Entity("SNM.LinkedIn.Domain.Entities.LinkedInChannel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("AccessToken")
                        .HasColumnType("longtext");

                    b.Property<string>("Author_urn")
                        .HasColumnType("longtext");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("char(36)");

                    b.Property<string>("ClientId")
                        .HasColumnType("longtext");

                    b.Property<string>("ClientSecret")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("LinkedInChannel");
                });

            modelBuilder.Entity("SNM.LinkedIn.Domain.Entities.LinkedInProfileData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CoverPhoto")
                        .HasColumnType("longtext");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext");

                    b.Property<string>("Headline")
                        .HasColumnType("longtext");

                    b.Property<string>("LinkedInUserId")
                        .HasColumnType("longtext");

                    b.Property<string>("LinkedinProfileLink")
                        .HasColumnType("longtext");

                    b.Property<string>("LinkedinUrn")
                        .HasColumnType("longtext");

                    b.Property<string>("access_token")
                        .HasColumnType("longtext");

                    b.Property<string>("expires_in")
                        .HasColumnType("longtext");

                    b.Property<string>("refresh_token")
                        .HasColumnType("longtext");

                    b.Property<string>("refresh_token_expires_in")
                        .HasColumnType("longtext");

                    b.Property<string>("scope")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("LinkedInProfileData");
                });

            modelBuilder.Entity("SNM.LinkedIn.Domain.Entities.Media", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int?>("Media_type")
                        .HasColumnType("int");

                    b.Property<string>("Media_url")
                        .HasColumnType("longtext");

                    b.Property<Guid>("PostId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("Media");
                });

            modelBuilder.Entity("SNM.LinkedIn.Domain.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Caption")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("PublicationDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("LinkedInPost", b =>
                {
                    b.HasOne("SNM.LinkedIn.Domain.Entities.LinkedInChannel", "LinkedInChannel")
                        .WithMany()
                        .HasForeignKey("LinkedInChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SNM.LinkedIn.Domain.Entities.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId");

                    b.Navigation("LinkedInChannel");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("SNM.LinkedIn.Domain.Entities.Channel", b =>
                {
                    b.HasOne("SNM.LinkedIn.Domain.Entities.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SNM.LinkedIn.Domain.Entities.ChannelProfile", "ChannelProfile")
                        .WithMany("SocialChannels")
                        .HasForeignKey("ChannelProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SNM.LinkedIn.Domain.Entities.ChannelType", "ChannelType")
                        .WithMany()
                        .HasForeignKey("ChannelTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("ChannelProfile");

                    b.Navigation("ChannelType");
                });

            modelBuilder.Entity("SNM.LinkedIn.Domain.Entities.LinkedInChannel", b =>
                {
                    b.HasOne("SNM.LinkedIn.Domain.Entities.Channel", "SocialChannel")
                        .WithMany()
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SocialChannel");
                });

            modelBuilder.Entity("SNM.LinkedIn.Domain.Entities.Media", b =>
                {
                    b.HasOne("SNM.LinkedIn.Domain.Entities.Post", "Post")
                        .WithMany("MediaData")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("SNM.LinkedIn.Domain.Entities.ChannelProfile", b =>
                {
                    b.Navigation("SocialChannels");
                });

            modelBuilder.Entity("SNM.LinkedIn.Domain.Entities.Post", b =>
                {
                    b.Navigation("MediaData");
                });
#pragma warning restore 612, 618
        }
    }
}
