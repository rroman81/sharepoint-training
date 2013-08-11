/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
delete from Employee

insert into Employee values ('Guy', 'Gilbert', 'R', 'guy1@adventure-works.com', '320-555-0195', 'M', 'Production')
insert into Employee values ('Kevin', 'Brown', 'R', 'kevin0@adventure-works.com', '150-555-0189', 'M', 'Marketing')
insert into Employee values ('Roberto', 'Tamburello', 'R', 'roberto0@adventure-works.com', '212-555-0187', 'M', 'Engineering')
insert into Employee values ('Rob', 'Walters', 'R', 'rob0@adventure-works.com', '612-555-0100', 'M', 'Tool Design')
insert into Employee values ('Thierry', 'Bradley', 'R', 'thierry0@adventure-works.com', '168-555-0183', 'M', 'Tool Design')
insert into Employee values ('David', 'Bradley', 'R', 'david0@adventure-works.com', '913-555-0172', 'M', 'Marketing')