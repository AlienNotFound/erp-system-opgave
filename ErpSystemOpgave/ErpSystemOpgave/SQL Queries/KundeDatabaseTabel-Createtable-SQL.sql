/* Create */
USE H1PD021122_Gruppe3
GO

CREATE TABLE Addresses (
		Id INT IDENTITY(1,1) NOT NULL,
		Street VARCHAR(16) NOT NULL,
		HouseNumber VARCHAR(32) NOT NULL,
		City VARCHAR(32) NOT NULL,
		ZipCode SMALLINT NOT NULL,
		Country VARCHAR(32) NOT NULL,
		PRIMARY KEY(Id)
)
GO

CREATE TABLE Contacts (
		Id INT IDENTITY(1,1) NOT NULL,
		PhoneNumber VARCHAR(16) NOT NULL,
		Email VARCHAR(32) NOT NULL,
		PRIMARY KEY(Id)
)
GO

CREATE TABLE Customers (
		Id INT IDENTITY(1,1) NOT NULL,
		FirstName VARCHAR(16) NOT NULL,
		LastName VARCHAR(16) NOT NULL,
		AddressId INT NOT NULL,
		ContactId INT NOT NULL,
		PRIMARY KEY(Id),
		FOREIGN KEY(AddressId) REFERENCES Addresses(Id),
		FOREIGN KEY(ContactId) REFERENCES Contacts(Id)
)
GO
    
    /* ALTERATIONS */    
ALTER TABLE Customers
DROP CONSTRAINT ContactId, AddressId
    
ALTER TABLE Customers
    ADD CONSTRAINT fk_Contact
        FOREIGN KEY (ContactId)
        REFERENCES Contacts (Id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,

        CONSTRAINT fk_Address
        FOREIGN KEY (AddressId)
        REFERENCES Addresses (Id)
        ON DELETE CASCADE
        ON UPDATE CASCADE

SELECT Customers.*, ISNULL(LastPurchase,'0001-01-01') AS LastPurchase FROM Customers
     LEFT JOIN LastPurchases ON CustomerId = Id

SELECT Id, FirstName, LastName, AddressId, ContactId, ISNULL(LastPurchase,'0001-01-01') AS LastPurchase FROM Customers 
LEFT JOIN LastPurchases ON CustomerId = Id WHERE Customers.Id = 1