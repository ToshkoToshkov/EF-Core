DROP DATABASE MinionsDB
CREATE DATABASE MinionsDB 
USE MinionsDB

CREATE TABLE Countries(Id INT PRIMARY KEY IDENTITY, [Name] VARCHAR(50))

INSERT INTO Countries
VALUES('Bulgaria'),('Greece'),('Norway'),('UK'),('Cyprus')

CREATE TABLE Towns( Id INT PRIMARY KEY IDENTITY, [Name] VARCHAR(50), CountryCode INT FOREIGN KEY REFERENCES Countries(Id))
INSERT INTO Towns
VALUES('Plovdiv',1),('Oslo',2),('Larnaka',3),('Athens',4),('London',5)

CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY, [Name] VARCHAR(50), Age INT, TownId INT FOREIGN KEY REFERENCES Towns(Id))
INSERT INTO Minions
VALUES('Stoyan',12,1),('George',22,2),('Ivan',25,3),('Kiro',30,4),('Mimi',5,5)

CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, [Name] VARCHAR(50))
INSERT INTO EvilnessFactors
VALUES('super good'),('good'),('bad'),('evil'),('super evil')

CREATE TABLE Villains(Id INT PRIMARY KEY IDENTITY, [Name] VARCHAR(50), EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))
INSERT INTO Villains
VALUES('Gru',2),('Ivo',3),('Teo',4),('Sto',5),('Pro',1)

CREATE TABLE MinionsVillains(MinionId INT FOREIGN KEY REFERENCES Minions(Id), VillainId INT FOREIGN KEY REFERENCES Villains(Id), PRIMARY KEY(MinionId,VillainId))
INSERT INTO MinionsVillains
VALUES(1,1),(2,2),(3,3),(4,4),(5,5)

--PO2

SELECT v.Name,COUNT(*) FROM MinionsVillains mv
JOIN Villains v ON v.Id=mv.VillainId
GROUP BY v.Id,v.Name
HAVING COUNT(*)>3
ORDER BY COUNT(*) DESC

--PO3

USE MinionsDB

SELECT ROW_NUMBER() OVER(ORDER BY m.Name) AS Row,m.Name,m.Age FROM MinionsVillains mv
JOIN Minions m ON m.Id=mv.MinionId
WHERE mv.VillainId=6
ORDER BY m.Name

SELECT [Name] FROM Villains
WHERE Id=1

--PO4

SELECT * FROM MinionsVillains mv
JOIN Minions m ON mv.MinionId=m.Id
JOIN Towns t ON t.Id=m.TownId
ORDER BY m.Id

SELECT * FROM EvilnessFactors
WHERE Name='Gru'

INSERT INTO MinionsVillains (MinionId,VillainId)
VALUES (@minionId,@villianid)

--PO5

SELECT * FROM Towns t
JOIN Countries c ON t.CountryCode=c.Id

CREATE PROC usp_GetOlder @Id INT
AS
UPDATE Minions
SET Age+=1
WHERE Id=@Id

EXEC usp_GetOlder 1