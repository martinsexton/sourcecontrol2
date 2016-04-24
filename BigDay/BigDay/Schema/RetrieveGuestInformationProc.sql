CREATE procedure RetrieveGuestInformation
	@id BigInt = null
as
begin
	SELECT G.Id, G.firstname, G.surname, G.diet_comment, G.status, G.guest_name FROM [Wedding].[dbo].[Guest] G where id = @id
end