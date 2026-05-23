CREATE TABLE Employee (
    ID INT IDENTITY(1,1) NOT NULL,
    Name VARCHAR(100) NOT NULL,
    ManagerID INT NULL,
    Enable BIT NOT NULL CONSTRAINT DF_Employee_Enable DEFAULT 1,
    
    CONSTRAINT PK_Employee PRIMARY KEY (ID),
    CONSTRAINT FK_Employee_Manager FOREIGN KEY (ManagerID) REFERENCES Employee(ID)
);
GO

CREATE PROCEDURE update_enable_employee
    @ID INT,
    @Enable BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Employee WHERE ID = @ID AND Enable = @Enable)
    BEGIN
        SELECT ID, Name, ManagerID, Enable
        FROM Employee
        WHERE ID = @ID;
        
        RETURN;
    END;

    UPDATE Employee
    SET Enable = @Enable
    OUTPUT 
        INSERTED.ID, 
        INSERTED.Name, 
        INSERTED.ManagerID, 
        INSERTED.Enable
    WHERE ID = @ID;
END;
GO
