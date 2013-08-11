CREATE TABLE [dbo].[Employee]
(
	[EmployeeKey] [int] NOT NULL PRIMARY KEY IDENTITY(1,1) ,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[MiddleName] [nvarchar](50) NULL,
	[EmailAddress] [nvarchar](50) NULL,
	[Phone] [nvarchar](25) NULL,
	[Gender] [nchar](1) NULL,
	[DepartmentName] [nvarchar](50) NULL
)
