using System.Data;
using FluentMigrator;

namespace Lending.ReadModels.Relational.Migrations
{
    [Migration(20171122)]
    public class AdminIdType : Migration
    {
        public override void Up()
        {
            Alter.Column("administratorid").OnTable("OpenedLibrary").AsString();
            Alter.Column("libraryadminid").OnTable("LibraryBook").AsString();
            Alter.Column("acceptingadministratorid").OnTable("LibraryLink").AsString();
            Alter.Column("requestingadministratorid").OnTable("LibraryLink").AsString();
            Alter.Column("targetadministratorid").OnTable("RequestedLink").AsString();
            Alter.Column("requestingadministratorid").OnTable("RequestedLink").AsString();
        }

        public override void Down()
        {
            //Alter.Column("administratorid").OnTable("OpenedLibrary").AsGuid();
            //Alter.Column("libraryadminid").OnTable("LibraryBook").AsGuid();
            //Alter.Column("acceptingadministratorid").OnTable("LibraryLink").AsGuid();
            //Alter.Column("requestingadministratorid").OnTable("LibraryLink").AsGuid();
            //Alter.Column("targetadministratorid").OnTable("RequestedLink").AsGuid();
            //Alter.Column("requestingadministratorid").OnTable("RequestedLink").AsGuid();
        }

    }
}
