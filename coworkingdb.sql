-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema coworkingdb
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema coworkingdb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `coworkingdb` DEFAULT CHARACTER SET utf8 ;
USE `coworkingdb` ;

-- -----------------------------------------------------
-- Table `coworkingdb`.`Area`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `coworkingdb`.`Area` (
  `idArea` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(200) NOT NULL,
  `Info` VARCHAR(200) NOT NULL,
  PRIMARY KEY (`idArea`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `coworkingdb`.`Workstation`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `coworkingdb`.`Workstation` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Description` VARCHAR(200) NULL,
  `Opening` TIME NULL,
  `Closing` TIME NULL,
  `MaxReservations` INT NULL,
  `idArea` INT NOT NULL,
  PRIMARY KEY (`Id`, `idArea`),
  INDEX `fk_Workstation_Area1_idx` (`idArea` ASC) VISIBLE,
  CONSTRAINT `fk_Workstation_Area1`
    FOREIGN KEY (`idArea`)
    REFERENCES `coworkingdb`.`Area` (`idArea`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `coworkingdb`.`Customer`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `coworkingdb`.`Customer` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(200) NOT NULL,
  `Address` VARCHAR(200) NOT NULL,
  `Email` VARCHAR(200) NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `coworkingdb`.` Booking`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `coworkingdb`.` Booking` (
  `Id` INT NOT NULL,
  `DateStart` DATETIME NULL,
  `DateEnd` DATETIME NULL,
  `LastUpdate` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  `idClient` INT NOT NULL,
  `idWorkstation` INT NOT NULL,
  `Notes` VARCHAR(200) NULL,
  PRIMARY KEY (`Id`, `idWorkstation`),
  INDEX `fk_Prenotation_Client_idx` (`idClient` ASC) VISIBLE,
  INDEX `fk_Prenotation_Workstation1_idx` (`idWorkstation` ASC) VISIBLE,
  CONSTRAINT `fk_Prenotation_Client`
    FOREIGN KEY (`idClient`)
    REFERENCES `coworkingdb`.`Customer` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Prenotation_Workstation1`
    FOREIGN KEY (`idWorkstation`)
    REFERENCES `coworkingdb`.`Workstation` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `coworkingdb`.`Features`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `coworkingdb`.`Features` (
  `idFeatures` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  `Description` VARCHAR(200) NOT NULL,
  PRIMARY KEY (`idFeatures`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `coworkingdb`.`Features_has_Workstation`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `coworkingdb`.`Features_has_Workstation` (
  `Features_idFeatures` INT NOT NULL,
  `Workstation_Id` INT NOT NULL,
  PRIMARY KEY (`Features_idFeatures`, `Workstation_Id`),
  INDEX `fk_Features_has_Workstation_Workstation1_idx` (`Workstation_Id` ASC) VISIBLE,
  INDEX `fk_Features_has_Workstation_Features1_idx` (`Features_idFeatures` ASC) VISIBLE,
  CONSTRAINT `fk_Features_has_Workstation_Features1`
    FOREIGN KEY (`Features_idFeatures`)
    REFERENCES `coworkingdb`.`Features` (`idFeatures`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Features_has_Workstation_Workstation1`
    FOREIGN KEY (`Workstation_Id`)
    REFERENCES `coworkingdb`.`Workstation` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `coworkingdb`.`Status`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `coworkingdb`.`Status` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Status` VARCHAR(200) NOT NULL,
  `idBooking` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `fk_Status_ Booking1_idx` (`idBooking` ASC) VISIBLE,
  CONSTRAINT `fk_Status_ Booking1`
    FOREIGN KEY (`idBooking`)
    REFERENCES `coworkingdb`.` Booking` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
