CREATE TABLE ApplicationUser
(
	ApplicationUser_Key  nvarchar(32) NOT NULL Primary key,
	AvatarFile bigint null,
	constraint FK_ApplicationUser_AspNetUsers foreign key (ApplicationUser_Key) references AspNetUsers,
) 


CREATE TABLE Note
	(
	Id uniqueidentifier NOT NULL Primary Key,
	CreatedDateTime DATETIME2 NOT NULL,
	[Content] nvarchar(MAX) NULL,
	UserId nvarchar(32) not null,
	constraint FK_Note_ApplicationUser foreign key (UserId) references ApplicationUser,
	)

	create table Files (
  Id uniqueidentifier primary key,
  [FileName] nvarchar(255) not null,
  CreatedDateTime datetime not null,
  [Length] bigint NOT NULL,
  ObjectId uniqueidentifier null,
  IsDeleted bit not null,
  CreatedById nvarchar(32) null,
  constraint FK_Issue_ApplicationUser foreign key (CreatedById) references ApplicationUser,
)