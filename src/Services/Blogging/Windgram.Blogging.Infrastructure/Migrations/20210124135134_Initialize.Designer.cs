﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Windgram.Blogging.Infrastructure;

namespace Windgram.Blogging.Infrastructure.Migrations
{
    [DbContext(typeof(BloggingDbContext))]
    [Migration("20210124135134_Initialize")]
    partial class Initialize
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Windgram.Blogging.ApplicationCore.Domain.Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CoverFileId")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastModifiedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ParentPostId")
                        .HasColumnType("int");

                    b.Property<int>("PostStatus")
                        .HasColumnType("int");

                    b.Property<int>("PostType")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PublishedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Slug")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.HasKey("Id");

                    b.ToTable("post", "blogging");
                });

            modelBuilder.Entity("Windgram.Blogging.ApplicationCore.Domain.Entities.PostComment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("CommentId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("post_comment", "blogging");
                });

            modelBuilder.Entity("Windgram.Blogging.ApplicationCore.Domain.Entities.PostContent", b =>
                {
                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<string>("HtmlContent")
                        .HasColumnType("longtext");

                    b.Property<string>("MetaDescription")
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)");

                    b.Property<string>("MetaKeyword")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("PostId");

                    b.ToTable("post_content", "blogging");
                });

            modelBuilder.Entity("Windgram.Blogging.ApplicationCore.Domain.Entities.PostRating", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsCommend")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("post_rating", "blogging");
                });

            modelBuilder.Entity("Windgram.Blogging.ApplicationCore.Domain.Entities.PostTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("TagId");

                    b.ToTable("post_tag", "blogging");
                });

            modelBuilder.Entity("Windgram.Blogging.ApplicationCore.Domain.Entities.PostView", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("HostAddress")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("post_view", "blogging");
                });

            modelBuilder.Entity("Windgram.Blogging.ApplicationCore.Domain.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastModifiedDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Alias")
                        .IsUnique();

                    b.ToTable("tag", "blogging");
                });

            modelBuilder.Entity("Windgram.Blogging.ApplicationCore.Domain.Entities.PostComment", b =>
                {
                    b.HasOne("Windgram.Blogging.ApplicationCore.Domain.Entities.Post", "Post")
                        .WithMany("PostComments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Windgram.Blogging.ApplicationCore.Domain.Entities.PostContent", b =>
                {
                    b.HasOne("Windgram.Blogging.ApplicationCore.Domain.Entities.Post", "Post")
                        .WithOne("PostContent")
                        .HasForeignKey("Windgram.Blogging.ApplicationCore.Domain.Entities.PostContent", "PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Windgram.Blogging.ApplicationCore.Domain.Entities.PostRating", b =>
                {
                    b.HasOne("Windgram.Blogging.ApplicationCore.Domain.Entities.Post", "Post")
                        .WithMany("PostRatings")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Windgram.Blogging.ApplicationCore.Domain.Entities.PostTag", b =>
                {
                    b.HasOne("Windgram.Blogging.ApplicationCore.Domain.Entities.Post", "Post")
                        .WithMany("PostTags")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Windgram.Blogging.ApplicationCore.Domain.Entities.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("Windgram.Blogging.ApplicationCore.Domain.Entities.PostView", b =>
                {
                    b.HasOne("Windgram.Blogging.ApplicationCore.Domain.Entities.Post", "Post")
                        .WithMany("PostViews")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Windgram.Blogging.ApplicationCore.Domain.Entities.Post", b =>
                {
                    b.Navigation("PostComments");

                    b.Navigation("PostContent");

                    b.Navigation("PostRatings");

                    b.Navigation("PostTags");

                    b.Navigation("PostViews");
                });
#pragma warning restore 612, 618
        }
    }
}
