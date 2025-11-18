using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SL_Bullion.Migrations
{
    /// <inheritdoc />
    public partial class historyDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "historyDate",
                table: "tblHistoryRate",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "historyDate",
                table: "tblHistoryRate");
        }
    }
}
