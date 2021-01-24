using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Windgram.Blogging.Infrastructure.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "blogging");

            migrationBuilder.CreateTable(
                name: "post",
                schema: "blogging",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParentPostId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    Slug = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    Description = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true),
                    CoverFileId = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PostType = table.Column<int>(type: "int", nullable: false),
                    PostStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModifiedDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PublishedDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tag",
                schema: "blogging",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Alias = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    NormalizedName = table.Column<string>(type: "longtext", nullable: true),
                    Description = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    IsPublished = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModifiedDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "post_comment",
                schema: "blogging",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    CommentId = table.Column<long>(type: "bigint", nullable: false),
                    IsApproved = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_post_comment_post_PostId",
                        column: x => x.PostId,
                        principalSchema: "blogging",
                        principalTable: "post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post_content",
                schema: "blogging",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    MetaDescription = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true),
                    MetaKeyword = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    HtmlContent = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_content", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_post_content_post_PostId",
                        column: x => x.PostId,
                        principalSchema: "blogging",
                        principalTable: "post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post_rating",
                schema: "blogging",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    IsCommend = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_rating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_post_rating_post_PostId",
                        column: x => x.PostId,
                        principalSchema: "blogging",
                        principalTable: "post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post_view",
                schema: "blogging",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    HostAddress = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_view", x => x.Id);
                    table.ForeignKey(
                        name: "FK_post_view_post_PostId",
                        column: x => x.PostId,
                        principalSchema: "blogging",
                        principalTable: "post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post_tag",
                schema: "blogging",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_post_tag_post_PostId",
                        column: x => x.PostId,
                        principalSchema: "blogging",
                        principalTable: "post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_post_tag_tag_TagId",
                        column: x => x.TagId,
                        principalSchema: "blogging",
                        principalTable: "tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_post_comment_PostId",
                schema: "blogging",
                table: "post_comment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_post_rating_PostId",
                schema: "blogging",
                table: "post_rating",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_post_tag_PostId",
                schema: "blogging",
                table: "post_tag",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_post_tag_TagId",
                schema: "blogging",
                table: "post_tag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_post_view_PostId",
                schema: "blogging",
                table: "post_view",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_tag_Alias",
                schema: "blogging",
                table: "tag",
                column: "Alias",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "post_comment",
                schema: "blogging");

            migrationBuilder.DropTable(
                name: "post_content",
                schema: "blogging");

            migrationBuilder.DropTable(
                name: "post_rating",
                schema: "blogging");

            migrationBuilder.DropTable(
                name: "post_tag",
                schema: "blogging");

            migrationBuilder.DropTable(
                name: "post_view",
                schema: "blogging");

            migrationBuilder.DropTable(
                name: "tag",
                schema: "blogging");

            migrationBuilder.DropTable(
                name: "post",
                schema: "blogging");
        }
    }
}
