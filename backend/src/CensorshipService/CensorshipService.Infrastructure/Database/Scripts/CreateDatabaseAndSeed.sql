CREATE DATABASE CensorshipService
GO
       
USE CensorshipService
GO
       
CREATE TABLE SanitizedMessages
(
    Id      UNIQUEIDENTIFIER    PRIMARY KEY DEFAULT NEWID(),
    Message NVARCHAR(100)       NOT NULL
)
GO