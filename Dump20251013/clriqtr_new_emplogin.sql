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
-- Table structure for table `emplogin`
--

DROP TABLE IF EXISTS `emplogin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `emplogin` (
  `empno` varchar(16) NOT NULL,
  `pwd` varchar(45) NOT NULL,
  `lab` varchar(45) NOT NULL,
  PRIMARY KEY (`empno`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `emplogin`
--

LOCK TABLES `emplogin` WRITE;
/*!40000 ALTER TABLE `emplogin` DISABLE KEYS */;
INSERT INTO `emplogin` VALUES ('10533','kCV10581053366','100'),('10569','Jeevan2015','100'),('10576','Chandran10576','100'),('10594','Kanimozhi90','100'),('10612','SRinivasan98','100'),('10673','arunavm73','100'),('10674','NAGAPPANc643','100'),('10712','Kennedyclri123','100'),('10788','Earth143','100'),('10798','Kumaran2022','100'),('10802','Geetha123','100'),('10813','Landau123','100'),('10835','Vennilaa11','100'),('10839','123Qwerty','100'),('10841','Shanmugam44','100'),('10847','Xlclri14','100'),('10848','Vedanatham77','100'),('10849','28Vimalakumar','100'),('10851','sangeetha23','100'),('10856','Clri2022','100'),('10858','Clri1234','100'),('10860','karthikspdc10','100'),('10862','Karnish09','100'),('10863','CLRIscientist63','100'),('10866','Moorthichem21','100'),('10869','ArKr99ArKr','100'),('10871','Yasmin2211','100'),('10872','CSIPTngri1711','100'),('10886','Ayyaclri78','100'),('10889','Swarnavinayak28','100'),('10903','Nagamma123','100'),('10906','clri1234','100'),('10914','Niranjan3009','100'),('10916','Saththu911','100'),('10917','Kalaiyarasu28','100'),('10918','bsa1mtb2','100'),('10919','hari2023','100'),('10920','Bathula10920','100'),('10921','Rattu1988','100'),('10926','Mahesh2017','100'),('10933','Sadiqul11','100'),('10939','Sundar2016','100'),('10940','Sugan1969','100'),('10950','27jan1996','100'),('10951','clri1234','100'),('10959','Durgaasri23','100'),('10961','Theviper123','100'),('10962','Jayanthi18','100'),('10971','Akshay0609','100'),('10974','Ganesan1980','100'),('10977','Clri2020','100'),('10978','indian12','100'),('10979','Siva1628','100'),('10981','Banusree11','100'),('10982','Bhuvana1975','100'),('10985','Ambika21','100'),('10986','Sathya1990','100'),('10987','rrr161192','100'),('10988','Indra1990','100'),('10990','Swetha143','100'),('10991','Abishekrio23','100'),('10992','Skyislimit14','100'),('10995','Pondi123','100'),('10996','Thanigai1234','100'),('10998','Ashish1992','100'),('10999','Gopi1738','100'),('11001','muthuP93','100'),('11004','Avinash1994','100'),('11005','Rathi123','100'),('11008','Akash1992','100'),('11009','Jiten625','100'),('11012','Psankar92','100'),('11013','Riniam2022','100'),('11025','Csir1234','100'),('11026','Swetha96ck','100'),('11027','Gowtham123','100'),('11028','Archana0396','100'),('11029','Haripriyankkan1996','100'),('11030','Kailasam97ha','100'),('11031','Onecsir1234','100'),('11032','Logesh1983','100'),('11035','Kannan1965','100'),('11036','Scholar18','100'),('11038','m390CLRI','100'),('11040','bala2023','100'),('11041','Kmdixit123','100'),('11042','Kmdixit123','100'),('11043','ranjani9988','100'),('11044','TataNexon96','100'),('11045','Jagadish88','100'),('11046','Kailash95','100'),('11047','kmsridhar2202','100'),('11048','Rakesh123','100'),('11049','Ngeetha79','100'),('11053','QWERTY123','100'),('11054','mithren2010L','100'),('11057','Sharma97','100'),('11058','Manikantha95','100'),('11060','CLRIhari12','100'),('11061','Gokul108','100'),('11063','Ngayathri97','100'),('11064','Priya696','100'),('11065','Push1234','100'),('11066','Vjk02794','100'),('11067','Sureshkumarp93','100'),('11068','Sahil123','100'),('11069','Sawan123','100'),('11070','Vimalav99','100'),('11072','umashankar2000','100'),('11073','Vijay798865','100'),('11079','Hemalathar1505','100'),('11082','Jagathisan95','100'),('11085','Gokukrishna11','100'),('11088','vIKAS123','100'),('11090','Ak678623','100'),('11091','Dushyant2004','100'),('11092','Seema123','100'),('11096','Sivakrishna1998','100'),('11097','Sivachnadran41','100'),('11098','Akshay1234','100'),('11100','Ankitpatel11','100'),('11102','SKali1996','100'),('11103','MAHAgaja2003','100'),('11104','Gopinath97','100'),('11105','Josephkj01','100'),('11106','shailukant24','100'),('114','Sudha69m','101'),('117','Abraham117','101'),('1230002','Padhu123','102'),('1230006','Deens300566','102'),('1230050','Niveditha123','102'),('12345','Palani@123','100'),('13056','Vani13056','101'),('13072','Rasmak90','101'),('15023','csiR1234','101'),('15025','Mani1911','101'),('16032','Yuvaraj123','101'),('16033','IndukumaR02','101'),('165','Leela1980','101'),('174','Chennai97','101'),('18021','Bharath123','101'),('183','vimalMOHAN34','101'),('185','Chikku31','101'),('198','Naviya2016','101'),('201','Kansserc16','101'),('202','Appa061010','101'),('205','Venkat1990','101'),('2050017','Sinchana123','102'),('2060019','09334Rishi','102'),('2060025','Yakub6542','102'),('208','Krisarun43','101'),('2150016','rusha3103','102'),('2310014','Anand764','102'),('5047','Ksk25047','101'),('5053','Hariserc12','101'),('5055','Onecsir1234','101'),('5056','Clriq303','101'),('5058','Puni0203','101'),('5060','Mirthu16','101'),('5061','ICTserc123','101'),('5065','Lakshmi2505','101'),('5069','Elamani2001','101'),('5071','Powervinoth12','101'),('5072','Atmega328','101'),('5073','Maniserc21','101'),('5181','Sibi5181@','100'),('77777','Prathyush11','101'),('8060','Bala1234','101'),('8064','Anjali0216','101'),('8069','Mahe6664','101'),('83','Sathish83','101'),('89','Bharath89','101'),('9999999','Prathyush11','102');
/*!40000 ALTER TABLE `emplogin` ENABLE KEYS */;
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
