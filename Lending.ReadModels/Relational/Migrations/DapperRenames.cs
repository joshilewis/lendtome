using System.Data;
using FluentMigrator;

namespace Lending.ReadModels.Relational.Migrations
{
    [Migration(20171104)]
    public class DapperRenames : Migration
    {
        public override void Up()
        {
            Rename.Column("username").OnTable("AuthenticatedUser").To("UserName");
            Rename.Column("id").OnTable("AuthenticatedUser").To("Id");
            Rename.Column("picture").OnTable("AuthenticatedUser").To("Picture");
            Rename.Column("email").OnTable("AuthenticatedUser").To("Email");

            Rename.Column("id").OnTable("OpenedLibrary").To("Id");
            Rename.Column("name").OnTable("OpenedLibrary").To("Name");
            Rename.Column("administratorid").OnTable("OpenedLibrary").To("AdministratorId");
            Rename.Column("administratorpicture").OnTable("OpenedLibrary").To("AdministratorPicture");

            Rename.Column("id").OnTable("RequestedLink").To("Id");
            Rename.Column("processid").OnTable("RequestedLink").To("ProcessId");
            Rename.Column("requestinglibraryid").OnTable("RequestedLink").To("RequestingLibraryId");
            Rename.Column("requestinglibraryname").OnTable("RequestedLink").To("RequestingLibraryName");
            Rename.Column("requestingadministratorid").OnTable("RequestedLink").To("RequestingAdministratorId");
            Rename.Column("requestingadministratorpicture").OnTable("RequestedLink").To("RequestingAdministratorPicture");
            Rename.Column("targetlibraryid").OnTable("RequestedLink").To("TargetLibraryId");
            Rename.Column("targetlibraryname").OnTable("RequestedLink").To("TargetLibraryName");
            Rename.Column("targetadministratorid").OnTable("RequestedLink").To("TargetAdministratorId");
            Rename.Column("targetadministratorpicture").OnTable("RequestedLink").To("TargetAdministratorPicture");

            Rename.Column("id").OnTable("LibraryLink").To("Id");
            Rename.Column("processid").OnTable("LibraryLink").To("ProcessId");
            Rename.Column("requestinglibraryid").OnTable("LibraryLink").To("RequestingLibraryId");
            Rename.Column("requestinglibraryname").OnTable("LibraryLink").To("RequestingLibraryName");
            Rename.Column("requestingadministratorid").OnTable("LibraryLink").To("RequestingAdministratorId");
            Rename.Column("requestingadministratorpicture").OnTable("LibraryLink").To("RequestingAdministratorPicture");
            Rename.Column("acceptinglibraryid").OnTable("LibraryLink").To("AcceptingLibraryId");
            Rename.Column("acceptinglibraryname").OnTable("LibraryLink").To("AcceptingLibraryName");
            Rename.Column("acceptingadministratorid").OnTable("LibraryLink").To("AcceptingAdministratorId");
            Rename.Column("acceptingadministratorpicture").OnTable("LibraryLink").To("AcceptingAdministratorPicture");

            Rename.Column("id").OnTable("LibraryBook").To("Id");
            Rename.Column("processid").OnTable("LibraryBook").To("ProcessId");
            Rename.Column("libraryid").OnTable("LibraryBook").To("LibraryId");
            Rename.Column("libraryname").OnTable("LibraryBook").To("LibraryName");
            Rename.Column("libraryadminid").OnTable("LibraryBook").To("LibraryAdminId");
            Rename.Column("administratorpicture").OnTable("LibraryBook").To("AdministratorPicture");
            Rename.Column("title").OnTable("LibraryBook").To("Title");
            Rename.Column("author").OnTable("LibraryBook").To("Author");
            Rename.Column("isbn").OnTable("LibraryBook").To("Isbn");
            Rename.Column("publishyear").OnTable("LibraryBook").To("PublishYear");

        }

        public override void Down()
        {
            Rename.Column("UserName").OnTable("AuthenticatedUser").To("username");
            Rename.Column("Id").OnTable("AuthenticatedUser").To("id");
            Rename.Column("Picture").OnTable("AuthenticatedUser").To("picture");
            Rename.Column("Email").OnTable("AuthenticatedUser").To("email");

            Rename.Column("Id").OnTable("OpenedLibrary").To("id");
            Rename.Column("Name").OnTable("OpenedLibrary").To("name");
            Rename.Column("AdministratorId").OnTable("OpenedLibrary").To("administratorid");
            Rename.Column("AdministratorPicture").OnTable("OpenedLibrary").To("administratorpicture");

            Rename.Column("Id").OnTable("RequestedLink").To("id");
            Rename.Column("ProcessId").OnTable("RequestedLink").To("processid");
            Rename.Column("RequestingLibraryId").OnTable("RequestedLink").To("requestinglibraryid");
            Rename.Column("RequestingLibraryName").OnTable("RequestedLink").To("requestinglibraryname");
            Rename.Column("RequestingAdministratorId").OnTable("RequestedLink").To("requestingadministratorid");
            Rename.Column("RequestingAdministratorPicture").OnTable("RequestedLink").To("requestingadministratorpicture");
            Rename.Column("TargetLibraryId").OnTable("RequestedLink").To("targetlibraryid");
            Rename.Column("TargetLibraryName").OnTable("RequestedLink").To("targetlibraryname");
            Rename.Column("TargetAdministratorId").OnTable("RequestedLink").To("targetadministratorid");
            Rename.Column("TargetAdministratorPicture").OnTable("RequestedLink").To("targetadministratorpicture");

            Rename.Column("Id").OnTable("LibraryLink").To("id");
            Rename.Column("ProcessId").OnTable("LibraryLink").To("processid");
            Rename.Column("RequestingLibraryId").OnTable("LibraryLink").To("requestinglibraryid");
            Rename.Column("RequestingLibraryName").OnTable("LibraryLink").To("requestinglibraryname");
            Rename.Column("RequestingAdministratorId").OnTable("LibraryLink").To("requestingadministratorid");
            Rename.Column("RequestingAdministratorPicture").OnTable("LibraryLink").To("requestingadministratorpicture");
            Rename.Column("AcceptingLibraryId").OnTable("LibraryLink").To("acceptinglibraryid");
            Rename.Column("AcceptingLibraryName").OnTable("LibraryLink").To("acceptinglibraryname");
            Rename.Column("AcceptingAdministratorId").OnTable("LibraryLink").To("acceptingadministratorid");
            Rename.Column("AcceptingAdministratorPicture").OnTable("LibraryLink").To("acceptingadministratorpicture");

            Rename.Column("Id").OnTable("LibraryBook").To("id");
            Rename.Column("ProcessId").OnTable("LibraryBook").To("processid");
            Rename.Column("LibraryId").OnTable("LibraryBook").To("libraryid");
            Rename.Column("LibraryName").OnTable("LibraryBook").To("libraryname");
            Rename.Column("LibraryAdminId").OnTable("LibraryBook").To("libraryadminid");
            Rename.Column("AdministratorPicture").OnTable("LibraryBook").To("administratorpicture");
            Rename.Column("Title").OnTable("LibraryBook").To("title");
            Rename.Column("Author").OnTable("LibraryBook").To("author");
            Rename.Column("Isbn").OnTable("LibraryBook").To("isbn");
            Rename.Column("PublishYear").OnTable("LibraryBook").To("publishyear");
        }
    }
}
