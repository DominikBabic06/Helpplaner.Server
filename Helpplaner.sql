CREATE DATABASE HELPPLANER;
USE HELPPLANER;

-- Nutzer-Tabelle
CREATE TABLE "User"
(
    "ID" INT IDENTITY(1,1) PRIMARY KEY,
    "Password"  VARCHAR(255),
    "Username" VARCHAR(100),
    "Email" VARCHAR(150),
	"IsSysAdmin" BIT  ,
);

-- Projekt-Tabelle
CREATE TABLE Project 
(
    "ID" INT IDENTITY(1,1) PRIMARY KEY,
    "Name" VARCHAR(100),
    "Description" TEXT
);

-- Arbeitspaket-Tabelle
CREATE TABLE WorkPackage 
(
    "ID" INT IDENTITY(1,1) PRIMARY KEY,
    "Name" VARCHAR(100),
    "ProjectID" INT,
    "Description" TEXT,
    "ExpectedTime" INT, 
	"RealTime" INT,
    "Responsible" INT, -- Verweis auf Nutzer-ID, 
    "Status" TEXT
     FOREIGN KEY ("ProjectID") REFERENCES "Project"(ID),
	 FOREIGN KEY ("Responsible") REFERENCES "User"(ID) 
);

-- Kommentar-Tabelle
CREATE TABLE Comment 
(
	"ID" INT IDENTITY(1,1) PRIMARY KEY,
    "CreatorID" INT,  -- Verweis auf Nutzer-ID
    "WorkPackageID" INT,
    "Text" TEXT,
    FOREIGN KEY ("CreatorID") REFERENCES "User"("ID"),
    FOREIGN KEY ("WorkPackageID") REFERENCES "WorkPackage"("ID")
);

-- Arbeitssitzung-Tabelle
CREATE TABLE WorkSession 
(
    "ID" INT IDENTITY(1,1) PRIMARY KEY,
    "WorkPackageID" INT,
    "CreatorID" INT,  -- Verweis auf Nutzer-ID
    "WorkTime" INT,
    FOREIGN KEY ("WorkPackageID") REFERENCES "WorkPackage"("ID"),
    FOREIGN KEY ("CreatorID") REFERENCES "User"("ID")
);

-- Kategorie-Tabelle
CREATE TABLE Category
(
   "ID" INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50)
);

-- Projekte-Nutzer-Beziehung
CREATE TABLE ProjectUser 
(
    UserID INT,
    ProjectID INT,
    Administrator BIT,  -- 1 für Administrator, 0 für normaler Nutzer
    PRIMARY KEY (UserID, ProjectID),
    FOREIGN KEY (UserID) REFERENCES "User"(ID),
    FOREIGN KEY (ProjectID) REFERENCES Project(ID)
);

-- Arbeitspaket-Kategorie-Beziehung
CREATE TABLE WorkpackageCategory
(
    WorkPackageID INT,
    CategoryID INT,
    PRIMARY KEY (WorkPackageID, CategoryID),
    FOREIGN KEY (WorkPackageID) REFERENCES "WorkPackage"(ID),
    FOREIGN KEY (CategoryID) REFERENCES "Category"(ID)
);

-- Arbeitspaket-Arbeitspaket-Beziehung (Vorgänger/Nachfolger)
CREATE TABLE WorkpackageRelation
(
    PredecessorID INT,
    SuccessorID INT,
    PRIMARY KEY (PredecessorID, SuccessorID),
    FOREIGN KEY (PredecessorID) REFERENCES "WorkPackage"(ID),
    FOREIGN KEY (SuccessorID) REFERENCES "WorkPackage"(ID)
);

