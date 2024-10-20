using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat_App.Web.Migrations
{
	/// <inheritdoc />
	public partial class InitialCreate : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Messages",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Username = table.Column<string>(type: "TEXT", nullable: false),
					Text = table.Column<string>(type: "TEXT", nullable: false),
					Timestamp = table.Column<string>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Messages", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Reactions",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					MessageId = table.Column<int>(type: "INTEGER", nullable: false),
					ReactionType = table.Column<string>(type: "TEXT", nullable: false),
					Username = table.Column<string>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Reactions", x => x.Id);
					table.ForeignKey(
						name: "FK_Reactions_Messages_MessageId",
						column: x => x.MessageId,
						principalTable: "Messages",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Reactions_MessageId",
				table: "Reactions",
				column: "MessageId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Reactions");

			migrationBuilder.DropTable(
				name: "Messages");
		}
	}
}
