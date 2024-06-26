﻿// <auto-generated />
using System;
using BevMan.Domain.Entities;
using BevMan.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BevMan.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240512031546_UserProfile")]
    partial class UserProfile
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "public", "app_role", new[] { "user", "user_manager", "admin" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BevMan.Domain.Entities.Balance", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric")
                        .HasColumnName("amount");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_balances");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_balances_user_id");

                    b.ToTable("balances", (string)null);
                });

            modelBuilder.Entity("BevMan.Domain.Entities.BalanceRequest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric")
                        .HasColumnName("amount");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_balance_requests");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_balance_requests_user_id");

                    b.ToTable("balance_requests", (string)null);
                });

            modelBuilder.Entity("BevMan.Domain.Entities.Product", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("description");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<string>("PublicUrl")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("public_url");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<Guid?>("StorageObjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("storage_object_id");

                    b.HasKey("Id")
                        .HasName("pk_products");

                    b.HasIndex("StorageObjectId")
                        .IsUnique()
                        .HasDatabaseName("ix_products_storage_object_id");

                    b.ToTable("products", (string)null);
                });

            modelBuilder.Entity("BevMan.Domain.Entities.StorageObject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("BucketId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("bucket_id");

                    b.Property<string[]>("PathTokens")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("path_tokens");

                    b.HasKey("Id")
                        .HasName("pk_objects");

                    b.ToTable("objects", "storage", t =>
                        {
                            t.ExcludeFromMigrations();
                        });
                });

            modelBuilder.Entity("BevMan.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", "auth", t =>
                        {
                            t.ExcludeFromMigrations();
                        });
                });

            modelBuilder.Entity("BevMan.Domain.Entities.UserProfile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("AvatarUrl")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("avatar_url");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("display_name");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<Guid?>("StorageObjectId")
                        .HasColumnType("uuid")
                        .HasColumnName("storage_object_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_profiles");

                    b.HasIndex("StorageObjectId")
                        .IsUnique()
                        .HasDatabaseName("ix_user_profiles_storage_object_id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_user_profiles_user_id");

                    b.ToTable("user_profiles", (string)null);
                });

            modelBuilder.Entity("BevMan.Domain.Entities.UserRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text")
                        .HasColumnName("last_modified_by");

                    b.Property<AppRole>("Role")
                        .HasColumnType("app_role")
                        .HasColumnName("role");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_roles");

                    b.HasIndex("UserId", "Role")
                        .IsUnique()
                        .HasDatabaseName("ix_user_roles_user_id_role");

                    b.ToTable("user_roles", (string)null);
                });

            modelBuilder.Entity("BevMan.Domain.Entities.Balance", b =>
                {
                    b.HasOne("BevMan.Domain.Entities.User", "User")
                        .WithOne("Balance")
                        .HasForeignKey("BevMan.Domain.Entities.Balance", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_balances_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BevMan.Domain.Entities.BalanceRequest", b =>
                {
                    b.HasOne("BevMan.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_balance_requests_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BevMan.Domain.Entities.Product", b =>
                {
                    b.HasOne("BevMan.Domain.Entities.StorageObject", "StorageObject")
                        .WithOne()
                        .HasForeignKey("BevMan.Domain.Entities.Product", "StorageObjectId")
                        .HasConstraintName("fk_products_storage_objects_storage_object_id");

                    b.Navigation("StorageObject");
                });

            modelBuilder.Entity("BevMan.Domain.Entities.UserProfile", b =>
                {
                    b.HasOne("BevMan.Domain.Entities.StorageObject", "StorageObject")
                        .WithOne()
                        .HasForeignKey("BevMan.Domain.Entities.UserProfile", "StorageObjectId")
                        .HasConstraintName("fk_user_profiles_storage_objects_storage_object_id");

                    b.HasOne("BevMan.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_profiles_users_user_id");

                    b.Navigation("StorageObject");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BevMan.Domain.Entities.UserRole", b =>
                {
                    b.HasOne("BevMan.Domain.Entities.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_roles_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BevMan.Domain.Entities.User", b =>
                {
                    b.Navigation("Balance")
                        .IsRequired();

                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
