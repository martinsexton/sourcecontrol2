CREATE procedure RetrieveGuestInformation
	@id BigInt = null
as
begin
	select source.id,source.reference_identifier,source.firstname,
	source.surname, source.nick_name,source.diet_comment, source.email, source.mobile_number, source.status,
	rg.id as related_guest_id,rg.reference_identifier as related_guest_reference_identifier,
	rg.firstname as related_guest_firstname,rg.surname as related_guest_surname, 
	rg.nick_name as related_guest_nick_name, rg.diet_comment as related_guest_diet_comment, rg.email as related_guest_email, 
	rg.mobile_number as related_guest_mobile_number, rg.status as related_guest_status from (
	select * from guest  as g
	join RelationShip r on g.reference_identifier = r.guest_identifier_a
	union
	select * from guest g
	join RelationShip r on g.reference_identifier = r.guest_identifier_b
	union
	select *, null,null,null from guest g where 
	not reference_identifier in (
		select guest_identifier_a from RelationShip
	) and not reference_identifier in (
		select guest_identifier_b from RelationShip
	)) source
	left join Guest rg on source.guest_identifier_a = rg.reference_identifier or source.guest_identifier_b = rg.reference_identifier
	where source.id = @id
	and (rg.reference_identifier is null or (source.reference_identifier != rg.reference_identifier))
end