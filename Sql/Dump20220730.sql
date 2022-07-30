-- --------------------------------------------------------
-- Host:                         192.168.20.36
-- Server version:               10.7.3-MariaDB - Arch Linux
-- Server OS:                    Linux
-- HeidiSQL Version:             10.2.0.5599
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for ladoseapi
CREATE DATABASE IF NOT EXISTS `ladoseapi` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci */;
USE `ladoseapi`;

-- Dumping structure for table ladoseapi.ApplicationRole
CREATE TABLE IF NOT EXISTS `ApplicationRole` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;

-- Data exporting was unselected.

-- Dumping structure for table ladoseapi.ApplicationUser
CREATE TABLE IF NOT EXISTS `ApplicationUser` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(45) DEFAULT NULL,
  `LastName` varchar(45) DEFAULT NULL,
  `UserName` varchar(45) DEFAULT NULL,
  `PasswordHash` blob DEFAULT NULL,
  `PasswordSalt` blob DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping structure for table ladoseapi.ApplicationUserRole
CREATE TABLE IF NOT EXISTS `ApplicationUserRole` (
  `UserId` int(11) NOT NULL,
  `RoleId` int(11) NOT NULL,
  UNIQUE KEY `UserId_RoleId` (`UserId`,`RoleId`),
  KEY `FK_ApplicationUserRole_ApplicationRole` (`RoleId`),
  CONSTRAINT `FK_ApplicationUserRole_ApplicationRole` FOREIGN KEY (`RoleId`) REFERENCES `ApplicationRole` (`Id`),
  CONSTRAINT `FK_ApplicationUserRole_ApplicationUser` FOREIGN KEY (`UserId`) REFERENCES `ApplicationUser` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- Data exporting was unselected.

-- Dumping structure for table ladoseapi.BotEvent
CREATE TABLE IF NOT EXISTS `BotEvent` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Date` date NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Data exporting was unselected.

-- Dumping structure for table ladoseapi.BotEventResult
CREATE TABLE IF NOT EXISTS `BotEventResult` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BotEventId` int(11) NOT NULL,
  `Name` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `DiscordId` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `Result` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`Id`),
  KEY `FK_BotEvent_BotEvent` (`BotEventId`),
  CONSTRAINT `FK_BotEvent_BotEvent` FOREIGN KEY (`BotEventId`) REFERENCES `BotEvent` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Data exporting was unselected.

-- Dumping structure for table ladoseapi.ChallongeParticipent
CREATE TABLE IF NOT EXISTS `ChallongeParticipent` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ChallongeId` int(11) NOT NULL DEFAULT 0,
  `ChallongeTournamentId` int(11) NOT NULL DEFAULT 0,
  `Name` varchar(500) NOT NULL DEFAULT '0',
  `Rank` int(11) DEFAULT 0,
  `IsMember` bit(1) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- Data exporting was unselected.

-- Dumping structure for table ladoseapi.ChallongeTournament
CREATE TABLE IF NOT EXISTS `ChallongeTournament` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ChallongeId` int(11) NOT NULL DEFAULT 0,
  `Name` varchar(500) DEFAULT NULL,
  `GameId` int(11) DEFAULT NULL,
  `Url` varchar(255) DEFAULT NULL,
  `Sync` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`Id`),
  KEY `ChallongeTournament_GameIdPK` (`GameId`),
  CONSTRAINT `ChallongeTournament_GameIdPK` FOREIGN KEY (`GameId`) REFERENCES `Game` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- Data exporting was unselected.

-- Dumping structure for table ladoseapi.Event
CREATE TABLE IF NOT EXISTS `Event` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `Date` datetime NOT NULL,
  `SmashId` int(11) DEFAULT NULL,
  `SmashSlug` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb3;

-- Data exporting was unselected.

-- Dumping structure for table ladoseapi.Game
CREATE TABLE IF NOT EXISTS `Game` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) CHARACTER SET utf8mb3 DEFAULT NULL,
  `ImgUrl` varchar(255) CHARACTER SET utf8mb3 DEFAULT NULL,
  `WordPressTag` varchar(255) DEFAULT NULL,
  `WordPressTagOs` varchar(255) DEFAULT NULL,
  `Order` int(11) NOT NULL DEFAULT 0,
  `LongName` varchar(255) DEFAULT NULL,
  `SmashId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `name_UNIQUE` (`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4;

-- Data exporting was unselected.

-- Dumping structure for procedure ladoseapi.ImportEvent
DELIMITER //
CREATE DEFINER=`ladoseapi`@`%` PROCEDURE `ImportEvent`()
BEGIN
	INSERT INTO WPEvent (Id, Name,Slug,Date ) 
    select event_id, event_name,event_slug, event_start_date from ladose.wp_em_events
    where event_id not in (select Id from WPEvent);
    
	 INSERT INTO WPUser (Id, Name, WPUSerLogin, WPMail) 
    select ID, display_name, user_login , user_email from ladose.wp_users
    where ID not in (select Id from WPUser);

    DELETE a from WPBooking a
	 INNER JOIN ladose.wp_em_bookings b  ON b.event_id = a.WPEventId AND b.person_id = a.WPUserId
	 WHERE  b.booking_status = 3;
	 
    INSERT INTO WPBooking (WPEventId, WPUserId, Message, Meta) 
    select event_id, person_id, booking_comment , booking_meta from ladose.wp_em_bookings b 
    where (event_id , person_id) not in (select WPEventId,WPUserId from WPBooking) and b.booking_status = 1;
	 -- Supression des joueurs qui ont un event annulé. 

-- Maj des nom d'utilisateur 
--	UPDATE WPUser  a
-- INNER JOIN ladose.wp_users b  ON a.Id = b.ID
-- SET a.Name = b.display_name
-- WHERE  b.display_name != a.Name;
END//
DELIMITER ;

-- Dumping structure for table ladoseapi.Player
CREATE TABLE IF NOT EXISTS `Player` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(150) COLLATE utf8mb4_unicode_ci NOT NULL,
  `ChallongeId` int(11) DEFAULT NULL,
  `SmashId` int(11) DEFAULT NULL,
  `Gamertag` varchar(150) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  KEY `Id` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=176 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Data exporting was unselected.

-- Dumping structure for table ladoseapi.Result
CREATE TABLE IF NOT EXISTS `Result` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PlayerId` int(11) NOT NULL DEFAULT 0,
  `TournamentId` int(11) NOT NULL DEFAULT 0,
  `Point` int(11) NOT NULL DEFAULT 0,
  `Rank` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1207 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Data exporting was unselected.

-- Dumping structure for table ladoseapi.Set
CREATE TABLE IF NOT EXISTS `Set` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TournamentId` int(11) NOT NULL,
  `Player1Id` int(11) NOT NULL,
  `Player2Id` int(11) NOT NULL,
  `Player1Score` int(11) DEFAULT NULL,
  `Player2Score` int(11) DEFAULT NULL,
  `Round` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1870 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Data exporting was unselected.

-- Dumping structure for table ladoseapi.SmashParticipent
CREATE TABLE IF NOT EXISTS `SmashParticipent` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(500) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Tag` varchar(500) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `SmashId` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Data exporting was unselected.

-- Dumping structure for table ladoseapi.Todo
CREATE TABLE IF NOT EXISTS `Todo` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `User` varchar(45) NOT NULL,
  `Task` mediumtext DEFAULT NULL,
  `Done` tinyint(4) NOT NULL DEFAULT 0,
  `Created` datetime NOT NULL,
  `Deleted` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb3;

-- Data exporting was unselected.

-- Dumping structure for table ladoseapi.Tournament
CREATE TABLE IF NOT EXISTS `Tournament` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(150) COLLATE utf8mb4_unicode_ci NOT NULL,
  `SmashId` int(11) DEFAULT NULL,
  `ChallongeId` int(11) DEFAULT NULL,
  `EventId` int(11) DEFAULT NULL,
  `GameId` int(11) DEFAULT NULL,
  `Finish` bit(1) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UNIQ_SmashId` (`SmashId`)
) ENGINE=InnoDB AUTO_INCREMENT=87 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Data exporting was unselected.

-- Dumping structure for table ladoseapi.WPBooking
CREATE TABLE IF NOT EXISTS `WPBooking` (
  `WPEventId` int(11) DEFAULT NULL,
  `WPUserId` int(11) DEFAULT NULL,
  `Message` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Meta` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- Data exporting was unselected.

-- Dumping structure for table ladoseapi.WPEvent
CREATE TABLE IF NOT EXISTS `WPEvent` (
  `Id` int(11) NOT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Slug` varchar(255) DEFAULT NULL,
  `Date` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- Data exporting was unselected.

-- Dumping structure for table ladoseapi.WPUser
CREATE TABLE IF NOT EXISTS `WPUser` (
  `Id` int(11) NOT NULL,
  `Name` varchar(45) DEFAULT NULL,
  `WPUserLogin` varchar(45) DEFAULT NULL,
  `WPMail` varchar(45) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;

-- Data exporting was unselected.

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
