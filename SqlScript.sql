create database FurnitureManager;

use FurnitureManager;

create table if not exists User(
	Id int not null unique auto_increment,
    Username varchar(32) not null,
    Password varchar(64) not null,
    Admin bool not null,
    primary key (Id));
    
create table if not exists Product(
	Id int not null auto_increment unique,
    Title varchar(32) not null,
    Color varchar(16),
    Width int not null, 
    Height int not null, 
    Dproductepth int not null,
    Price float not null, 
    Stock int not null,
    primary key (Id));
    
create table if not exists CustomerOrder(
	Id int not null auto_increment unique, 
    CustomerName varchar(64) not null,
    DeliveryAdress varchar(64) not null,
	DeliveryDate Date not null,
    OrderStatus bool not null,
    UserId int not null,
    primary key (Id),
    foreign key (UserId) references User(Id));
    
create table if not exists ShoppingCart(
	CustomerOrderId int not null,
    ProductId int not null,
    Quantity int not null,
    primary key (CustomerOrderId, ProductId),
    foreign key (CustomerOrderId) references CustomerOrder(Id),
    foreign key (ProductId) references Product(Id));
    
create table if not exists UserActivity(
	Id int auto_increment primary key,
	UserId int not null,
    ItemId int not null,
    Description text not null,
    TableName ENUM('Product', 'CustomerOrder', 'ShoppingCart') not null,
    ActivityDateTime DateTime not null,
    foreign key (UserId) references User(Id));
    
grant all on FurnitureManager.* to zoli;

insert into UserActivity(userid, ItemId, Description, TableName, ActivityDateTime) values
	(9, 2, "szia", "Product", "2017-3-27 13:00:00");
    
delete from UserActivity 
where ActivityDateTime <= "2017-3-27 13:00:00";

insert into CustomerOrder(CustomerName, DeliveryAdress, DeliveryDate, OrderStatus, UserId) values
	("Banhidi Attila", "Zalau, Closca 47", "2017-5-1", false, 11);
    
insert into ShoppingCart(CustomerOrderId, ProductId, Quantity) values
	(1, 6, 5);
    
insert into CustomerOrder(CustomerName, DeliveryAdress, DeliveryDate, OrderStatus, UserId) values
	("Banhidi Agnes", "Zalau, Closca 47", "2017-5-10", false, 11);
    
insert into ShoppingCart(CustomerOrderId, ProductId, Quantity) values
	(3, 17, 1);
    
delete from shoppingCart;

    