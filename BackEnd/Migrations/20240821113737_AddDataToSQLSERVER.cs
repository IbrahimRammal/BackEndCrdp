using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToSQLSERVER : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Codes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CodeDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Competencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CompetenceName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CompetenceType = table.Column<int>(type: "int", nullable: true),
                    CompetenceDetails = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CompetenceParentId = table.Column<int>(type: "int", nullable: true),
                    CompetenceActive = table.Column<bool>(type: "bit", nullable: true),
                    CompetenceLevel = table.Column<int>(type: "int", nullable: true),
                    UserCreated = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompetenciesVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainId = table.Column<int>(type: "int", nullable: false),
                    IdNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CompetenceName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CompetenceType = table.Column<int>(type: "int", nullable: true),
                    CompetenceDetails = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CompetenceParentId = table.Column<int>(type: "int", nullable: true),
                    CompetenceActive = table.Column<bool>(type: "bit", nullable: true),
                    CompetenceLevel = table.Column<int>(type: "int", nullable: true),
                    VersionDateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserCreated = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetenciesVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConceptTree",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConceptName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ConceptType = table.Column<int>(type: "int", nullable: true),
                    ConceptDomain = table.Column<int>(type: "int", nullable: true),
                    ConceptField = table.Column<int>(type: "int", nullable: true),
                    ConceptDetails = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ConceptParentId = table.Column<int>(type: "int", nullable: true),
                    ConceptActive = table.Column<bool>(type: "bit", nullable: true),
                    ConceptLevel = table.Column<int>(type: "int", nullable: true),
                    UserCreated = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptTree", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigCompetenciesLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompetenciesLevel = table.Column<int>(type: "int", nullable: true),
                    CompetenciesPreviousLevel = table.Column<int>(type: "int", nullable: true),
                    CompetenciesNextLevel = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigCompetenciesLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigConceptTreeLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConceptTreeLevel = table.Column<int>(type: "int", nullable: true),
                    ConceptTreePreviousLevel = table.Column<int>(type: "int", nullable: true),
                    ConceptTreeNextLevel = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigConceptTreeLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigCycleClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cycle = table.Column<int>(type: "int", nullable: true),
                    Class = table.Column<int>(type: "int", nullable: true),
                    PreviousClass = table.Column<int>(type: "int", nullable: true),
                    NextClass = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigCycleClass", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigDomainField",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainConcept = table.Column<int>(type: "int", nullable: true),
                    FiledConcept = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigDomainField", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RoleDetails = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UserModified = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UserCreated = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CLURL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SVURL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Dependencies = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    hasCHildren = table.Column<bool>(type: "bit", nullable: true),
                    Parent = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UserModified = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UserCreated = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNb = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserStatus = table.Column<bool>(type: "bit", nullable: true),
                    WorkGroup = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodesContent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeContentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CodeContentDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CodeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Code_Content", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Code_Content_Codes",
                        column: x => x.CodeId,
                        principalTable: "Codes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompetenciesClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CId = table.Column<int>(type: "int", nullable: true),
                    ClassId = table.Column<int>(type: "int", nullable: true),
                    UserCreated = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetenciesClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetenciesClass_Competencies",
                        column: x => x.CId,
                        principalTable: "Competencies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompetenciesConceptField",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CId = table.Column<int>(type: "int", nullable: true),
                    ConceptFieldId = table.Column<int>(type: "int", nullable: true),
                    UserCreated = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetenciesConceptField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetenciesConceptField_Competencies",
                        column: x => x.CId,
                        principalTable: "Competencies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompetenciesCross",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CompMainID = table.Column<int>(type: "int", nullable: true),
                    CompSubID = table.Column<int>(type: "int", nullable: true),
                    UserCreated = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetenciesCross", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetenciesCross_CompetenciesMain",
                        column: x => x.CompMainID,
                        principalTable: "Competencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompetenciesCross_CompetenciesSub",
                        column: x => x.Id,
                        principalTable: "Competencies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompetenciesDomain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CId = table.Column<int>(type: "int", nullable: true),
                    DomainId = table.Column<int>(type: "int", nullable: true),
                    UserCreated = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetenciesDomain", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetenciesDomain_Competencies",
                        column: x => x.CId,
                        principalTable: "Competencies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompetenciesSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Step = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StepUserId = table.Column<int>(type: "int", nullable: true),
                    StepComment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StepStatus = table.Column<int>(type: "int", nullable: true),
                    StepDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Cid = table.Column<int>(type: "int", nullable: true),
                    UserCreated = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetenciesSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetenciesSteps_Competencies",
                        column: x => x.Cid,
                        principalTable: "Competencies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompetenciesConceptTree",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CId = table.Column<int>(type: "int", nullable: true),
                    ConceptTreeId = table.Column<int>(type: "int", nullable: true),
                    UserCreated = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetenciesConceptTree", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetenciesConceptTree_Competencies",
                        column: x => x.CId,
                        principalTable: "Competencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompetenciesConceptTree_ConceptTree",
                        column: x => x.ConceptTreeId,
                        principalTable: "ConceptTree",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ConceptTreeClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CTId = table.Column<int>(type: "int", nullable: true),
                    ClassId = table.Column<int>(type: "int", nullable: true),
                    UserCreated = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserModified = table.Column<int>(type: "int", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptTreeClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConceptTreeClass_ConceptTree",
                        column: x => x.CTId,
                        principalTable: "ConceptTree",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    CanView = table.Column<bool>(type: "bit", nullable: true),
                    CanEdit = table.Column<bool>(type: "bit", nullable: true),
                    CanDelete = table.Column<bool>(type: "bit", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UserModified = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UserCreated = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleServices_Roles",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoleServices_Services",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserRoleId = table.Column<int>(type: "int", nullable: true),
                    Class = table.Column<int>(type: "int", nullable: true),
                    Domain = table.Column<int>(type: "int", nullable: true),
                    ConceptFiled = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRolePermissions_UserRoles",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodesContent_CodeId",
                table: "CodesContent",
                column: "CodeId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetenciesClass_CId",
                table: "CompetenciesClass",
                column: "CId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetenciesConceptField_CId",
                table: "CompetenciesConceptField",
                column: "CId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetenciesConceptTree_CId",
                table: "CompetenciesConceptTree",
                column: "CId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetenciesConceptTree_ConceptTreeId",
                table: "CompetenciesConceptTree",
                column: "ConceptTreeId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetenciesCross_CompMainID",
                table: "CompetenciesCross",
                column: "CompMainID");

            migrationBuilder.CreateIndex(
                name: "IX_CompetenciesDomain_CId",
                table: "CompetenciesDomain",
                column: "CId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetenciesSteps_Cid",
                table: "CompetenciesSteps",
                column: "Cid");

            migrationBuilder.CreateIndex(
                name: "IX_ConceptTreeClass_CTId",
                table: "ConceptTreeClass",
                column: "CTId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleServices_RoleId",
                table: "RoleServices",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleServices_ServiceId",
                table: "RoleServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePermissions_UserRoleId",
                table: "UserRolePermissions",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodesContent");

            migrationBuilder.DropTable(
                name: "CompetenciesClass");

            migrationBuilder.DropTable(
                name: "CompetenciesConceptField");

            migrationBuilder.DropTable(
                name: "CompetenciesConceptTree");

            migrationBuilder.DropTable(
                name: "CompetenciesCross");

            migrationBuilder.DropTable(
                name: "CompetenciesDomain");

            migrationBuilder.DropTable(
                name: "CompetenciesSteps");

            migrationBuilder.DropTable(
                name: "CompetenciesVersions");

            migrationBuilder.DropTable(
                name: "ConceptTreeClass");

            migrationBuilder.DropTable(
                name: "ConfigCompetenciesLevel");

            migrationBuilder.DropTable(
                name: "ConfigConceptTreeLevel");

            migrationBuilder.DropTable(
                name: "ConfigCycleClass");

            migrationBuilder.DropTable(
                name: "ConfigDomainField");

            migrationBuilder.DropTable(
                name: "RoleServices");

            migrationBuilder.DropTable(
                name: "UserRolePermissions");

            migrationBuilder.DropTable(
                name: "Codes");

            migrationBuilder.DropTable(
                name: "Competencies");

            migrationBuilder.DropTable(
                name: "ConceptTree");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
