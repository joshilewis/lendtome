[![Stories in Ready](https://badge.waffle.io/joshilewis/lending.png?label=ready&title=Ready)](https://waffle.io/joshilewis/lending)

[![Join the chat at https://gitter.im/joshilewis/lending](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/joshilewis/lending?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

lender
======

Once upon a time, you discovered a book you'd really like to read. Someone mentioned it in passing, or it was mentioned in one of your favourite books. The trouble was the book was then out of print. Or it wasn't available in your country, and it was prohibitively expensive to get it shipped. Fortunately you were able to ask your friends and colleagues if any of them owned the book. You managed to find someone with a copy, and who was willing to lend it to you. You reminded the owner to bring the book to your lunch date in 2 days' time. You read the book, and returned it to the owner within two weeks. The owner remembered who had borrowed his book and didn't panic that it was lost for all time. And they all lived happily ever after.

This is a small app I plan to use ro find out whether someone I know owns a copy of any specific book I'd like to read.
It is also intended to showcase some of my thoughts on application design, Object-oriented programming, and unit testing.

To run the tests you need to be able to connect to a PostgreSql database. See the connection string parameters in the App.Config in the Tests project.
You can run them with another database, then you'll need to change the FluentNhibernate configuration in DatabaseFixtureBase class (see http://www.fluentnhibernate.org/).

It is recommended that you enable Nuget package restore on the solution.

Getting started
===============

1. Install **Postgres**. This can easily be done via the **[Postgres package](https://chocolatey.org/packages/postgresql)** using [**Chocolatey**](https://chocolatey.org/).

2. Get **Lending.Web** to load in the solution. Otherwise the following error is displayed:
`The Web Application Project Lending.Web is configured to use IIS.  The Web server 'http://lend-to.me' could not be found.`
  - Configure **http://lend-to.me** in IIS
  - Add a host entry for the domain to point to 127.0.0.1