-- --------------------------------------------------------------------------------
-- Name: Eric Shepard
 
-- Abstract: Employee Management System
-- --------------------------------------------------------------------------------

-- --------------------------------------------------------------------------------
-- Options
-- --------------------------------------------------------------------------------
USE dbEmployeeManager;     
SET NOCOUNT ON;  

-- --------------------------------------------------------------------------------
--						Drop Tables
-- --------------------------------------------------------------------------------

-- Drop Table Statements
IF OBJECT_ID ('TDepartments')			IS NOT NULL DROP TABLE TDepartments
IF OBJECT_ID ('TEmployees')				IS NOT NULL DROP TABLE TEmployees

-- --------------------------------------------------------------------------------
--	Create tables
-- --------------------------------------------------------------------------------
CREATE TABLE TDepartments
(
	 intDepartmentID				INTEGER	IDENTITY(1,1)			NOT NULL
	,strDepartmentName				VARCHAR(255)					NOT NULL
	,CONSTRAINT TDepartments_PK PRIMARY KEY ( intDepartmentID )
)

CREATE TABLE TEmployees
(
	 intEmployeeID					INTEGER	IDENTITY(1,1)			NOT NULL
	,strFirstName					VARCHAR(255)					NOT NULL
	,strLastName					VARCHAR(255)					NOT NULL
	,strEmail						VARCHAR(255)					NOT NULL
	,dblSalary						DECIMAL(10,2)					NOT NULL
    ,dtmHireDate					DATE							NOT NULL
    ,intDepartmentId				INTEGER							NOT NULL
	,CONSTRAINT TEmployees_PK PRIMARY KEY ( intEmployeeID )
)



-- --------------------------------------------------------------------------------
--	Establish Referential Integrity 
-- --------------------------------------------------------------------------------
--
-- #	Child							Parent						Column
-- -	-----							------						---------
-- 1	TDepartments					TEmployees					intDepartmentID

--1
ALTER TABLE TEmployees ADD CONSTRAINT TEmployees_TDepartments_FK 
FOREIGN KEY ( intDepartmentID ) REFERENCES TDepartments ( intDepartmentID ) ON DELETE CASCADE



-- Insert Dummy Data

-- 1️ TDepartments
INSERT INTO TDepartments (strDepartmentName) 
VALUES 
('HR'),
('IT'),
('Finance');

-- 1️ TEmployees
INSERT INTO TEmployees (strFirstName, strLastName, strEmail, dblSalary, dtmHireDate, intDepartmentId) 
VALUES 
('Joe', 'Jenkins', 'jenkins@mail.com', 60467.00, '2023-01-15', 1),
('Karen', 'Parky', 'Karen@mail.com', 40467.14, '2021-11-23', 3),
('Phil', 'Elioza', 'phil@mail.com', 70467.00, '2024-07-12', 2);


-- Inspect all tables with simple SELECT queries
SELECT * FROM TDepartments;
SELECT * FROM TEmployees;