As a User I want to Search for Users so that I can find my friends and search their libraries.

#### SearchingForUserWithSingleMatchShouldReturnThatUser
**GIVEN**
Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered
**WHEN**
I Search for Users with the search string 'Lew'
**THEN**
'Joshua Lewis' gets returned

#### SearchingForUserWithSingleMatchWithWrongCaseShouldReturnThatUser
**GIVEN**
Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered
**WHEN**
I Search for Users with the search string 'lEw'
**THEN**
'Joshua Lewis' gets returned

#### SearchingForUserWithNoMatchesShouldReturnEmptyList
**GIVEN**
Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Joshua Doe', 'Audrey Hepburn' have Registered
**WHEN**
I Search for Users with the search string 'Pet'
**THEN**
An empty list is returned

#### SearchingForUserWithTwoMatchsShouldReturnTwoUsers
**GIVEN**
Users with the following names 'Joshua Lewis', 'Suzaan Hepburn', 'Josie Doe', 'Audrey Hepburn' have Registered
**WHEN**
I Search for Users with the search string 'Jos'
**THEN**
'Joshua Lewis', 'Josie Doe' gets returned

