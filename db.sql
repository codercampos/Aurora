IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'DataBase')
BEGIN
    CREATE DATABASE Aurora
END
GO
