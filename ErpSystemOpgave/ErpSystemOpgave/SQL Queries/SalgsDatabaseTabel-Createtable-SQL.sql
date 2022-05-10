/* Create */
USE H1PD021122_Gruppe3
GO

CREATE TABLE SalesOrderHeaders (
       Id INT IDENTITY(1,1) NOT NULL,
       CustomerId INT NOT NULL,
       State VARCHAR(16) NOT NULL,
       PriceSum DECIMAL NOT NULL,
       Date DATE NOT NULL,
       Street VARCHAR(16) NOT NULL,
       HouseNumber VARCHAR(32) NOT NULL,
       City VARCHAR(32) NOT NULL,
       ZipCode SMALLINT NOT NULL,
       Country VARCHAR(32) NOT NULL,
       PRIMARY KEY(Id),
       FOREIGN KEY(CustomerId) REFERENCES Customers(Id)
)
GO

CREATE TABLE OrderLines (
        Id INT IDENTITY(1,1) NOT NULL,
        ProductId INT NOT NULL,
        Quantity INT NOT NULL,
        SalesOrderHeaderId INT NOT NULL,
        Price DECIMAL NOT NULL,
        PRIMARY KEY(Id)
        FOREIGN KEY(ProductId) REFERENCES Product(Id),
        FOREIGN KEY(SalesOrderHeaderId) REFERENCES SalesOrderHeaders(Id),
)
GO
    
    /* ALTERATIONS */
USE H1PD021122_Gruppe3
GO

CREATE TABLE SalesOrderHeaders (
                                   Id INT IDENTITY(1,1) NOT NULL,
                                   CustomerId INT NOT NULL,
                                   State VARCHAR(16) NOT NULL,
                                   PriceSum DECIMAL NOT NULL,
                                   Date DATE NOT NULL,
                                   PRIMARY KEY(Id),
                                   CONSTRAINT CustomerId FOREIGN KEY (CustomerId)
                                       REFERENCES Customers (Id)
)
GO

CREATE TABLE OrderLines (
                            Id INT IDENTITY(1,1) NOT NULL,
                            ProductId INT NOT NULL,
                            Quantity INT NOT NULL,
                            PricePer DECIMAL NOT NULL,
                            PriceSum DECIMAL NOT NULL,
                            SalesOrderHeaderId INT NOT NULL
                                PRIMARY KEY(Id)
                                CONSTRAINT ProductId FOREIGN KEY (ProductId)
                                REFERENCES Products (Id),
                            CONSTRAINT SalesOrderHeaderId FOREIGN KEY (SalesOrderHeaderId)
                                REFERENCES SalesOrderHeaders (Id)
)
GO
    
ALTER TABLE SalesOrderHeaders
    ADD Street VarChar(16),
	HouseNumber VarChar(32),
	City VarChar(32),
	ZipCode SmallInt,
	Country VarChar(32);

ALTER TABLE SalesOrderHeaders
    ADD CHECK (State IN ('None', 'Created', 'Confirmed', 'Packed'))

ALTER TABLE OrderLines
    DROP COLUMN TotalPrice, SalePrice

CREATE VIEW LastPurchases
AS
SELECT CustomerId, Max(Date) FROM SalesOrderHeaders
GROUP BY CustomerId

CREATE VIEW OrderTotals
AS
SELECT SalesOrderHeaderId, SUM(Price*Quantity) AS TotalPrice FROM OrderLines
GROUP BY SalesOrderHeaderId