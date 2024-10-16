using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banktive.Web.Data.Migrations
{
    public partial class AddMoreBaseData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("Countries", new string[]
                {"Id", "Name", "Code","Enabled"},
                new object[]
                {
                    1,"Chile","CLP",true
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
