
    drop table if exists lender."RegisteredUser" cascade;

    drop table if exists lender.UserAuth cascade;

    drop table if exists lender.UserAuth_Permissions cascade;

    drop table if exists lender.UserAuth_Roles cascade;

    drop table if exists lender.UserOAuthProvider cascade;

    drop table if exists lender.UserOAuthProvider_Items cascade;

    drop table if exists lender."LibraryBook" cascade;

    drop table if exists lender."UserConnection" cascade;

    create table lender."RegisteredUser" (
        Id uuid not null,
       UserName varchar(255),
       AuthUserId int8 unique,
       primary key (Id)
    );

    create table lender.UserAuth (
        Id  serial,
       CreatedDate timestamp,
       DisplayName varchar(255),
       Email varchar(255),
       FirstName varchar(255),
       LastName varchar(255),
       ModifiedDate timestamp,
       PasswordHash varchar(255),
       PrimaryEmail varchar(255),
       Salt varchar(255),
       UserName varchar(255),
       primary key (Id)
    );

    create table lender.UserAuth_Permissions (
        UserAuthID int4 not null,
       Permission varchar(255)
    );

    create table lender.UserAuth_Roles (
        UserAuthID int4 not null,
       Role varchar(255)
    );

    create table lender.UserOAuthProvider (
        Id  serial,
       AccessToken varchar(255),
       AccessTokenSecret varchar(255),
       CreatedDate timestamp,
       DisplayName varchar(255),
       Email varchar(255),
       FirstName varchar(255),
       LastName varchar(255),
       ModifiedDate timestamp,
       Provider varchar(255),
       RequestToken varchar(255),
       RequestTokenSecret varchar(255),
       UserAuthId int4,
       UserId varchar(255),
       UserName varchar(255),
       primary key (Id)
    );

    create table lender.UserOAuthProvider_Items (
        UserOAuthProviderID int4 not null,
       Value varchar(255),
       "Key" varchar(255) not null,
       primary key (UserOAuthProviderID, "Key")
    );

    create table lender."LibraryBook" (
        Id  bigserial,
       ProcessId uuid,
       OwnerId uuid,
       Title varchar(255),
       Author varchar(255),
       Isbn varchar(255),
       primary key (Id),
      unique (OwnerId, Title, Author, Isbn)
    );

    create table lender."UserConnection" (
        Id  bigserial,
       ProcessId uuid,
       RequestingUserId uuid,
       AcceptingUserId uuid,
       primary key (Id),
      unique (RequestingUserId, AcceptingUserId)
    );

    alter table lender.UserAuth_Permissions 
        add constraint FK71A9AE2FFF74C40E 
        foreign key (UserAuthID) 
        references lender.UserAuth;

    alter table lender.UserAuth_Roles 
        add constraint FK451B2ADCFF74C40E 
        foreign key (UserAuthID) 
        references lender.UserAuth;

    alter table lender.UserOAuthProvider_Items 
        add constraint FK77D5F781AE4A649B 
        foreign key (UserOAuthProviderID) 
        references lender.UserOAuthProvider;

