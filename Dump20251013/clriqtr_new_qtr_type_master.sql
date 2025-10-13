-- MySQL dump 10.13  Distrib 8.0.43, for Win64 (x86_64)
--
-- Host: localhost    Database: clriqtr_new
-- ------------------------------------------------------
-- Server version	8.0.43

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `qtr_type_master`
--

DROP TABLE IF EXISTS `qtr_type_master`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `qtr_type_master` (
  `qid` int NOT NULL AUTO_INCREMENT,
  `qtrdesc` varchar(120) NOT NULL,
  `qtrtype` varchar(30) NOT NULL,
  PRIMARY KEY (`qid`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `qtr_type_master`
--

LOCK TABLES `qtr_type_master` WRITE;
/*!40000 ALTER TABLE `qtr_type_master` DISABLE KEYS */;
INSERT INTO `qtr_type_master` VALUES (1,'Dispensary Quarters','DIS'),(2,'Scientist Apartments','SA'),(3,'Type I (New)','I (New)'),(4,'Type III (New)','III New'),(5,'Type IV -(Fan Type)','IV Fan'),(6,'Type IV(Old)','IV Old'),(7,'Type-II  (New)','II New'),(8,'Type-II(MS Block)','II MS'),(9,'Type-II(Old)','II Old'),(10,'Type-III (MS Block)','III MS'),(11,'Type-III (Old)','III Old'),(12,'Type-IV (New)','IV New'),(13,'Type-V  (New)','V New'),(14,'Type-V  (Old)-D','V Old'),(15,'Type-V  MS Block','V MS');
/*!40000 ALTER TABLE `qtr_type_master` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-10-13 10:17:10
