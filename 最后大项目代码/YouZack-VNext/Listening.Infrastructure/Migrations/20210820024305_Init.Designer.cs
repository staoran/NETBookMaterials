﻿// <auto-generated />
using System;
using Listening.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Listening.Infrastructure.Migrations
{
    [DbContext(typeof(ListeningDbContext))]
    [Migration("20210820024305_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "6.0.0-preview.7.21378.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Listening.Domain.Entities.Album", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("AutoIncId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("SequenceNumber")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .IsClustered(false);

                    b.HasIndex("CategoryId", "IsDeleted");

                    b.ToTable("T_Albums", (string)null);
                });

            modelBuilder.Entity("Listening.Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CoverUrl")
                        .HasMaxLength(500)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("SequenceNumber")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .IsClustered(false);

                    b.ToTable("T_Categories", (string)null);
                });

            modelBuilder.Entity("Listening.Domain.Entities.Episode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AlbumId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeSpan>("AudioDuration")
                        .HasColumnType("time");

                    b.Property<string>("AudioUrl")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("SequenceNumber")
                        .HasColumnType("int");

                    b.Property<string>("Subtitle")
                        .IsRequired()
                        .HasMaxLength(2147483647)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubtitleType")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.HasKey("Id")
                        .IsClustered(false);

                    b.HasIndex("AlbumId", "IsDeleted");

                    b.ToTable("T_Episodes", (string)null);
                });

            modelBuilder.Entity("Listening.Domain.Entities.Album", b =>
                {
                    b.OwnsOne("Zack.DomainCommons.Models.MultilingualString", "Name", b1 =>
                        {
                            b1.Property<Guid>("AlbumId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Chinese")
                                .IsRequired()
                                .HasMaxLength(200)
                                .IsUnicode(true)
                                .HasColumnType("nvarchar(200)");

                            b1.Property<string>("English")
                                .IsRequired()
                                .HasMaxLength(200)
                                .IsUnicode(true)
                                .HasColumnType("nvarchar(200)");

                            b1.HasKey("AlbumId");

                            b1.ToTable("T_Albums");

                            b1.WithOwner()
                                .HasForeignKey("AlbumId");
                        });

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("Listening.Domain.Entities.Category", b =>
                {
                    b.OwnsOne("Zack.DomainCommons.Models.MultilingualString", "Name", b1 =>
                        {
                            b1.Property<Guid>("CategoryId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Chinese")
                                .IsRequired()
                                .HasMaxLength(200)
                                .IsUnicode(true)
                                .HasColumnType("nvarchar(200)");

                            b1.Property<string>("English")
                                .IsRequired()
                                .HasMaxLength(200)
                                .IsUnicode(true)
                                .HasColumnType("nvarchar(200)");

                            b1.HasKey("CategoryId");

                            b1.ToTable("T_Categories");

                            b1.WithOwner()
                                .HasForeignKey("CategoryId");
                        });

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("Listening.Domain.Entities.Episode", b =>
                {
                    b.OwnsOne("Zack.DomainCommons.Models.MultilingualString", "Name", b1 =>
                        {
                            b1.Property<Guid>("EpisodeId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Chinese")
                                .IsRequired()
                                .HasMaxLength(200)
                                .IsUnicode(true)
                                .HasColumnType("nvarchar(200)");

                            b1.Property<string>("English")
                                .IsRequired()
                                .HasMaxLength(200)
                                .IsUnicode(true)
                                .HasColumnType("nvarchar(200)");

                            b1.HasKey("EpisodeId");

                            b1.ToTable("T_Episodes");

                            b1.WithOwner()
                                .HasForeignKey("EpisodeId");
                        });

                    b.Navigation("Name")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
