using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PestKitAB104.Migrations
{
    public partial class CreateOrdersandBasketItemsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItem_AspNetUsers_AppUserId",
                table: "BasketItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketItem_Order_OrderId",
                table: "BasketItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketItem_Products_ProductId",
                table: "BasketItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_AppUserId",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasketItem",
                table: "BasketItem");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "BasketItem",
                newName: "BasketItems");

            migrationBuilder.RenameIndex(
                name: "IX_Order_AppUserId",
                table: "Orders",
                newName: "IX_Orders_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketItem_ProductId",
                table: "BasketItems",
                newName: "IX_BasketItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketItem_OrderId",
                table: "BasketItems",
                newName: "IX_BasketItems_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketItem_AppUserId",
                table: "BasketItems",
                newName: "IX_BasketItems_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasketItems",
                table: "BasketItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_AspNetUsers_AppUserId",
                table: "BasketItems",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_Orders_OrderId",
                table: "BasketItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_Products_ProductId",
                table: "BasketItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_AppUserId",
                table: "Orders",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_AspNetUsers_AppUserId",
                table: "BasketItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_Orders_OrderId",
                table: "BasketItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_Products_ProductId",
                table: "BasketItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_AppUserId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasketItems",
                table: "BasketItems");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameTable(
                name: "BasketItems",
                newName: "BasketItem");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_AppUserId",
                table: "Order",
                newName: "IX_Order_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketItems_ProductId",
                table: "BasketItem",
                newName: "IX_BasketItem_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketItems_OrderId",
                table: "BasketItem",
                newName: "IX_BasketItem_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketItems_AppUserId",
                table: "BasketItem",
                newName: "IX_BasketItem_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasketItem",
                table: "BasketItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItem_AspNetUsers_AppUserId",
                table: "BasketItem",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItem_Order_OrderId",
                table: "BasketItem",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItem_Products_ProductId",
                table: "BasketItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_AppUserId",
                table: "Order",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
