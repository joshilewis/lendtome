using System.Data;
using FluentMigrator;

namespace Lending.ReadModels.Relational.Migrations
{
    [Migration(20171127)]
    public class BookCoverPicture : Migration
    {
        public override void Up()
        {
            Alter.Table("LibraryBook").AddColumn("coverpicture").AsString();
        }

        public override void Down()
        {
            Delete.Column("coverpicture").FromTable("LibraryBook");
        }

    }
}
