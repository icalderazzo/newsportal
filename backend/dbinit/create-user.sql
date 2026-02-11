CREATE LOGIN newsportalapi WITH PASSWORD = 'NewsPortal@1234!';
GO

USE NewsPortal;
GO
CREATE USER newsportalapi FOR LOGIN newsportalapi;
GO

ALTER ROLE db_datareader ADD MEMBER newsportalapi;
ALTER ROLE db_datawriter ADD MEMBER newsportalapi;
GO