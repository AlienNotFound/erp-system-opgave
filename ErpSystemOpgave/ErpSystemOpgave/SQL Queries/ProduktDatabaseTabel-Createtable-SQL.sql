/* Create */
USE H1PD021122_Gruppe3
GO

CREATE TABLE Products (
		Id INT IDENTITY(1,1) NOT NULL,
		Name VARCHAR(16) NOT NULL,
		Description VARCHAR(50) NOT NULL,
		SalePrice DECIMAL NOT NULL,
		BuyPrice DECIMAL NOT NULL,
		InStock FLOAT NOT NULL,
		Location VARCHAR(16) NOT NULL,
		Unit VARCHAR(16) NOT NULL,
		PRIMARY KEY(Id)
)
GO
    
    /* ALTERATIONS */
USE H1PD021122_Gruppe3
GO

CREATE TABLE Products (
                          Id INT IDENTITY(1,1) NOT NULL,
                          Name VARCHAR(16) NOT NULL,
                          Description VARCHAR(50) NOT NULL,
                          SalePrice DECIMAL NOT NULL,
                          BuyPrice DECIMAL NOT NULL,
                          InStock FLOAT NOT NULL,
                          Location VARCHAR(16) NOT NULL,
                          Unit VARCHAR(16) NOT NULL,
                          AvancePercent DECIMAL NOT NULL,
                          AvanceKroner DECIMAL NOT NULL,
                          PRIMARY KEY(Id)
)
GO

ALTER TABLE Products
    ADD CHECK (Unit IN ('Hours', 'Meters', 'Kilos', 'Quantity', 'Liters'))

ALTER TABLE Products
DROP COLUMN AvancePercent, AvanceKroner