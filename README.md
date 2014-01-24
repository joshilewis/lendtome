lender
======

To run the tests you need to be able to connect to a PostgreSql database. See the connection string parameters in the App.Config in the Tests project.
You can run them with another database, then you'll need to change the FluentNhibernate configuration in DatabaseFixtureBase class (see http://www.fluentnhibernate.org/).

It is recommended that you enable Nuget package restore on the solution.