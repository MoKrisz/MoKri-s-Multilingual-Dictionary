using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dictionary.Data.Migrations
{
    /// <inheritdoc />
    public partial class Translations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Text",
                schema: "dictionary",
                table: "Words",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Plural",
                schema: "dictionary",
                table: "Words",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Conjugation",
                schema: "dictionary",
                table: "Words",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Article",
                schema: "dictionary",
                table: "Words",
                type: "character varying(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "dictionary",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "TranslationGroups",
                schema: "dictionary",
                columns: table => new
                {
                    TranslationGroupId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslationGroups", x => x.TranslationGroupId);
                });

            migrationBuilder.CreateTable(
                name: "TranslationGroupTags",
                schema: "dictionary",
                columns: table => new
                {
                    TranslationGroupTagId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TranslationGroupId = table.Column<int>(type: "integer", nullable: false),
                    TagId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslationGroupTags", x => x.TranslationGroupTagId);
                    table.ForeignKey(
                        name: "FK_TranslationGroupTags_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "dictionary",
                        principalTable: "Tags",
                        principalColumn: "TagId");
                    table.ForeignKey(
                        name: "FK_TranslationGroupTags_TranslationGroups_TranslationGroupId",
                        column: x => x.TranslationGroupId,
                        principalSchema: "dictionary",
                        principalTable: "TranslationGroups",
                        principalColumn: "TranslationGroupId");
                });

            migrationBuilder.CreateTable(
                name: "WordTranslationGroups",
                schema: "dictionary",
                columns: table => new
                {
                    WordTranslationGroupId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WordId = table.Column<int>(type: "integer", nullable: false),
                    TranslationGroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordTranslationGroups", x => x.WordTranslationGroupId);
                    table.ForeignKey(
                        name: "FK_WordTranslationGroups_TranslationGroups_TranslationGroupId",
                        column: x => x.TranslationGroupId,
                        principalSchema: "dictionary",
                        principalTable: "TranslationGroups",
                        principalColumn: "TranslationGroupId");
                    table.ForeignKey(
                        name: "FK_WordTranslationGroups_Words_WordId",
                        column: x => x.WordId,
                        principalSchema: "dictionary",
                        principalTable: "Words",
                        principalColumn: "WordId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Words_Text",
                schema: "dictionary",
                table: "Words",
                column: "Text");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Text",
                schema: "dictionary",
                table: "Tags",
                column: "Text",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TranslationGroups_Description",
                schema: "dictionary",
                table: "TranslationGroups",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_TranslationGroupTags_TagId",
                schema: "dictionary",
                table: "TranslationGroupTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TranslationGroupTags_TranslationGroupId",
                schema: "dictionary",
                table: "TranslationGroupTags",
                column: "TranslationGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_WordTranslationGroups_TranslationGroupId",
                schema: "dictionary",
                table: "WordTranslationGroups",
                column: "TranslationGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_WordTranslationGroups_WordId",
                schema: "dictionary",
                table: "WordTranslationGroups",
                column: "WordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TranslationGroupTags",
                schema: "dictionary");

            migrationBuilder.DropTable(
                name: "WordTranslationGroups",
                schema: "dictionary");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "dictionary");

            migrationBuilder.DropTable(
                name: "TranslationGroups",
                schema: "dictionary");

            migrationBuilder.DropIndex(
                name: "IX_Words_Text",
                schema: "dictionary",
                table: "Words");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                schema: "dictionary",
                table: "Words",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Plural",
                schema: "dictionary",
                table: "Words",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Conjugation",
                schema: "dictionary",
                table: "Words",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Article",
                schema: "dictionary",
                table: "Words",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5,
                oldNullable: true);
        }
    }
}
