drop database if exists ga_emne7_avansert;
create database ga_emne7_avansert;
use ga_emne7_avansert;

# create user
CREATE USER IF NOT EXISTS 'ga-app'@'localhost' IDENTIFIED BY 'ga-5ecret-%';
CREATE USER IF NOT EXISTS 'ga-app'@'%' IDENTIFIED BY 'ga-5ecret-%';

GRANT ALL privileges ON Bookstore.* TO 'ga-app'@'%';
GRANT ALL privileges ON Bookstore.* TO 'ga-app'@'localhost';

FLUSH PRIVILEGES;

create table Person
(
    Id int auto_increment primary key,
    FirstName varchar(255),
    LastName varchar(255),
    Age int
);
