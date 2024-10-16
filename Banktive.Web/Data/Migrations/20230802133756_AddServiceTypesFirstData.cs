using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banktive.Web.Data.Migrations
{
    public partial class AddServiceTypesFirstData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("ServiceTypes", new string[]
                {"Id", "Name"},
                new object[]
                {
                    1,"Electricity"
                });
            migrationBuilder.InsertData("ServiceTypes", new string[]
                {"Id", "Name"},
                new object[]
                {
                    2,"Water"
                });
            migrationBuilder.InsertData("ServiceTypes", new string[]
                {"Id", "Name"},
                new object[]
                {
                    3,"Professional services"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
