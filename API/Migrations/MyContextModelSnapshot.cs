﻿// <auto-generated />
using System;
using API.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("API.Models.Employee", b =>
                {
                    b.Property<string>("EmpId");

                    b.Property<string>("Address");

                    b.Property<string>("AssignmentSite");

                    b.Property<DateTimeOffset>("CreateData");

                    b.Property<DateTimeOffset>("DeleteData");

                    b.Property<string>("NIK");

                    b.Property<string>("Name");

                    b.Property<string>("Phone");

                    b.Property<DateTimeOffset>("UpdateDate");

                    b.Property<bool>("isDelete");

                    b.HasKey("EmpId");

                    b.ToTable("TB_M_Employee");
                });

            modelBuilder.Entity("API.Models.Feedback", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreateData");

                    b.Property<DateTimeOffset>("DeleteData");

                    b.Property<int>("QuestionId");

                    b.Property<double>("Rate");

                    b.Property<string>("Review");

                    b.Property<DateTimeOffset>("UpdateDate");

                    b.Property<string>("UserId");

                    b.Property<bool>("isDelete");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("UserId");

                    b.ToTable("TB_Trans_Feedback");
                });

            modelBuilder.Entity("API.Models.LogActivity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreateDate");

                    b.Property<string>("Email");

                    b.Property<string>("Response");

                    b.HasKey("Id");

                    b.ToTable("TB_Trans_Log");
                });

            modelBuilder.Entity("API.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreateData");

                    b.Property<DateTimeOffset>("DeleteData");

                    b.Property<string>("QuestionDesc");

                    b.Property<int>("TrainingId");

                    b.Property<DateTimeOffset>("UpdateDate");

                    b.Property<bool>("isDelete");

                    b.HasKey("Id");

                    b.HasIndex("TrainingId");

                    b.ToTable("TB_M_Question");
                });

            modelBuilder.Entity("API.Models.Role", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<DateTimeOffset>("CreateData");

                    b.Property<DateTimeOffset>("DeleteData");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedName");

                    b.Property<DateTimeOffset>("UpdateDate");

                    b.Property<bool>("isDelete");

                    b.HasKey("Id");

                    b.ToTable("TB_M_Role");
                });

            modelBuilder.Entity("API.Models.Training", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreateData");

                    b.Property<DateTimeOffset>("DeleteData");

                    b.Property<string>("Location");

                    b.Property<DateTimeOffset>("Schedule");

                    b.Property<string>("Target");

                    b.Property<string>("Title");

                    b.Property<int>("TypeId");

                    b.Property<DateTimeOffset>("UpdateDate");

                    b.Property<string>("UserId");

                    b.Property<bool>("isDelete");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.HasIndex("UserId");

                    b.ToTable("TB_M_Training");
                });

            modelBuilder.Entity("API.Models.TypeTraining", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreateData");

                    b.Property<DateTimeOffset>("DeleteData");

                    b.Property<string>("Name");

                    b.Property<DateTimeOffset>("UpdateDate");

                    b.Property<bool>("isDelete");

                    b.HasKey("Id");

                    b.ToTable("TB_M_TypeTraining");
                });

            modelBuilder.Entity("API.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Password");

                    b.Property<string>("VerifyCode");

                    b.HasKey("Id");

                    b.ToTable("TB_M_User");
                });

            modelBuilder.Entity("API.Models.UserRole", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("TB_M_UserRole");
                });

            modelBuilder.Entity("API.Models.Employee", b =>
                {
                    b.HasOne("API.Models.User", "User")
                        .WithOne("Employee")
                        .HasForeignKey("API.Models.Employee", "EmpId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("API.Models.Feedback", b =>
                {
                    b.HasOne("API.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("API.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("API.Models.Question", b =>
                {
                    b.HasOne("API.Models.Training", "Training")
                        .WithMany()
                        .HasForeignKey("TrainingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("API.Models.Training", b =>
                {
                    b.HasOne("API.Models.TypeTraining", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("API.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("API.Models.UserRole", b =>
                {
                    b.HasOne("API.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("API.Models.User", "User")
                        .WithMany("userRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
