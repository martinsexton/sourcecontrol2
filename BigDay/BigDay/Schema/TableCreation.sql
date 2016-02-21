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
insert into Guest values (21,'Johnny','Grealish','Johnny',NULL,'086 059 6714','Unknown');
insert into Guest values (22,'Deirdre','Grealish','Dee',NULL,'086 059 6714','Unknown');
insert into Guest values (23,'Emilia','Alguilar','Emi',NULL,'086 059 6714','Unknown');
insert into Guest values (24,'Ondrej','Louda','Ondrej',NULL,'086 059 6714','Unknown');
insert into Guest values (25,'Stephen','Franklin','C',NULL,'086 059 6714','Unknown');
insert into Guest values (26,'Niamh','Franklin','Niamh',NULL,'086 059 6714','Unknown');
insert into Guest values (27,'Gerard','Hayes','Ger',NULL,'086 059 6714','Unknown');
insert into Guest values (28,'Clodagh','Hayes','Clodagh',NULL,'086 059 6714','Unknown');
insert into Guest values (29,'Mike','Guiney','Mike',NULL,'086 059 6714','Unknown');
insert into Guest values (30,'Edel','Guiney','Edel',NULL,'086 059 6714','Unknown');
insert into Guest values (31,'Gordon','Murray','Gordo',NULL,'086 059 6714','Unknown');
insert into Guest values (32,'Ronnie','Murray','Ronnie',NULL,'086 059 6714','Unknown');
insert into Guest values (33,'James','Canon','James',NULL,'086 059 6714','Unknown');
insert into Guest values (34,'Michelle','Canon','James',NULL,'086 059 6714','Unknown');
insert into Guest values (35,'Pamela','Canon','Pam',NULL,'086 059 6714','Unknown');
insert into Guest values (36,'Pat','Canon','Pat',NULL,'086 059 6714','Unknown');
insert into Guest values (37,'Liam','Canon','Liameen',NULL,'086 059 6714','Unknown');
insert into Guest values (38,'Siobhan','Jennings','Siobhan',NULL,'086 059 6714','Unknown');
insert into Guest values (39,'Jason','McCormac','Jayo',NULL,'086 059 6714','Unknown');
insert into Guest values (40,'Sarah','McCormac','Jayo',NULL,'086 059 6714','Unknown');
insert into Guest values (41,'Alan','Keane','Kano',NULL,'086 059 6714','Unknown');
insert into Guest values (42,'Mairead','Keane','Mairead',NULL,'086 059 6714','Unknown');
insert into Guest values (43,'Kevin','Keane','Kev',NULL,'086 059 6714','Unknown');
insert into Guest values (44,'Maura','Keane','Maura',NULL,'086 059 6714','Unknown');
insert into Guest values (45,'Marius','Claudy','Marius',NULL,'086 059 6714','Unknown');
insert into Guest values (46,'Michelle','Claudy','Michelle',NULL,'086 059 6714','Unknown');
insert into Guest values (47,'Mark','Smyth','Mark',NULL,'086 059 6714','Unknown');
insert into Guest values (48,'Fiona','Smyth','Fiona',NULL,'086 059 6714','Unknown');
insert into Guest values (49,'Paul','Murphy','Paul',NULL,'086 059 6714','Unknown');
insert into Guest values (50,'Catherine','Murphy','Catherine',NULL,'086 059 6714','Unknown');
insert into Guest values (51,'Joe','Murphy','Joe',NULL,'086 059 6714','Unknown');
insert into Guest values (52,'Nicola','Murphy','Nicola',NULL,'086 059 6714','Unknown');

insert into RelationShip values(1,2,'Married');
insert into RelationShip values(3,4,'Married');
insert into RelationShip values(5,6,'Married');
insert into RelationShip values(7,8,'Married');
insert into RelationShip values(10,11,'Married');
insert into RelationShip values(13,14,'Married');
insert into RelationShip values(15,16,'Married');
insert into RelationShip values(19,20,'Married');
insert into RelationShip values(21,22,'Married');
insert into RelationShip values(23,24,'Married');
insert into RelationShip values(25,26,'Married');
insert into RelationShip values(27,28,'Married');
insert into RelationShip values(29,30,'Married');
insert into RelationShip values(31,32,'Married');
insert into RelationShip values(33,34,'Married');
insert into RelationShip values(35,36,'Married');
insert into RelationShip values(37,38,'Married');
insert into RelationShip values(39,40,'Married');
insert into RelationShip values(41,42,'Married');
insert into RelationShip values(43,44,'Married');
insert into RelationShip values(45,46,'Married');
insert into RelationShip values(47,48,'Married');
insert into RelationShip values(49,50,'Married');
insert into RelationShip values(51,52,'Married');




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