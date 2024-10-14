using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWebApp1.Migrations
{
    /// <inheritdoc />
    public partial class PawFund : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "PetCategory",
            //    columns: table => new
            //    {
            //        PetCategoryId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PetCategory", x => x.PetCategoryId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Role",
            //    columns: table => new
            //    {
            //        RoleId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Role", x => x.RoleId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TransactionStatus",
            //    columns: table => new
            //    {
            //        TransactionStatusId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TransactionStatus", x => x.TransactionStatusId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TransactionType",
            //    columns: table => new
            //    {
            //        TransactionTypeId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TransactionType", x => x.TransactionTypeId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "User",
            //    columns: table => new
            //    {
            //        UserId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Fullname = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        IsApprovedUser = table.Column<bool>(type: "bit", nullable: false),
            //        IsApproved = table.Column<bool>(type: "bit", nullable: false),
            //        Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_User", x => x.UserId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Pet",
            //    columns: table => new
            //    {
            //        PetId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        PetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        PetType = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Age = table.Column<int>(type: "int", nullable: false),
            //        Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        MedicalCondition = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        ContactPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        PetCategoryId = table.Column<int>(type: "int", nullable: false),
            //        IsAdopted = table.Column<bool>(type: "bit", nullable: false),
            //        IsApproved = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Pet", x => x.PetId);
            //        table.ForeignKey(
            //            name: "FK_Pet_PetCategory_PetCategoryId",
            //            column: x => x.PetCategoryId,
            //            principalTable: "PetCategory",
            //            principalColumn: "PetCategoryId");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "DonationEvent",
            //    columns: table => new
            //    {
            //        EventId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        EventContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        EventStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        EventEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        IsEnded = table.Column<bool>(type: "bit", nullable: false),
            //        IsApproved = table.Column<bool>(type: "bit", nullable: false),
            //        UserCreatedId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_DonationEvent", x => x.EventId);
            //        table.ForeignKey(
            //            name: "FK_DonationEvent_User_UserCreatedId",
            //            column: x => x.UserCreatedId,
            //            principalTable: "User",
            //            principalColumn: "UserId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "UserRole",
            //    columns: table => new
            //    {
            //        UserId = table.Column<int>(type: "int", nullable: false),
            //        RoleId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
            //        table.ForeignKey(
            //            name: "FK_UserRole_Role_RoleId",
            //            column: x => x.RoleId,
            //            principalTable: "Role",
            //            principalColumn: "RoleId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_UserRole_User_UserId",
            //            column: x => x.UserId,
            //            principalTable: "User",
            //            principalColumn: "UserId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Adoption",
            //    columns: table => new
            //    {
            //        AdoptionId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        UserId = table.Column<int>(type: "int", nullable: false),
            //        PetId = table.Column<int>(type: "int", nullable: false),
            //        IsApproved = table.Column<bool>(type: "bit", nullable: false),
            //        Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Adoption", x => x.AdoptionId);
            //        table.ForeignKey(
            //            name: "FK_Adoption_Pet_PetId",
            //            column: x => x.PetId,
            //            principalTable: "Pet",
            //            principalColumn: "PetId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Adoption_User_UserId",
            //            column: x => x.UserId,
            //            principalTable: "User",
            //            principalColumn: "UserId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PetImage",
            //    columns: table => new
            //    {
            //        PetImageId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ImageDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        IsThumbnailImage = table.Column<bool>(type: "bit", nullable: false),
            //        PetId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PetImage", x => x.PetImageId);
            //        table.ForeignKey(
            //            name: "FK_PetImage_Pet_PetId",
            //            column: x => x.PetId,
            //            principalTable: "Pet",
            //            principalColumn: "PetId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "DonationImage",
            //    columns: table => new
            //    {
            //        ImageId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ImageDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        IsThumbnailImage = table.Column<bool>(type: "bit", nullable: false),
            //        DonationEventId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_DonationImage", x => x.ImageId);
            //        table.ForeignKey(
            //            name: "FK_DonationImage_DonationEvent_DonationEventId",
            //            column: x => x.DonationEventId,
            //            principalTable: "DonationEvent",
            //            principalColumn: "EventId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Transaction",
            //    columns: table => new
            //    {
            //        TransactionId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        TransactionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        IsMoneyDonation = table.Column<bool>(type: "bit", nullable: false),
            //        IsResourceDonation = table.Column<bool>(type: "bit", nullable: false),
            //        DonationEventId = table.Column<int>(type: "int", nullable: true),
            //        UserId = table.Column<int>(type: "int", nullable: false),
            //        TransactionStatusId = table.Column<int>(type: "int", nullable: false),
            //        TransactionTypeId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Transaction", x => x.TransactionId);
            //        table.ForeignKey(
            //            name: "FK_Transaction_DonationEvent_DonationEventId",
            //            column: x => x.DonationEventId,
            //            principalTable: "DonationEvent",
            //            principalColumn: "EventId");
            //        table.ForeignKey(
            //            name: "FK_Transaction_TransactionStatus_TransactionStatusId",
            //            column: x => x.TransactionStatusId,
            //            principalTable: "TransactionStatus",
            //            principalColumn: "TransactionStatusId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Transaction_TransactionType_TransactionTypeId",
            //            column: x => x.TransactionTypeId,
            //            principalTable: "TransactionType",
            //            principalColumn: "TransactionTypeId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Transaction_User_UserId",
            //            column: x => x.UserId,
            //            principalTable: "User",
            //            principalColumn: "UserId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Adoption_PetId",
            //    table: "Adoption",
            //    column: "PetId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Adoption_UserId",
            //    table: "Adoption",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_DonationEvent_UserCreatedId",
            //    table: "DonationEvent",
            //    column: "UserCreatedId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_DonationImage_DonationEventId",
            //    table: "DonationImage",
            //    column: "DonationEventId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Pet_PetCategoryId",
            //    table: "Pet",
            //    column: "PetCategoryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PetImage_PetId",
            //    table: "PetImage",
            //    column: "PetId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Transaction_DonationEventId",
            //    table: "Transaction",
            //    column: "DonationEventId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Transaction_TransactionStatusId",
            //    table: "Transaction",
            //    column: "TransactionStatusId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Transaction_TransactionTypeId",
            //    table: "Transaction",
            //    column: "TransactionTypeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Transaction_UserId",
            //    table: "Transaction",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_UserRole_RoleId",
            //    table: "UserRole",
            //    column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adoption");

            migrationBuilder.DropTable(
                name: "DonationImage");

            migrationBuilder.DropTable(
                name: "PetImage");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Pet");

            migrationBuilder.DropTable(
                name: "DonationEvent");

            migrationBuilder.DropTable(
                name: "TransactionStatus");

            migrationBuilder.DropTable(
                name: "TransactionType");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "PetCategory");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
