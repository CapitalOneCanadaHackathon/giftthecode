using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Indspire.Soaring.Engagement.Data.Migrations
{
    public partial class AddedInstance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstanceID",
                table: "Award",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Instance",
                columns: table => new
                {
                    InstanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DefaultInstance = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instance", x => x.InstanceID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Award_InstanceID",
                table: "Award",
                column: "InstanceID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Award_Instance_InstanceID",
            //    table: "Award",
            //    column: "InstanceID",
            //    principalTable: "Instance",
            //    principalColumn: "InstanceID",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Award_Instance_InstanceID",
            //    table: "Award");

            migrationBuilder.DropTable(
                name: "Instance");

            migrationBuilder.DropIndex(
                name: "IX_Award_InstanceID",
                table: "Award");

            migrationBuilder.DropColumn(
                name: "InstanceID",
                table: "Award");
        }
    }
}
