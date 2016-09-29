using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace Tests.Migrations
{
    [Migration(20160929)]
    public class InitialCreation : Migration
    {
        public override void Up()
        {
            Create.Table("AuthenticatedUser")
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("username").AsString().NotNullable()
                .WithColumn("email").AsString().NotNullable()
                .WithColumn("picture").AsString().NotNullable()
                ;

            Create.Table("AuthenticationProvider")
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("name").AsString().NotNullable()
                .WithColumn("userid").AsString().NotNullable()
                .WithColumn("authenticateduserid").AsGuid().Nullable();
            Create.UniqueConstraint("NameAndUserId")
                .OnTable("AuthenticationProvider")
                .Columns("name", "userid");
            Create.ForeignKey("AuthenticationProvider_AuthenticatedUser")
                .FromTable("AuthenticationProvider")
                .ForeignColumn("authenticateduserid")
                .ToTable("AuthenticatedUser")
                .PrimaryColumn("id")
                .OnDeleteOrUpdate(Rule.Cascade);

            Create.Table("LibraryBook")
                .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("processid").AsGuid().NotNullable()
                .WithColumn("libraryid").AsGuid().NotNullable()
                .WithColumn("libraryname").AsString().NotNullable()
                .WithColumn("libraryadminid").AsGuid().NotNullable()
                .WithColumn("administratorpicture").AsString().NotNullable()
                .WithColumn("title").AsString().NotNullable()
                .WithColumn("author").AsString().NotNullable()
                .WithColumn("isbn").AsString().NotNullable()
                .WithColumn("publishyear").AsInt32().NotNullable()
                ;
            Create.UniqueConstraint("UniqueLibraryBook")
                .OnTable("LibraryBook")
                .Columns("libraryid", "libraryname", "libraryadminid", "administratorpicture", "title", "author", "isbn",
                    "publishyear");

            Create.Table("OpenedLibrary")
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("name").AsString().NotNullable()
                .WithColumn("administratorid").AsGuid().NotNullable()
                .WithColumn("administratorpicture").AsString().NotNullable()
                ;

            Create.Table("LibraryLink")
                .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("processid").AsGuid().NotNullable()
                .WithColumn("acceptingadministratorid").AsGuid().NotNullable()
                .WithColumn("acceptingadministratorpicture").AsString().NotNullable()
                .WithColumn("acceptinglibraryid").AsGuid().NotNullable()
                .WithColumn("acceptinglibraryname").AsString().NotNullable()
                .WithColumn("requestingadministratorid").AsGuid().NotNullable()
                .WithColumn("requestingadministratorpicture").AsString().NotNullable()
                .WithColumn("requestinglibraryid").AsGuid().NotNullable()
                .WithColumn("requestinglibraryname").AsString().NotNullable()
                ;
            Create.UniqueConstraint("UniqueLibraryLink")
                .OnTable("LibraryLink")
                .Columns("acceptingadministratorid", "acceptingadministratorpicture", "acceptinglibraryid",
                    "acceptinglibraryname", "requestingadministratorid", "requestingadministratorpicture",
                    "requestinglibraryid", "requestinglibraryname");

            Create.Table("RequestedLink")
            .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
            .WithColumn("processid").AsGuid().NotNullable()
            .WithColumn("targetadministratorid").AsGuid().NotNullable()
            .WithColumn("targetadministratorpicture").AsString().NotNullable()
            .WithColumn("targetlibraryid").AsGuid().NotNullable()
            .WithColumn("targetlibraryname").AsString().NotNullable()
            .WithColumn("requestingadministratorid").AsGuid().NotNullable()
            .WithColumn("requestingadministratorpicture").AsString().NotNullable()
            .WithColumn("requestinglibraryid").AsGuid().NotNullable()
            .WithColumn("requestinglibraryname").AsString().NotNullable()
            ;
            Create.UniqueConstraint("UniqueRequestedLink")
                .OnTable("RequestedLink")
                .Columns("targetadministratorid", "targetadministratorpicture", "targetlibraryid",
                    "targetlibraryname", "requestingadministratorid", "requestingadministratorpicture",
                    "requestinglibraryid", "requestinglibraryname");

        }

        public override void Down()
        {
            Delete.UniqueConstraint("UniqueRequestedLink").FromTable("RequestedLink");
            Delete.Table("RequestedLink");
            Delete.UniqueConstraint("UniqueLibraryLink").FromTable("LibraryLink");
            Delete.Table("LibraryLink");
            Delete.Table("OpenedLibrary");
            Delete.UniqueConstraint("UniqueLibraryBook").FromTable("LibraryBook");
            Delete.Table("LibraryBook");
            Delete.ForeignKey("AuthenticationProvider_AuthenticatedUser").OnTable("AuthenticationProvider");
            Delete.UniqueConstraint("NameAndUserId").FromTable("AuthenticationProvider");
            Delete.Table("AuthenticationProvider");
            Delete.Table("AuthenticatedUser");
        }
    }
}
