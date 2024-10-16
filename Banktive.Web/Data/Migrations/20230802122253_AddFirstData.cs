using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banktive.Web.Data.Migrations
{
    public partial class AddFirstData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("Currencies", new string[]
                {"Id", "Name", "Code","Enabled"},
                new object[]
                {
                    1,"XRP","XRP",true
                });

            migrationBuilder.InsertData("PaymentStatus", new string[]
                {"Id", "Name"},
                new object[]
                {
                    1,"Created"
                });
            migrationBuilder.InsertData("PaymentStatus", new string[]
                {"Id", "Name"},
                new object[]
                {
                    2,"Cancelled"
                });
            migrationBuilder.InsertData("PaymentStatus", new string[]
                {"Id", "Name"},
                new object[]
                {
                    3,"Confirmed"
                });
            migrationBuilder.InsertData("PaymentStatus", new string[]
                {"Id", "Name"},
                new object[]
                {
                    4,"Rejected"
                });

            migrationBuilder.InsertData("PaymentTypes", new string[]
                {"Id", "Name"},
                new object[]
                {
                    1,"Regular"
                });
            migrationBuilder.InsertData("PaymentTypes", new string[]
                {"Id", "Name"},
                new object[]
                {
                    2,"Check"
                });
            migrationBuilder.InsertData("PaymentTypes", new string[]
                {"Id", "Name"},
                new object[]
                {
                    3,"Service"
                });
            migrationBuilder.InsertData("PaymentTypes", new string[]
                {"Id", "Name"},
                new object[]
                {
                    4,"TimeDeposit"
                });

            migrationBuilder.InsertData("CheckStatus", new string[]
                {"Id", "Name"},
                new object[]
                {
                    1,"Created"
                });
            migrationBuilder.InsertData("CheckStatus", new string[]
                {"Id", "Name"},
                new object[]
                {
                    2,"Cancelled"
                });
            migrationBuilder.InsertData("CheckStatus", new string[]
                {"Id", "Name"},
                new object[]
                {
                    3,"Confirmed"
                });
            migrationBuilder.InsertData("CheckStatus", new string[]
                {"Id", "Name"},
                new object[]
                {
                    4,"Cashed"
                });

            migrationBuilder.InsertData("DepositStatus", new string[]
                {"Id", "Name"},
                new object[]
                {
                    1,"Created"
                });
            migrationBuilder.InsertData("DepositStatus", new string[]
                {"Id", "Name"},
                new object[]
                {
                    2,"Cancelled"
                });
            migrationBuilder.InsertData("DepositStatus", new string[]
                {"Id", "Name"},
                new object[]
                {
                    3,"Confirmed"
                });
            migrationBuilder.InsertData("DepositStatus", new string[]
                {"Id", "Name"},
                new object[]
                {
                    4,"Cashed"
                });
            migrationBuilder.InsertData("DepositStatus", new string[]
                {"Id", "Name"},
                new object[]
                {
                    5,"Rejected"
                });
            migrationBuilder.InsertData("DepositStatus", new string[]
                {"Id", "Name"},
                new object[]
                {
                    6,"Expired"
                });

            migrationBuilder.InsertData("DepositTypes", new string[]
                {"Id", "Name"},
                new object[]
                {
                    1,"TimeEscrow"
                });
            migrationBuilder.InsertData("DepositTypes", new string[]
                {"Id", "Name"},
                new object[]
                {
                    2,"ConditionalEscrow"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
