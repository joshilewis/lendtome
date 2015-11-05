As a User I want to Search for Books in my Connections' Libraries so that I can find out if any of my Connections have the Book I want to Borrow.

#### SearchingForBookNotOwnedByAnyConnectionShouldReturnEmptyList
**GIVEN**
User1 has Registered **AND** User2 has Registered **AND** User1 has Request to Connect with User2 **AND** User2 has Accepted the Connection from User1

**WHEN**
User1 Searches for a Book with the Search Term "Extreme Programming Eplained"

**THEN**
A successful result is returned with an empty list of owners

#### SearchingForBookWithNoConnectionsShouldFail
**GIVEN**
User1 has Registered

**WHEN**
User1 Searches for a Book with the Search Term "Extreme Programming Eplained"

**THEN**
A failed result is returned, with reason 'User has no Connections'

#### SearchingForBookWithSingleMatchingTitleInManyLibrariesShouldReturnAllOwners
**GIVEN**
User1, User2, User3 and User4 have all Registered, **AND** User1 is Connected to User2, User3 and User4 **AND** User2 and User4 have Added the Book ("Extreme Programming Explained", "Kent Beck") to their Libraries **AND** User2 has Added the Book "Test-Driven Development", "Kent Beck") to her Library

**WHEN**
User1 Searches for a Book with the Search Term "Extreme Programming Eplained"

**THEN**
A successful result is returned with content ('User2:("Extreme Programming Explained", "Kent Beck")', 'User4':("Extreme Programming Explained", "Kent Beck"))

#### SearchingForBookWithManyMatchingTitlesInManyLibrariesShouldReturnAllOwnersAndBooks
**GIVEN**
User1, User2, User3, User4 and User5 have all Registered, **AND** User1 is Connected to User2, User3, User4 and User5 **AND** User2 and User4 have Added the Book ("Extreme Programming Explained", "Kent Beck") to their Libraries **AND** User2 has Added the Book "Test-Driven Development", "Kent Beck") to her Library **AND** User5 has Added the Book ("Extreme Snowboard Stunts", "Some Skiier")

**WHEN**
User1 Searches for a Book with the Search Term "Extreme"

**THEN**
A successful result is returned with content ('User2:("Extreme Programming Explained", "Kent Beck")', 'User4':("Extreme Programming Explained", "Kent Beck"), 'User5':("Extreme Snowboard Stunts", "Some Skiier"))

#### SearchingForBookWithSingleMatchingAuthorInManyLibrariesShouldReturnAllOwnersAndBooks
**GIVEN**
User1, User2, User3, User4 and User5 have all Registered, **AND** User1 is Connected to User2, User3, User4 and User5 **AND** User2 and User4 have Added the Book ("Extreme Programming Explained", "Kent Beck") to their Libraries **AND** User2 has Added the Book "Test-Driven Development", "Kent Beck") to her Library **AND** User5 has Added the Book ("Extreme Snowboard Stunts", "Some Skiier")

**WHEN**
User1 Searches for a Book with the Search Term "Kent Beck"

**THEN**
A successful result is returned with content ('User2:("Extreme Programming Explained", "Kent Beck")', "Kent Beck"), 'User3':("Test-Driven Development", "Kent Beck"), 'User4':("Extreme Programming Explained"))

#### SearchingForBookWithManyMatchingTitlesAndAuthorsInManyLibrariesShouldReturnAllOwnersAndBooks
**GIVEN**
User1, User2, User3, User4, User5 and User6 have all Registered, **AND** User1 is Connected to User2, User3, User4, User5 and User6 **AND** User2 and User4 have Added the Book ("Extreme Programming Explained", "Kent Beck") to their Libraries **AND** User2 has Added the Book "Test-Driven Development", "Kent Beck") to her Library **AND** User5 has Added the Book ("Extreme Snowboard Stunts", "Some Skiier") **AND** User6 has Added the Book ("Beck: A Musical Maestro", "Some Author")

**WHEN**
User1 Searches for a Book with the Search Term "Beck"

**THEN**
A successful result is returned with content ('User2:("Extreme Programming Explained", "Kent Beck")', "Kent Beck"), 'User3':("Test-Driven Development", "Kent Beck"), 'User4':("Extreme Programming Explained")'User6':("Beck: A Musical Maestro", "Some Author"))



