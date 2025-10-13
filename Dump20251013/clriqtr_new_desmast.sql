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
-- Table structure for table `desmast`
--

DROP TABLE IF EXISTS `desmast`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `desmast` (
  `desid` int NOT NULL,
  `desdesc` varchar(245) DEFAULT NULL,
  PRIMARY KEY (`desid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `desmast`
--

LOCK TABLES `desmast` WRITE;
/*!40000 ALTER TABLE `desmast` DISABLE KEYS */;
INSERT INTO `desmast` VALUES (1,'Director'),(2,'Distinguished Scientist'),(3,'Chief Scientist'),(4,'Senior Principal Scientist'),(5,'Principal Scientist'),(6,'Senior Scientist'),(7,'Scientist'),(8,'PTO'),(9,'PTO/ Senior Superintending Engineer (Civil)'),(10,'PTO/ Senior Superintending Engineer (Electrical)'),(11,'Senior Technical Officer (3)'),(12,'STO (3)/ Medical Officer'),(13,'STO (3)/ Superintending Engineer (Civil)'),(14,'Senior Technical Officer (2)'),(15,'STO (2) / Executive Engineer (Civil)'),(16,'STO (2)/ Medical Officer'),(17,'STO (1)/ Assistant Executive Engineer (Civil)'),(18,'Senior Technical Officer (1)'),(19,'Technical Officer'),(20,'Technical Assistant'),(21,'Senior Technician (3)'),(22,'Senior Technician (2)'),(23,'Senior Technician (1)'),(24,'Technician (2)'),(25,'Technician (1)'),(26,'Lab Assistant'),(27,'COA'),(28,'AO '),(29,'Section Officer (Gen)'),(30,'Assistant Section Officer (Gen)'),(31,'Senior Secretariat Assistant (Gen)'),(32,'Junior Secretariat Assistant (Gen)'),(33,'Controller of Finance and Accounts (COFA)'),(34,'Finance & Accounts Officer (FAO)'),(35,'Section Officer (F&A)'),(36,'Assistant Section Officer (F&A)'),(37,'Senior Secretariat Assistant (F&A) '),(38,'Junior Secretariat Assistant (F&A)'),(39,'Controller of Stores and Purchase (COSP)'),(40,'Section Officer (S&P)'),(41,'Assistant Section Officer (S&P)'),(42,'Senior Secretariat Assistant (S&P)'),(43,'Junior Secretariat Assistant (S&P)'),(44,'Principal Private Secretary'),(45,'Senior Stenographer'),(46,'Hindi Officer'),(47,'Junior Hindi Translator'),(48,'Security Assistant'),(49,'Guest House Asst'),(50,'Staff Car Driver II (2)'),(51,'MTS'),(52,'Junior Stenographer'),(53,'Senior Hindi Officer'),(54,'Junior Hindi Officer'),(55,'Private Secretary'),(56,'Sr.Controller of Administration'),(57,'Manager-cum-accountant'),(58,'Tea and Coffee Maker'),(59,'STO (1)/ Assistant Executive Engineer(Elec.)');
/*!40000 ALTER TABLE `desmast` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-10-13 10:17:11
