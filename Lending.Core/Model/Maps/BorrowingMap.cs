namespace Lending.Core.Model.Maps
{
    public class BorrowingMap : BaseMap<Borrowing>
    {
        public BorrowingMap()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.Borrower)
                .Column("BorrowerId")
                .Cascade.None()
                ;

            References(x => x.Ownership)
                .Column("OwnershipId")
                .Cascade.None()
                ;

        }
    }
}
