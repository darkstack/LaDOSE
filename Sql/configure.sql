-- phpMyAdmin SQL Dump
-- version 4.6.4
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Oct 06, 2018 at 10:12 PM
-- Server version: 5.6.40-log
-- PHP Version: 7.1.18

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `ladoseapi`
--

-- --------------------------------------------------------

--
-- Table structure for table `ApplicationUser`
--

CREATE TABLE `ApplicationUser` (
  `Id` int(11) NOT NULL,
  `FirstName` varchar(45) DEFAULT NULL,
  `LastName` varchar(45) DEFAULT NULL,
  `UserName` varchar(45) DEFAULT NULL,
  `PasswordHash` blob,
  `PasswordSalt` blob
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `Event`
--

CREATE TABLE `Event` (
  `Id` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Date` datetime NOT NULL,
  `SeasonId` int(11) NOT NULL,
  `Ranking` tinyint(4) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `Game`
--

CREATE TABLE `Game` (
  `Id` int(11) NOT NULL,
  `Name` varchar(45) CHARACTER SET utf8 DEFAULT NULL,
  `ImgUrl` varchar(255) CHARACTER SET utf8 DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `Season`
--

CREATE TABLE `Season` (
  `Id` int(11) NOT NULL,
  `Name` varchar(45) DEFAULT NULL,
  `StartDate` datetime DEFAULT NULL,
  `EndDate` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `SeasonGame`
--

CREATE TABLE `SeasonGame` (
  `SeasonId` int(11) NOT NULL,
  `GameId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `ApplicationUser`
--
ALTER TABLE `ApplicationUser`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `Event`
--
ALTER TABLE `Event`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `SeasonPK_idx` (`SeasonId`);

--
-- Indexes for table `Game`
--
ALTER TABLE `Game`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `name_UNIQUE` (`Name`);

--
-- Indexes for table `Season`
--
ALTER TABLE `Season`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Name_UNIQUE` (`Name`);

--
-- Indexes for table `SeasonGame`
--
ALTER TABLE `SeasonGame`
  ADD PRIMARY KEY (`SeasonId`,`GameId`),
  ADD KEY `GamePK_idx` (`GameId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `ApplicationUser`
--
ALTER TABLE `ApplicationUser`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT for table `Event`
--
ALTER TABLE `Event`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
--
-- AUTO_INCREMENT for table `Game`
--
ALTER TABLE `Game`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT for table `Season`
--
ALTER TABLE `Season`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
--
-- Constraints for dumped tables
--

--
-- Constraints for table `Event`
--
ALTER TABLE `Event`
  ADD CONSTRAINT `SeasonsPK` FOREIGN KEY (`SeasonId`) REFERENCES `Season` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Constraints for table `SeasonGame`
--
ALTER TABLE `SeasonGame`
  ADD CONSTRAINT `GamePK` FOREIGN KEY (`GameId`) REFERENCES `Game` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `SeasonPK` FOREIGN KEY (`SeasonId`) REFERENCES `Season` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
