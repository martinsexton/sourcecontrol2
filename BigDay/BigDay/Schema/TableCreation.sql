CREATE TABLE [Wedding].[dbo].[Guest](
	Id int IDENTITY PRIMARY KEY NOT NULL,
	firstname nvarchar(20) NOT NULL,
	surname nvarchar(20) NOT NULL,
	diet_comment nvarchar(max) NULL,
	status nvarchar(50) NOT NULL,
	guest_name nvarchar(50) NULL
);

delete from [Wedding].[dbo].[Guest]
delete from [Wedding].[dbo].[RelationShip]

select * from [Wedding].[dbo].[Guest];
--drop table [Wedding].[dbo].[Guest];
--drop table [Wedding].[dbo].[RelationShip];


select * from guest;
