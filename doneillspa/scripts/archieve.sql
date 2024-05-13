/****** Script for SelectTopNRows command from SSMS  ******/
SELECT Id FROM [dbo].[Timesheet] where status = 3 and WeekStarting < DATEADD(DAY,-30,GETDATE())
update Timesheet set Status = 5 where Id in (SELECT Id FROM [dbo].[Timesheet] where status = 3 and WeekStarting < DATEADD(DAY,-30,GETDATE()))