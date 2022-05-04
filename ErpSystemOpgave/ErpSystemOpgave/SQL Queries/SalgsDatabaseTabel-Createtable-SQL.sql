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
    
    /* ALTERATIONS */
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