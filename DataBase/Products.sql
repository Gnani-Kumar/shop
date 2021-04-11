CREATE TABLE Products (
ProductId int primary key identity,
    Name varchar(255) NOT NULL,
    Description varchar(255) NOT NULL,
    Price DECIMAL(20,3),
    ProdAddedDate DATE,
	Availability  varchar(10),
	Brand  varchar(255)

);
