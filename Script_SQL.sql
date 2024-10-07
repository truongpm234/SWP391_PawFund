USE [master]
GO

/****** Object:  Database [PawFund]    Script Date: 07/10/2024 11:45:57 CH ******/
CREATE DATABASE [PawFund]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PawFund', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\PawFund.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PawFund_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\PawFund_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PawFund].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [PawFund] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [PawFund] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [PawFund] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [PawFund] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [PawFund] SET ARITHABORT OFF 
GO

ALTER DATABASE [PawFund] SET AUTO_CLOSE ON 
GO

ALTER DATABASE [PawFund] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [PawFund] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [PawFund] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [PawFund] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [PawFund] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [PawFund] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [PawFund] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [PawFund] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [PawFund] SET  ENABLE_BROKER 
GO

ALTER DATABASE [PawFund] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [PawFund] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [PawFund] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [PawFund] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [PawFund] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [PawFund] SET READ_COMMITTED_SNAPSHOT ON 
GO

ALTER DATABASE [PawFund] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [PawFund] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [PawFund] SET  MULTI_USER 
GO

ALTER DATABASE [PawFund] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [PawFund] SET DB_CHAINING OFF 
GO

ALTER DATABASE [PawFund] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [PawFund] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [PawFund] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [PawFund] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [PawFund] SET QUERY_STORE = ON
GO

ALTER DATABASE [PawFund] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO

ALTER DATABASE [PawFund] SET  READ_WRITE 
GO

