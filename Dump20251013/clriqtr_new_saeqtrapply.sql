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
-- Table structure for table `saeqtrapply`
--

DROP TABLE IF EXISTS `saeqtrapply`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `saeqtrapply` (
  `saqtrappno` varchar(45) NOT NULL,
  `empno` varchar(45) NOT NULL,
  `ownhouse` varchar(45) NOT NULL,
  `ownname` varchar(45) NOT NULL,
  `ownadd` varchar(245) NOT NULL,
  `isrent` varchar(45) NOT NULL,
  `ownrent` varchar(45) NOT NULL,
  `ishouseeightkm` varchar(45) NOT NULL,
  `neworcor` varchar(45) NOT NULL,
  `cpaccom` varchar(245) NOT NULL,
  `lowertypesel` varchar(45) NOT NULL,
  `saint` varchar(45) NOT NULL,
  `doa` varchar(45) NOT NULL,
  `toe` varchar(45) NOT NULL,
  `qtrres` varchar(45) NOT NULL,
  `empmobno` varchar(45) NOT NULL,
  `appstatus` varchar(45) NOT NULL,
  `permtemp` varchar(45) NOT NULL,
  `surname` varchar(45) NOT NULL,
  `surpost` varchar(45) NOT NULL,
  `surdesig` varchar(45) NOT NULL,
  `labcode` varchar(45) NOT NULL,
  PRIMARY KEY (`saqtrappno`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `saeqtrapply`
--

LOCK TABLES `saeqtrapply` WRITE;
/*!40000 ALTER TABLE `saeqtrapply` DISABLE KEYS */;
INSERT INTO `saeqtrapply` VALUES ('QTR/CLRI/2025/SA/1001','10979','NO','NA','NA','NA','NA','NA','null','House No:14/9, 1st Floor (F2), Baktavachalam Nagar 3rd Street\r\nAdyar, Chennai-600020','NA','SI','11/12/2024','IV','OQ','null','C','P','NA','NA','NA','100'),('QTR/CLRI/2025/SA/1002','10987','NO','NA','NA','NA','NA','NA','null','SF1, A-Block, Newtech Paradise Apts, Anna Main Road, Agaram Then, Padhuvanchery, Chennai - 126.','NA','SI','11/12/2024','IV','OQ','null','C','P','NA','NA','NA','100'),('QTR/CLRI/2025/SA/1003','11013','NO','NA','NA','NA','NA','NA','null','FLAT M NO  4U, KGEYES  CAROLINA, BHARNAI STREET, BHARTI NAGAR, VILACHERI, CHENNAI\r\nPincode - 600042','NA','SI','16/12/2024','IV','OQ','null','C','P','NA','NA','NA','100'),('QTR/CLRI/2025/SA/1004','11106','NO','NA','NA','NA','NA','NA','null','633/27 Radhapuram, Matiyari, Chinhat Lucknow, Uttar Pradesh, 226028','NA','SI','16/12/2024','IV','OQ','null','C','P','NA','NA','NA','100'),('QTR/CLRI/2025/SA/1005','11040','NO','NA','NA','NA','NA','NA','null','CT2, Paramount Pearls Apartment, Dr. Seetaram Nagar, Velachery, Chennai-42.','I','SI','16/12/2024','V','OQ','null','E','P','NA','NA','NA','100'),('QTR/CLRI/2025/SA/1006','10569','O','NA','NA','NR','0','NA','NA','NA','NI','SI','03/10/2025','III','NA','','C','P','NA','NA','NA','100'),('QTR/CLRI/2025/SA/1008','10594','O','Present Place of Residence with Full Address','Present Place of Residence with Full Address','R','5467','NA','NA','Present Place of Residence with Full Address','I','SI','08/10/2025','V','OQ','','C','P','NA','NA','NA','100'),('QTR/CMC/2025/SA/1001','2060019','NO','NA','NA','NA','NA','NA','null','4L, Kgeyes Carolina, Udhyam Nagar, Velachery, Chennai ','I','SI','11/12/2024','V','OQ','null','C','P','NA','NA','NA','102'),('QTR/CMC/2025/SA/1002','9999999','NO','NA','NA','NA','NA','NA','null','test','I','SI','11/12/2024','V','OQ','null','E','P','NA','NA','NA','102'),('QTR/CMC/2025/SA/1003','1230006','O','V.VASUGI','No.4, Third Cross Street, \r\nChittibabu Nagar. \r\nPallikaranai\r\nChennai 600100','NR','NA','NA','null','No.4, Third Cross Street, \r\nChittibabu Nagar. \r\nPallikaranai\r\nChennai 600100','NA','SI','13/12/2024','IV','OQ','null','C','P','NA','NA','NA','102'),('QTR/CMC/2025/SA/1004','2060025','NO','NA','NA','NA','NA','NA','null','H.No. 12/2, Bharathiyar Street, MG Nagar, Taramani','NA','SI','17/12/2024','III','OQ','null','C','P','NA','NA','NA','102'),('QTR/SERC/2025/SA/1001','77777','NO','NA','NA','NA','NA','NA','null','Adyar','I','SI','11/12/2024','V','OQ','null','E','P','NA','NA','NA','101'),('QTR/SERC/2025/SA/1002','5060','NO','NA','NA','NA','NA','NA','null','Type-II  (New)-9','NA','SI','11/12/2024','III','IQ','null','D','P','NA','NA','NA','101'),('QTR/SERC/2025/SA/1003','5072','NO','NA','NA','NA','NA','NA','null','No.17, Jeeva Street, Anna Nagar Extension, Velacherry - Chennai 600042','NA','SI','12/12/2024','IV','OQ','null','C','P','NA','NA','NA','101'),('QTR/SERC/2025/SA/1004','5071','NO','NA','NA','NA','NA','NA','null','No 14, Ex-service man colony, Adambakkam, chennai-88','NA','SI','12/12/2024','IV','OQ','null','C','P','NA','NA','NA','101'),('QTR/SERC/2025/SA/1005','208','NO','NA','NA','NA','NA','NA','null','13/11, F2, Throwpathy Amman Kovil 1st Street, Velachery, Chennai-600042.','NA','SI','12/12/2024','IV','OQ','null','C','P','NA','NA','NA','101'),('QTR/SERC/2025/SA/1006','202','NO','NA','NA','NA','NA','NA','null','1/24, Rajaji Nagar Main Road, Thiruvanmiyur, Chennai-41','I','SI','12/12/2024','V','OQ','null','C','P','NA','NA','NA','101'),('QTR/SERC/2025/SA/1007','183','NO','NA','NA','NR','0','NA','NA','a','NI','SI','08/10/2025','V','IQ','','C','P','NA','NA','NA','101'),('SAQTR52055327','10856','O','xxxxx','122, Adyar, Chennai-20','NR','0','NA','NA','122, Adyar, Chennai-20','NI','SI','23/09/2025','V','NA','','C','P','NA','NA','NA','100');
/*!40000 ALTER TABLE `saeqtrapply` ENABLE KEYS */;
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
