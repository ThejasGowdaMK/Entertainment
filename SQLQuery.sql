
create table Producer(
PID int identity(1,1) Primary key,
Producer_Name  varchar(255) NOT NULL,
Production_Company varchar(255) NOT NULL,
unique(Producer_Name),
Unique(Production_Company)
)

create table Actors(
AID int identity(1,1) Primary key,
_Name  varchar(255) NOT NULL,
unique(_Name)
)


create table Movies(
MID int identity(1,1) Primary key ,
Movie_Name varchar(255) NOT null,
Release_Date datetime NOT NULL,
PID int NOT NULL,
Foreign key(PID) references Producer(PID),
unique(Movie_Name)

)

create table Actors_Movie(
CMID int identity(1,1) Primary key,
MID int NOT NULL,
AID int NOT NULL,
Foreign key(MID) references Movies(MID),
Foreign key(AID) references Actors(AID)
)

