CREATE SCHEMA `authmanager` ;

CREATE TABLE `authmanager`.`users` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `username` VARCHAR(45) NOT NULL,
  `password` VARCHAR(45) NOT NULL,
  `email` VARCHAR(45) NOT NULL,
  `firstname` VARCHAR(45) NOT NULL,
  `lastname` VARCHAR(45) NOT NULL,
  `rol` VARCHAR(15) NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `username_UNIQUE` (`username` ASC) VISIBLE,
  UNIQUE INDEX `email_UNIQUE` (`email` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

INSERT INTO `authmanager`.`users` (`username`, `password`, `email`, `firstname`, `lastname`, `rol`) VALUES 
('afperea', 'TmN@x', 'andres.perea@hotmail.com', 'Andres', 'Garcia', 'Admin');