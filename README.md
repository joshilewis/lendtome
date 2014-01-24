lender
======

This is a small app I plan to use ro find out whether someone I know owns a copy of any specific book I'd like to read.
It is also intended to showcase some of my thoughts on application design, Object-oriented programming, and unit testing.

To run the tests you need to be able to connect to a PostgreSql database. See the connection string parameters in the App.Config in the Tests project.
You can run them with another database, then you'll need to change the FluentNhibernate configuration in DatabaseFixtureBase class (see http://www.fluentnhibernate.org/).

It is recommended that you enable Nuget package restore on the solution.