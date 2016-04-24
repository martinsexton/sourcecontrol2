CREATE TABLE Guest(
	Id int IDENTITY PRIMARY KEY NOT NULL,
	reference_identifier int unique NOT NULL,
	firstname nvarchar(20) NOT NULL,
	surname nvarchar(20) NOT NULL,
	diet_comment nvarchar(max) NULL,
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

insert into Guest values (1,'Georgina','Duggan',NULL,'Gina',NULL,'086 856 1016','Unknown');
insert into Guest values (2,'Trevor','Horgan',NULL,'Trevor',NULL,NULL,'Unknown');
insert into Guest values (3,'Colm','Blunnie',NULL,'Colm',NULL,'086 020 3412','Unknown');
insert into Guest values (4,'Jane','Blunnie',NULL,'Jane',NULL,NULL,'Unknown');
insert into Guest values (5,'Paul','Gormley',NULL,'Paul',NULL,'087 130 2325','Unknown');
insert into Guest values (6,'Jane','Gormley',NULL,'Jane',NULL,NULL,'Unknown');
insert into Guest values (7,'David','Branley',NULL,'Dave',NULL,NULL,'Unknown');
insert into Guest values (8,'Sarah','Branley',NULL,'Sarah',NULL,NULL,'Unknown');
insert into Guest values (9,'Barry','Joyce',NULL,'Baz',NULL,'087 702 1200','Unknown');
insert into Guest values (10,'Barry','Kelly',NULL,'Baz',NULL,'087 795 9425','Unknown');
insert into Guest values (11,'Jane','Kelly',NULL,'Jane',NULL,NULL,'Unknown');
insert into Guest values (12,'Shane','Clusky',NULL,'Clusky',NULL,'087 764 6709','Unknown');
insert into Guest values (13,'Ed','Byrne',NULL,'The Chin',NULL,NULL,'Unknown');
insert into Guest values (14,'Edel','Byrne',NULL,'Edel',NULL,NULL,'Unknown');
insert into Guest values (15,'Cathal','Linnane',NULL,'Linnane',NULL,'087 614 2332','Unknown');
insert into Guest values (16,'Andrea','Mccullough',NULL,'Andrea',NULL,NULL,'Unknown');
insert into Guest values (17,'Seamus','Cuddy',NULL,'Cuddy',NULL,NULL,'Unknown');
insert into Guest values (18,'Denis','Mchugh',NULL,'Denny',NULL,NULL,'Unknown');
insert into Guest values (19,'David','Mcloughlain',NULL,'Dave',NULL,'086 059 6714','Unknown');
insert into Guest values (20,'Yune','Azpiazu',NULL,'Yune',NULL,'086 059 6714','Unknown');
insert into Guest values (21,'Johnny','Grealish',NULL,'Johnny',NULL,'086 059 6714','Unknown');
insert into Guest values (22,'Deirdre','Grealish',NULL,'Dee',NULL,'086 059 6714','Unknown');
insert into Guest values (23,'Emilia','Alguilar',NULL,'Emi',NULL,'086 059 6714','Unknown');
insert into Guest values (24,'Ondrej','Louda',NULL,'Ondrej',NULL,'086 059 6714','Unknown');
insert into Guest values (25,'Stephen','Franklin',NULL,'C',NULL,'086 059 6714','Unknown');
insert into Guest values (26,'Niamh','Franklin',NULL,'Niamh',NULL,'086 059 6714','Unknown');
insert into Guest values (27,'Gerard','Hayes',NULL,'Ger',NULL,'086 059 6714','Unknown');
insert into Guest values (28,'Clodagh','Hayes',NULL,'Clodagh',NULL,'086 059 6714','Unknown');
insert into Guest values (29,'Mike','Guiney',NULL,'Mike',NULL,'086 059 6714','Unknown');
insert into Guest values (30,'Edel','Guiney',NULL,'Edel',NULL,'086 059 6714','Unknown');
insert into Guest values (31,'Gordon','Murray',NULL,'Gordo',NULL,'086 059 6714','Unknown');
insert into Guest values (32,'Ronnie','Murray',NULL,'Ronnie',NULL,'086 059 6714','Unknown');
insert into Guest values (33,'James','Canon',NULL,'James',NULL,'086 059 6714','Unknown');
insert into Guest values (34,'Michelle','Canon',NULL,'James',NULL,'086 059 6714','Unknown');
insert into Guest values (35,'Pamela','Canon',NULL,'Pam',NULL,'086 059 6714','Unknown');
insert into Guest values (36,'Pat','Canon',NULL,'Pat',NULL,'086 059 6714','Unknown');
insert into Guest values (37,'Liam','Canon',NULL,'Liameen',NULL,'086 059 6714','Unknown');
insert into Guest values (38,'Siobhan','Jennings',NULL,'Siobhan',NULL,'086 059 6714','Unknown');
insert into Guest values (39,'Jason','McCormac',NULL,'Jayo',NULL,'086 059 6714','Unknown');
insert into Guest values (40,'Sarah','McCormac',NULL,'Jayo',NULL,'086 059 6714','Unknown');
insert into Guest values (41,'Alan','Keane',NULL,'Kano',NULL,'086 059 6714','Unknown');
insert into Guest values (42,'Mairead','Keane',NULL,'Mairead',NULL,'086 059 6714','Unknown');
insert into Guest values (43,'Kevin','Keane',NULL,'Kev',NULL,'086 059 6714','Unknown');
insert into Guest values (44,'Maura','Keane',NULL,'Maura',NULL,'086 059 6714','Unknown');
insert into Guest values (45,'Marius','Claudy',NULL,'Marius',NULL,'086 059 6714','Unknown');
insert into Guest values (46,'Michelle','Claudy',NULL,'Michelle',NULL,'086 059 6714','Unknown');
insert into Guest values (47,'Mark','Smyth',NULL,'Mark',NULL,'086 059 6714','Unknown');
insert into Guest values (48,'Fiona','Smyth',NULL,'Fiona',NULL,'086 059 6714','Unknown');
insert into Guest values (49,'Paul','Murphy',NULL,'Paul',NULL,'086 059 6714','Unknown');
insert into Guest values (50,'Catherine','Murphy',NULL,'Catherine',NULL,'086 059 6714','Unknown');
insert into Guest values (51,'Joe','Murphy',NULL,'Joe',NULL,'086 059 6714','Unknown');
insert into Guest values (52,'Nicola','Murphy',NULL,'Nicola',NULL,'086 059 6714','Unknown');

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