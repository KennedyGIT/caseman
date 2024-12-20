USE [master]
GO
/****** Object:  Database [CaseManOrganisationDb]    Script Date: 12/5/2024 9:48:41 PM ******/
CREATE DATABASE [CaseManOrganisationDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CaseManOrganisationDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\CaseManOrganisationDb.mdf' , SIZE = 3264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'CaseManOrganisationDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\CaseManOrganisationDb_log.ldf' , SIZE = 832KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [CaseManOrganisationDb] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CaseManOrganisationDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CaseManOrganisationDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CaseManOrganisationDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CaseManOrganisationDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CaseManOrganisationDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CaseManOrganisationDb] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [CaseManOrganisationDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET RECOVERY FULL 
GO
ALTER DATABASE [CaseManOrganisationDb] SET  MULTI_USER 
GO
ALTER DATABASE [CaseManOrganisationDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CaseManOrganisationDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CaseManOrganisationDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CaseManOrganisationDb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [CaseManOrganisationDb] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'CaseManOrganisationDb', N'ON'
GO
USE [CaseManOrganisationDb]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 12/5/2024 9:48:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Organisations]    Script Date: 12/5/2024 9:48:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organisations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationName] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[LastUpdatedAt] [datetime2](7) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_Organisations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [CaseManOrganisationDb] SET  READ_WRITE 
GO
