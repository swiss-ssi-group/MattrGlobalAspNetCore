﻿// <auto-generated />
using BoInsurance.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BoInsurance.Migrations
{
    [DbContext(typeof(BoInsuranceVerifyMattrContext))]
    [Migration("20210423164008_bo_update2")]
    partial class bo_update2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BoInsurance.Data.DrivingLicensePresentationTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DidId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MattrPresentationTemplateReponse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TemplateId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DrivingLicensePresentationTemplates");
                });

            modelBuilder.Entity("BoInsurance.Data.DrivingLicensePresentationVerify", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CallbackUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Did")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DidId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvokePresentationResponse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SignAndEncodePresentationRequestBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TemplateId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DrivingLicensePresentationVerifications");
                });
#pragma warning restore 612, 618
        }
    }
}
