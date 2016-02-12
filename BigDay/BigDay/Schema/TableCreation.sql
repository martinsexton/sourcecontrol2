CREATE TABLE Guest(
	Id int IDENTITY PRIMARY KEY NOT NULL,
	reference_identifier int unique NOT NULL,
	firstname nvarchar(20) NOT NULL,
	surname nvarchar(20) NOT NULL,
	nick_name nvarchar(20) NULL,
	email nvarchar(100) NULL,
	mobile_number nvarchar(50) NULL,
	status nvarchar(50) NOT NULL
);

CREATE TABLE RelationShip(
	guest_identifier_a int NOT NULL,
	guest_identifier_b int NOT NULL,
	relationship varchar(20) NOT NULL
);

ALTER TABLE RelationShip
ADD CONSTRAINT FK_Relationship_Guest_a FOREIGN KEY (guest_identifier_a)
	REFERENCES Guest(reference_identifier);

ALTER TABLE RelationShip
ADD CONSTRAINT FK_Relationship_Guest_b FOREIGN KEY (guest_identifier_b)
	REFERENCES Guest(reference_identifier);

select * from Guest;
select * from relationship
--drop table relationship
--drop table Guest
--delete from Guest;
--delete from RelationShip;

insert into Guest values (1,'Georgina','Duggan','Gina',NULL,'086 856 1016','Unknown');
insert into Guest values (2,'Trevor','Horgan','Trevor',NULL,NULL,'Unknown');
insert into Guest values (3,'Colm','Blunnie','Colm',NULL,'086 020 3412','Unknown');
insert into Guest values (4,'Jane','Blunnie','Jane',NULL,NULL,'Unknown');
insert into Guest values (5,'Paul','Gormley','Paul',NULL,'087 130 2325','Unknown');
insert into Guest values (6,'Jane','Gormley','Jane',NULL,NULL,'Unknown');
insert into Guest values (7,'David','Branley','Dave',NULL,NULL,'Unknown');
insert into Guest values (8,'Sarah','Branley','Sarah',NULL,NULL,'Unknown');
insert into Guest values (9,'Barry','Joyce','Baz',NULL,'087 702 1200','Unknown');
insert into Guest values (10,'Barry','Kelly','Baz',NULL,'087 795 9425','Unknown');
insert into Guest values (11,'Jane','Kelly','Jane',NULL,NULL,'Unknown');
insert into Guest values (12,'Shane','Clusky','Clusky',NULL,'087 764 6709','Unknown');
insert into Guest values (13,'Ed','Byrne','The Chin',NULL,NULL,'Unknown');
insert into Guest values (14,'Edel','Byrne','Edel',NULL,NULL,'Unknown');
insert into Guest values (15,'Cathal','Linnane','Linnane',NULL,'087 614 2332','Unknown');
insert into Guest values (16,'Andrea','Mccullough','Andrea',NULL,NULL,'Unknown');
insert into Guest values (17,'Seamus','Cuddy','Cuddy',NULL,NULL,'Unknown');
insert into Guest values (18,'Denis','Mchugh','Denny',NULL,NULL,'Unknown');
insert into Guest values (19,'David','Mcloughlain','Dave',NULL,'086 059 6714','Unknown');
insert into Guest values (20,'Yune','Azpiazu','Yune',NULL,'086 059 6714','Unknown');

insert into RelationShip values(1,2,'Married');
insert into RelationShip values(3,4,'Married');
insert into RelationShip values(5,6,'Married');
insert into RelationShip values(7,8,'Married');
insert into RelationShip values(10,11,'Married');
insert into RelationShip values(13,14,'Married');
insert into RelationShip values(16,16,'Married');
insert into RelationShip values(19,20,'Married');




select source.id,source.reference_identifier,source.firstname,
source.surname, source.nick_name, source.email, source.mobile_number, source.status,
rg.id as related_guest_id,rg.reference_identifier as related_guest_reference_identifier,
rg.firstname as related_guest_firstname,rg.surname as related_guest_surname, 
rg.nick_name as related_guest_nick_name, rg.email as related_guest_email, 
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
where source.id = 17
and (rg.reference_identifier is null or (source.reference_identifier != rg.reference_identifier))


exec RetrieveGuestInformation @id=1