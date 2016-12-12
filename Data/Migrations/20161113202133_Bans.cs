using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infinity.so.Data.Migrations
{
    public partial class Bans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdminCkey = table.Column<string>(nullable: true),
                    BanDate = table.Column<DateTime>(nullable: true),
                    BanExpiryTime = table.Column<DateTime>(nullable: true),
                    BanType = table.Column<int>(nullable: false),
                    Job = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: false),
                    SubjectCid = table.Column<string>(nullable: true),
                    SubjectCkey = table.Column<string>(nullable: false),
                    SubjectIPAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bans", x => x.Id);
                });

            migrationBuilder.AlterColumn<string>(
                name: "Ckey",
                table: "AspNetUsers",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bans");

            migrationBuilder.AlterColumn<string>(
                name: "Ckey",
                table: "AspNetUsers",
                nullable: true);
        }
    }
}
