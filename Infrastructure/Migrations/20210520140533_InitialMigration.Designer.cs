﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20210520140533_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("CourseStudent", b =>
                {
                    b.Property<string>("CoursesCourseId")
                        .HasColumnType("text");

                    b.Property<string>("StudentsId")
                        .HasColumnType("text");

                    b.HasKey("CoursesCourseId", "StudentsId");

                    b.HasIndex("StudentsId");

                    b.ToTable("CourseStudent");
                });

            modelBuilder.Entity("Domain.BaseModels.BaseUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("AvatarId")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsValidating")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasDefaultValue("");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("ResettingPassword")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasDefaultValue("");

                    b.Property<int>("UserType")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(2);

                    b.Property<string>("ValidationCode")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AvatarId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("NOT \"UserName\"=''");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasFilter("NOT \"UserName\"=''");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BaseUser");
                });

            modelBuilder.Entity("Domain.BaseModels.FileEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("ContentType")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<byte>("Type")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Domain.Models.Announcement", b =>
                {
                    b.Property<string>("AnnouncementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("AnnouncementDescription")
                        .HasColumnType("text");

                    b.Property<string>("AnnouncementTitle")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Department")
                        .HasColumnType("text");

                    b.Property<string>("Faculty")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("AnnouncementId");

                    b.HasIndex("UserId");

                    b.ToTable("Announcements");
                });

            modelBuilder.Entity("Domain.Models.Course", b =>
                {
                    b.Property<string>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("CourseTitle")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("InstructorId")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("CourseId");

                    b.HasIndex("InstructorId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("Domain.Models.CourseEvent", b =>
                {
                    b.Property<string>("CourseEventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("CourseId")
                        .HasColumnType("text");

                    b.Property<string>("EventDescription")
                        .HasColumnType("text");

                    b.Property<string>("EventName")
                        .HasColumnType("text");

                    b.Property<DateTime>("EventTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("EventType")
                        .HasColumnType("integer");

                    b.HasKey("CourseEventId");

                    b.HasIndex("CourseId");

                    b.ToTable("CourseEvents");
                });

            modelBuilder.Entity("Domain.Models.Event", b =>
                {
                    b.Property<string>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("EventDescription")
                        .HasColumnType("text");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EventTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastModifiedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Domain.Models.Favourite", b =>
                {
                    b.Property<string>("FavouriteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("FavouriteId");

                    b.HasIndex("UserId");

                    b.ToTable("Favourites");
                });

            modelBuilder.Entity("Domain.Models.News", b =>
                {
                    b.Property<string>("NewsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("FileId")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("NewsId");

                    b.HasIndex("FileId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("Domain.Models.Offer", b =>
                {
                    b.Property<string>("OfferId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("AvatarId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("OfferType")
                        .HasColumnType("integer");

                    b.Property<string>("Price")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("OfferId");

                    b.HasIndex("AvatarId");

                    b.HasIndex("UserId");

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("Domain.Models.PollAnswer", b =>
                {
                    b.Property<string>("AnswerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("AnswerDescription")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("QuestionId")
                        .HasColumnType("text");

                    b.HasKey("AnswerId");

                    b.HasIndex("QuestionId");

                    b.ToTable("PollAnswers");
                });

            modelBuilder.Entity("Domain.Models.PollQuestion", b =>
                {
                    b.Property<string>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("CourseId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("InstructorId")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsOpen")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("MultiVote")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("QuestionDescription")
                        .HasColumnType("text");

                    b.HasKey("QuestionId");

                    b.HasIndex("CourseId");

                    b.HasIndex("InstructorId");

                    b.ToTable("PollQuestions");
                });

            modelBuilder.Entity("Domain.Models.Suggestion", b =>
                {
                    b.Property<string>("SuggestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Detail")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("SenderId")
                        .HasColumnType("text");

                    b.HasKey("SuggestionId");

                    b.HasIndex("SenderId");

                    b.ToTable("Suggestions");
                });

            modelBuilder.Entity("Domain.Models.Time", b =>
                {
                    b.Property<string>("TimeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("CourseId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("WeekDay")
                        .HasColumnType("text");

                    b.HasKey("TimeId");

                    b.HasIndex("CourseId");

                    b.ToTable("Times");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("PollAnswerStudent", b =>
                {
                    b.Property<string>("PollAnswersAnswerId")
                        .HasColumnType("text");

                    b.Property<string>("VotersId")
                        .HasColumnType("text");

                    b.HasKey("PollAnswersAnswerId", "VotersId");

                    b.HasIndex("VotersId");

                    b.ToTable("PollAnswerStudent");
                });

            modelBuilder.Entity("Domain.Models.Instructor", b =>
                {
                    b.HasBaseType("Domain.BaseModels.BaseUser");

                    b.Property<string>("InstructorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("Instructor");
                });

            modelBuilder.Entity("Domain.Models.Student", b =>
                {
                    b.HasBaseType("Domain.BaseModels.BaseUser");

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("CourseStudent", b =>
                {
                    b.HasOne("Domain.Models.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursesCourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.Student", null)
                        .WithMany()
                        .HasForeignKey("StudentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.BaseModels.BaseUser", b =>
                {
                    b.HasOne("Domain.BaseModels.FileEntity", "Avatar")
                        .WithMany()
                        .HasForeignKey("AvatarId");

                    b.Navigation("Avatar");
                });

            modelBuilder.Entity("Domain.Models.Announcement", b =>
                {
                    b.HasOne("Domain.BaseModels.BaseUser", "BaseUser")
                        .WithMany("Announcements")
                        .HasForeignKey("UserId");

                    b.Navigation("BaseUser");
                });

            modelBuilder.Entity("Domain.Models.Course", b =>
                {
                    b.HasOne("Domain.Models.Instructor", "Instructor")
                        .WithMany("Courses")
                        .HasForeignKey("InstructorId");

                    b.Navigation("Instructor");
                });

            modelBuilder.Entity("Domain.Models.CourseEvent", b =>
                {
                    b.HasOne("Domain.Models.Course", "Course")
                        .WithMany("CourseEvents")
                        .HasForeignKey("CourseId");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Domain.Models.Event", b =>
                {
                    b.HasOne("Domain.BaseModels.BaseUser", "User")
                        .WithMany("Events")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Models.Favourite", b =>
                {
                    b.HasOne("Domain.BaseModels.BaseUser", "BaseUser")
                        .WithMany("Favourites")
                        .HasForeignKey("UserId");

                    b.Navigation("BaseUser");
                });

            modelBuilder.Entity("Domain.Models.News", b =>
                {
                    b.HasOne("Domain.BaseModels.FileEntity", "FileEntity")
                        .WithMany()
                        .HasForeignKey("FileId");

                    b.Navigation("FileEntity");
                });

            modelBuilder.Entity("Domain.Models.Offer", b =>
                {
                    b.HasOne("Domain.BaseModels.FileEntity", "Avatar")
                        .WithMany()
                        .HasForeignKey("AvatarId");

                    b.HasOne("Domain.BaseModels.BaseUser", "BaseUser")
                        .WithMany("Offers")
                        .HasForeignKey("UserId");

                    b.Navigation("Avatar");

                    b.Navigation("BaseUser");
                });

            modelBuilder.Entity("Domain.Models.PollAnswer", b =>
                {
                    b.HasOne("Domain.Models.PollQuestion", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Domain.Models.PollQuestion", b =>
                {
                    b.HasOne("Domain.Models.Course", "Course")
                        .WithMany("Polls")
                        .HasForeignKey("CourseId");

                    b.HasOne("Domain.Models.Instructor", null)
                        .WithMany("Polls")
                        .HasForeignKey("InstructorId");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Domain.Models.Suggestion", b =>
                {
                    b.HasOne("Domain.BaseModels.BaseUser", "Sender")
                        .WithMany("Suggestions")
                        .HasForeignKey("SenderId");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Domain.Models.Time", b =>
                {
                    b.HasOne("Domain.Models.Course", "Course")
                        .WithMany("Times")
                        .HasForeignKey("CourseId");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Domain.BaseModels.BaseUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Domain.BaseModels.BaseUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.BaseModels.BaseUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Domain.BaseModels.BaseUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PollAnswerStudent", b =>
                {
                    b.HasOne("Domain.Models.PollAnswer", null)
                        .WithMany()
                        .HasForeignKey("PollAnswersAnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.Student", null)
                        .WithMany()
                        .HasForeignKey("VotersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.BaseModels.BaseUser", b =>
                {
                    b.Navigation("Announcements");

                    b.Navigation("Events");

                    b.Navigation("Favourites");

                    b.Navigation("Offers");

                    b.Navigation("Suggestions");
                });

            modelBuilder.Entity("Domain.Models.Course", b =>
                {
                    b.Navigation("CourseEvents");

                    b.Navigation("Polls");

                    b.Navigation("Times");
                });

            modelBuilder.Entity("Domain.Models.PollQuestion", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("Domain.Models.Instructor", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("Polls");
                });
#pragma warning restore 612, 618
        }
    }
}