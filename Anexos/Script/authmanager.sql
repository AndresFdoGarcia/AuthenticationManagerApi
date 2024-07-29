CREATE TABLE `authmanager`.`persons` (
  `id` CHAR(36) PRIMARY KEY NOT NULL,
  `idnumber` VARCHAR(20) NOT NULL,
  `idtype` VARCHAR(5) NOT NULL,  
  `email` VARCHAR(45) NOT NULL,
  `firstname` VARCHAR(45) NOT NULL,
  `lastname` VARCHAR(45) NOT NULL,
  `creationdate` DATETIME DEFAULT CURRENT_TIMESTAMP,
  `fullid` VARCHAR(255) AS (REPLACE(CONCAT(idtype,idnumber),' ','')),
  `fullname` VARCHAR(255) AS (REPLACE(CONCAT(firstname,lastname),' ','')));


CREATE TABLE `authmanager`.`users` (
    `id` CHAR(36) PRIMARY KEY,
    `username` VARCHAR(45) NOT NULL,
    `password` VARCHAR(45) NOT NULL,
    `creationdate` DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (Id) REFERENCES `authmanager`.`persons`(Id)
);

DELIMITER //
use `authmanager`;
CREATE PROCEDURE GetAllUsers()
BEGIN
    SELECT p.firstname, p.lastname, p.email, u.username 
    FROM persons p
    JOIN users u ON p.id = u.id;
END //

DELIMITER ;
