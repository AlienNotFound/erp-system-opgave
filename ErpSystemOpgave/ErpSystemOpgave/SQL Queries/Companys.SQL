CREATE TABLE Companies (
		Id INT IDENTITY(1,1) NOT NULL,
		Name varchar(32) NOT NULL,
	    AddressId INT NOT NULL,
		Currency VARCHAR(4) NOT NULL
		PRIMARY KEY(Id),
		FOREIGN KEY(AddressId) REFERENCES Addresses(Id)
)