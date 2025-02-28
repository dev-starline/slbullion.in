using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SL_Bullion.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblBank",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clientId = table.Column<int>(type: "int", nullable: false),
                    accountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ifscCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    branchName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bankLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bankLogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBank", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblBankLogo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBankLogo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblBankRate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clientId = table.Column<int>(type: "int", nullable: false),
                    premiumGold = table.Column<double>(type: "float", nullable: false),
                    premiumSilver = table.Column<double>(type: "float", nullable: false),
                    spotTypeGold = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    spotTypeSilver = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    interBankGold = table.Column<double>(type: "float", nullable: false),
                    interBankSilver = table.Column<double>(type: "float", nullable: false),
                    conversionGold = table.Column<double>(type: "float", nullable: false),
                    conversionSilver = table.Column<double>(type: "float", nullable: false),
                    customDutyGold = table.Column<double>(type: "float", nullable: false),
                    customDutySilver = table.Column<double>(type: "float", nullable: false),
                    marginGold = table.Column<double>(type: "float", nullable: false),
                    marginSilver = table.Column<double>(type: "float", nullable: false),
                    gstGold = table.Column<double>(type: "float", nullable: false),
                    gstSilver = table.Column<double>(type: "float", nullable: false),
                    divisionGold = table.Column<double>(type: "float", nullable: false),
                    divisionSilver = table.Column<double>(type: "float", nullable: false),
                    multiplyGold = table.Column<double>(type: "float", nullable: false),
                    multiplySilver = table.Column<double>(type: "float", nullable: false),
                    modifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBankRate", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblCoin",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clientId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isView = table.Column<bool>(type: "bit", nullable: false),
                    isStock = table.Column<bool>(type: "bit", nullable: false),
                    rateType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    buyPremium = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sellPremium = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    division = table.Column<double>(type: "float", nullable: false),
                    multiply = table.Column<double>(type: "float", nullable: false),
                    buyCommonPremium = table.Column<double>(type: "float", nullable: false),
                    sellCommonPremium = table.Column<double>(type: "float", nullable: false),
                    index = table.Column<int>(type: "int", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCoin", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblCoinBank",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clientId = table.Column<int>(type: "int", nullable: false),
                    premiumGold = table.Column<double>(type: "float", nullable: false),
                    premiumSilver = table.Column<double>(type: "float", nullable: false),
                    spotTypeGold = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    spotTypeSilver = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    interBankGold = table.Column<double>(type: "float", nullable: false),
                    interBankSilver = table.Column<double>(type: "float", nullable: false),
                    conversionGold = table.Column<double>(type: "float", nullable: false),
                    conversionSilver = table.Column<double>(type: "float", nullable: false),
                    customDutyGold = table.Column<double>(type: "float", nullable: false),
                    customDutySilver = table.Column<double>(type: "float", nullable: false),
                    marginGold = table.Column<double>(type: "float", nullable: false),
                    marginSilver = table.Column<double>(type: "float", nullable: false),
                    gstGold = table.Column<double>(type: "float", nullable: false),
                    gstSilver = table.Column<double>(type: "float", nullable: false),
                    divisionGold = table.Column<double>(type: "float", nullable: false),
                    divisionSilver = table.Column<double>(type: "float", nullable: false),
                    multiplyGold = table.Column<double>(type: "float", nullable: false),
                    multiplySilver = table.Column<double>(type: "float", nullable: false),
                    modifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCoinBank", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblContact",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clientId = table.Column<int>(type: "int", nullable: false),
                    marqueeTop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    marqueeBottom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatsAppNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isBuy = table.Column<bool>(type: "bit", nullable: false),
                    isSell = table.Column<bool>(type: "bit", nullable: false),
                    isHigh = table.Column<bool>(type: "bit", nullable: false),
                    isLow = table.Column<bool>(type: "bit", nullable: false),
                    isRate = table.Column<bool>(type: "bit", nullable: false),
                    isCoinRate = table.Column<bool>(type: "bit", nullable: false),
                    bannerWeb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bannerApp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    goldDifferance = table.Column<int>(type: "int", nullable: false),
                    silverDifferance = table.Column<int>(type: "int", nullable: false),
                    modifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblContact", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblFeedback",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clientId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFeedback", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblKyc",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clientId = table.Column<int>(type: "int", nullable: false),
                    mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    companyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    companyAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    partnerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    partnerMobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    officeMobile1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    officeMobile2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    residenceAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    branchName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    accountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ifsc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gstNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    panNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblKyc", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    firmName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    clientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    domain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    symbol = table.Column<int>(type: "int", nullable: false),
                    group = table.Column<int>(type: "int", nullable: false),
                    versionAndroid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    versionIos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fcmKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isCoin = table.Column<bool>(type: "bit", nullable: false),
                    isJewellery = table.Column<bool>(type: "bit", nullable: false),
                    isKyc = table.Column<bool>(type: "bit", nullable: false),
                    isOtr = table.Column<bool>(type: "bit", nullable: false),
                    isUpdate = table.Column<bool>(type: "bit", nullable: false),
                    isBank = table.Column<bool>(type: "bit", nullable: false),
                    isFeedback = table.Column<bool>(type: "bit", nullable: false),
                    isClientRate = table.Column<bool>(type: "bit", nullable: false),
                    createDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMaster", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblMasterLogin",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMasterLogin", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblOtr",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clientId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    firmname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    modifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOtr", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblReferanceSymbol",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clientId = table.Column<int>(type: "int", nullable: false),
                    source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isMaster = table.Column<bool>(type: "bit", nullable: false),
                    isView = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReferanceSymbol", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblSymbol",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clientId = table.Column<int>(type: "int", nullable: false),
                    uniqueId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sourceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isView = table.Column<bool>(type: "bit", nullable: false),
                    rateType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productType = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    buyPremium = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sellPremium = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    division = table.Column<double>(type: "float", nullable: false),
                    multiply = table.Column<double>(type: "float", nullable: false),
                    gst = table.Column<double>(type: "float", nullable: false),
                    buyCommonPremium = table.Column<double>(type: "float", nullable: false),
                    sellCommonPremium = table.Column<double>(type: "float", nullable: false),
                    index = table.Column<int>(type: "int", nullable: false),
                    digit = table.Column<int>(type: "int", nullable: false),
                    identifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    changePremiumDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSymbol", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tblUpdate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    clientId = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    modifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUpdate", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblBank");

            migrationBuilder.DropTable(
                name: "tblBankLogo");

            migrationBuilder.DropTable(
                name: "tblBankRate");

            migrationBuilder.DropTable(
                name: "tblCoin");

            migrationBuilder.DropTable(
                name: "tblCoinBank");

            migrationBuilder.DropTable(
                name: "tblContact");

            migrationBuilder.DropTable(
                name: "tblFeedback");

            migrationBuilder.DropTable(
                name: "tblKyc");

            migrationBuilder.DropTable(
                name: "tblMaster");

            migrationBuilder.DropTable(
                name: "tblMasterLogin");

            migrationBuilder.DropTable(
                name: "tblOtr");

            migrationBuilder.DropTable(
                name: "tblReferanceSymbol");

            migrationBuilder.DropTable(
                name: "tblSymbol");

            migrationBuilder.DropTable(
                name: "tblUpdate");
        }
    }
}
