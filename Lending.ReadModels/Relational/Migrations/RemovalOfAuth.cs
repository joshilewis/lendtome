using System.Data;
using FluentMigrator;

namespace Lending.ReadModels.Relational.Migrations
{
    [Migration(20171121)]
    public class RemovalOfAuth : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey("AuthenticationProvider_AuthenticatedUser").OnTable("AuthenticationProvider");
            Delete.UniqueConstraint("NameAndUserId").FromTable("AuthenticationProvider");
            Delete.Table("AuthenticationProvider");
            Delete.Table("AuthenticatedUser");
        }

        public override void Down()
        {
            Create.Table("AuthenticatedUser")
                .WithColumn("id").AsString().NotNullable().PrimaryKey()
                .WithColumn("username").AsString().NotNullable()
                .WithColumn("email").AsString().NotNullable()
                .WithColumn("picture").AsString().NotNullable()
                ;

            Create.Table("AuthenticationProvider")
                .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("name").AsString().NotNullable()
                .WithColumn("userid").AsString().NotNullable()
                .WithColumn("authenticateduserid").AsString().Nullable();
            Create.UniqueConstraint("NameAndUserId")
                .OnTable("AuthenticationProvider")
                .Columns("name", "userid");
            Create.ForeignKey("AuthenticationProvider_AuthenticatedUser")
                .FromTable("AuthenticationProvider")
                .ForeignColumn("authenticateduserid")
                .ToTable("AuthenticatedUser")
                .PrimaryColumn("id")
                .OnDeleteOrUpdate(Rule.Cascade);

        }

    }
}
