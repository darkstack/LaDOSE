-- --------------------------------------------------------
-- Hôte :                        api.ladose.net
-- Version du serveur:           5.7.25-log - Gentoo Linux mysql-5.7.25
-- SE du serveur:                Linux
-- HeidiSQL Version:             10.2.0.5599
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Listage de la structure de la base pour ladoseapi
CREATE DATABASE IF NOT EXISTS `ladoseapi` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `ladoseapi`;

-- Listage de la structure de la table ladoseapi. ApplicationUser
CREATE TABLE IF NOT EXISTS `ApplicationUser` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(45) DEFAULT NULL,
  `LastName` varchar(45) DEFAULT NULL,
  `UserName` varchar(45) DEFAULT NULL,
  `PasswordHash` blob,
  `PasswordSalt` blob,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table ladoseapi. ChallongeParticipent
CREATE TABLE IF NOT EXISTS `ChallongeParticipent` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ChallongeId` int(11) NOT NULL DEFAULT '0',
  `ChallongeTournamentId` int(11) NOT NULL DEFAULT '0',
  `Name` varchar(500) NOT NULL DEFAULT '0',
  `Rank` int(11) DEFAULT '0',
  `IsMember` bit(1) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=687 DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table ladoseapi. ChallongeTournament
CREATE TABLE IF NOT EXISTS `ChallongeTournament` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ChallongeId` int(11) NOT NULL DEFAULT '0',
  `Name` varchar(500) DEFAULT NULL,
  `GameId` int(11) DEFAULT NULL,
  `Url` varchar(255) DEFAULT NULL,
  `Sync` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `ChallongeTournament_GameIdPK` (`GameId`),
  CONSTRAINT `ChallongeTournament_GameIdPK` FOREIGN KEY (`GameId`) REFERENCES `Game` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=48 DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table ladoseapi. Event
CREATE TABLE IF NOT EXISTS `Event` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `Date` datetime NOT NULL,
  `SeasonId` int(11) NOT NULL,
  `Ranking` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `SeasonPK_idx` (`SeasonId`),
  CONSTRAINT `SeasonsPK` FOREIGN KEY (`SeasonId`) REFERENCES `Season` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table ladoseapi. EventGame
CREATE TABLE IF NOT EXISTS `EventGame` (
  `EventId` int(11) NOT NULL,
  `GameId` int(11) NOT NULL,
  `ChallongeId` int(11) DEFAULT NULL,
  `ChallongeUrl` varchar(250) DEFAULT NULL,
  PRIMARY KEY (`EventId`,`GameId`),
  KEY `GamePK_idx` (`GameId`),
  CONSTRAINT `EventGame_EventPK` FOREIGN KEY (`EventId`) REFERENCES `Event` (`Id`),
  CONSTRAINT `EventGame_GamePk` FOREIGN KEY (`GameId`) REFERENCES `Game` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table ladoseapi. Game
CREATE TABLE IF NOT EXISTS `Game` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) CHARACTER SET utf8 DEFAULT NULL,
  `ImgUrl` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `WordPressTag` varchar(255) DEFAULT NULL,
  `WordPressTagOs` varchar(255) DEFAULT NULL,
  `Order` int(11) NOT NULL DEFAULT '0',
  `LongName` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `name_UNIQUE` (`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la procédure ladoseapi. ImportEvent
DELIMITER //
CREATE DEFINER=`ladoseapi`@`%` PROCEDURE `ImportEvent`()
BEGIN
	INSERT INTO WPEvent (Id, Name,Slug,Date ) 
    select event_id, event_name,event_slug, event_start_date from ladose.wp_em_events
    where event_id not in (select Id from WPEvent);
    
	INSERT INTO WPUser (Id, Name, WPUSerLogin, WPMail) 
    select ID, display_name, user_login , user_email from ladose.wp_users
    where ID not in (select Id from WPUser);
    
    INSERT INTO WPBooking (WPEventId, WPUserId, Message, Meta) 
    select event_id, person_id, booking_comment , booking_meta from ladose.wp_em_bookings
    where (event_id , person_id) not in (select WPEventId,WPUserId from WPBooking);
END//
DELIMITER ;

-- Listage de la structure de la table ladoseapi. Season
CREATE TABLE IF NOT EXISTS `Season` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) DEFAULT NULL,
  `StartDate` datetime DEFAULT NULL,
  `EndDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Name_UNIQUE` (`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table ladoseapi. SeasonGame
CREATE TABLE IF NOT EXISTS `SeasonGame` (
  `SeasonId` int(11) NOT NULL,
  `GameId` int(11) NOT NULL,
  PRIMARY KEY (`SeasonId`,`GameId`),
  KEY `GamePK_idx` (`GameId`),
  CONSTRAINT `GamePK` FOREIGN KEY (`GameId`) REFERENCES `Game` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `SeasonPK` FOREIGN KEY (`SeasonId`) REFERENCES `Season` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table ladoseapi. Todo
CREATE TABLE IF NOT EXISTS `Todo` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `User` varchar(45) NOT NULL,
  `Task` mediumtext,
  `Done` tinyint(4) NOT NULL DEFAULT '0',
  `Created` datetime NOT NULL,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table ladoseapi. WPBooking
CREATE TABLE IF NOT EXISTS `WPBooking` (
  `WPEventId` int(11) DEFAULT NULL,
  `WPUserId` int(11) DEFAULT NULL,
  `Message` varchar(5000) DEFAULT NULL,
  `Meta` varchar(5000) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table ladoseapi. WPEvent
CREATE TABLE IF NOT EXISTS `WPEvent` (
  `Id` int(11) NOT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Slug` varchar(255) DEFAULT NULL,
  `Date` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table ladoseapi. WPUser
CREATE TABLE IF NOT EXISTS `WPUser` (
  `Id` int(11) NOT NULL,
  `Name` varchar(45) DEFAULT NULL,
  `WPUserLogin` varchar(45) DEFAULT NULL,
  `WPMail` varchar(45) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
