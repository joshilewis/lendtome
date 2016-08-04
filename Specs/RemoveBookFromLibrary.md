As a User I want to Remove Books from my Library so that my Connections can see that I no longer Lend them the Book

#### RemoveBookInLibraryShouldSucceed
**GIVEN**
'User1' is a Registered User **AND** Book 'Book1' is in User1's Library 
**WHEN**
User1 Removes Book1 from her Library
**THEN**
Book1 is Removed from User1's Library

#### RemoveBookNotInLibraryShouldFail
**GIVEN**
'User1' is a Registered user **AND** Book1 is not in User1's Library
**WHEN**
User1 Removes Book1 from her Library
**THEN**
User1 is notified that Book1 is not in her Library