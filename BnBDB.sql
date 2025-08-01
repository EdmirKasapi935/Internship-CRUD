USE [master]
GO
/****** Object:  Database [BnBDB]    Script Date: 07/28/2025 18:42:18 ******/
CREATE DATABASE [BnBDB] ON  PRIMARY 
( NAME = N'BnBDB', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\BnBDB.mdf' , SIZE = 2304KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'BnBDB_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\BnBDB_log.LDF' , SIZE = 504KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [BnBDB] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BnBDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BnBDB] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [BnBDB] SET ANSI_NULLS OFF
GO
ALTER DATABASE [BnBDB] SET ANSI_PADDING OFF
GO
ALTER DATABASE [BnBDB] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [BnBDB] SET ARITHABORT OFF
GO
ALTER DATABASE [BnBDB] SET AUTO_CLOSE ON
GO
ALTER DATABASE [BnBDB] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [BnBDB] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [BnBDB] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [BnBDB] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [BnBDB] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [BnBDB] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [BnBDB] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [BnBDB] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [BnBDB] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [BnBDB] SET  ENABLE_BROKER
GO
ALTER DATABASE [BnBDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [BnBDB] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [BnBDB] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [BnBDB] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [BnBDB] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [BnBDB] SET READ_COMMITTED_SNAPSHOT ON
GO
ALTER DATABASE [BnBDB] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [BnBDB] SET  READ_WRITE
GO
ALTER DATABASE [BnBDB] SET RECOVERY SIMPLE
GO
ALTER DATABASE [BnBDB] SET  MULTI_USER
GO
ALTER DATABASE [BnBDB] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [BnBDB] SET DB_CHAINING OFF
GO
USE [BnBDB]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 07/28/2025 18:42:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](max) NOT NULL,
	[UserEmail] [nvarchar](max) NOT NULL,
	[UserPassword] [nvarchar](max) NOT NULL,
	[UserPhone] [nvarchar](max) NOT NULL,
	[IsOwner] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Users] ([Id], [UserName], [UserEmail], [UserPassword], [UserPhone], [IsOwner]) VALUES (N'dfd0aafb-a82c-49c4-4b46-08ddcddb24c4', N'Mark Grayson', N'invincible@gmail.com', N'xsmquH1/s2xtvnjW4/YydmszX77xlIVtUr1nSc1TBK0=', N'0224445577', 0)
INSERT [dbo].[Users] ([Id], [UserName], [UserEmail], [UserPassword], [UserPhone], [IsOwner]) VALUES (N'96b389c4-df9e-4aac-940f-08ddcddd0f44', N'Owner One', N'firstowner@gmail.com', N'XBNZ9oZODhqtY7yP9LOYlTqPVjVou9e4CcC1WENPiPU=', N'0174342210', 1)
INSERT [dbo].[Users] ([Id], [UserName], [UserEmail], [UserPassword], [UserPhone], [IsOwner]) VALUES (N'a8fc7fbc-4f9a-41f4-79da-08ddcde3e775', N'Second Owner', N'secondowner@gmail.com', N'2RQZSR9K/0iz0svVYdzCGxLTx4pcDQax/k36ip8Hdto=', N'0772347851', 1)
/****** Object:  Table [dbo].[Rooms]    Script Date: 07/28/2025 18:42:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rooms](
	[Id] [uniqueidentifier] NOT NULL,
	[Owner_Id] [nvarchar](max) NOT NULL,
	[Room_Name] [nvarchar](max) NOT NULL,
	[Room_Type] [nvarchar](max) NOT NULL,
	[Room_Capacity] [int] NOT NULL,
	[Room_Location] [nvarchar](max) NOT NULL,
	[Room_PricePerNight] [float] NOT NULL,
	[Room_Description] [nvarchar](max) NOT NULL,
	[Has_Wifi] [bit] NOT NULL,
	[Has_Pool] [bit] NOT NULL,
	[Has_Kitchen] [bit] NOT NULL,
	[Has_Parking] [bit] NOT NULL,
	[Has_AirConditioning] [bit] NOT NULL,
 CONSTRAINT [PK_Rooms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Rooms] ([Id], [Owner_Id], [Room_Name], [Room_Type], [Room_Capacity], [Room_Location], [Room_PricePerNight], [Room_Description], [Has_Wifi], [Has_Pool], [Has_Kitchen], [Has_Parking], [Has_AirConditioning]) VALUES (N'6d6d0e86-adce-48ac-196b-08ddcdddda69', N'96b389c4-df9e-4aac-940f-08ddcddd0f44', N'Comfortable Penthouse', N'Standard Room', 4, N'Bremen, Germany', 220, N'       Enjoy your stay in this modern and cozy penthouse, featuring a spacious double room with a plush bed, soft linens, and plenty of natural light. Perfect for couples or solo travelers, the space includes a private bathroom, a fully equipped kitchen, and a comfortable living area with stunning city views. Quiet, clean, and centrally located—your perfect home away from home.       ', 1, 0, 1, 0, 1)
INSERT [dbo].[Rooms] ([Id], [Owner_Id], [Room_Name], [Room_Type], [Room_Capacity], [Room_Location], [Room_PricePerNight], [Room_Description], [Has_Wifi], [Has_Pool], [Has_Kitchen], [Has_Parking], [Has_AirConditioning]) VALUES (N'd274eb95-6ca8-4dc4-1b66-08ddcde223ee', N'96b389c4-df9e-4aac-940f-08ddcddd0f44', N'Wooden Shack', N'Standard Room', 1, N'Bremen, Germany', 100, N'   Escape to this charming wooden shack nestled in nature. Warm, rustic, and full of character, it offers a comfortable bed, simple amenities, and a serene atmosphere—perfect for unwinding and disconnecting. Ideal for nature lovers seeking a quiet, relaxing retreat.   ', 0, 0, 1, 0, 0)
INSERT [dbo].[Rooms] ([Id], [Owner_Id], [Room_Name], [Room_Type], [Room_Capacity], [Room_Location], [Room_PricePerNight], [Room_Description], [Has_Wifi], [Has_Pool], [Has_Kitchen], [Has_Parking], [Has_AirConditioning]) VALUES (N'0157697b-1868-492b-1b67-08ddcde223ee', N'a8fc7fbc-4f9a-41f4-79da-08ddcde3e775', N'Beach House', N'Standard Room', 2, N'Miami, Florida', 200, N' Wake up to the sound of waves in this bright and breezy beach house. With cozy interiors, ocean views, and direct access to the beach, it''s the perfect spot for a relaxing coastal escape. Ideal for couples, families, or friends looking to unwind by the sea. ', 0, 0, 0, 1, 1)
/****** Object:  Table [dbo].[Reservations]    Script Date: 07/28/2025 18:42:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reservations](
	[Id] [uniqueidentifier] NOT NULL,
	[Room_ID] [nvarchar](max) NOT NULL,
	[Customer_ID] [nvarchar](max) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[Total_price] [float] NOT NULL,
 CONSTRAINT [PK_Reservations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Reservations] ([Id], [Room_ID], [Customer_ID], [StartDate], [EndDate], [Total_price]) VALUES (N'f575fbf8-ee30-43ee-4127-08ddcde59117', N'0157697b-1868-492b-1b67-08ddcde223ee', N'dfd0aafb-a82c-49c4-4b46-08ddcddb24c4', CAST(0x84480B00 AS Date), CAST(0x88480B00 AS Date), 1000)
/****** Object:  Table [dbo].[Pictures]    Script Date: 07/28/2025 18:42:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pictures](
	[Id] [uniqueidentifier] NOT NULL,
	[Path] [nvarchar](max) NOT NULL,
	[Room_Id] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Pictures] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Pictures] ([Id], [Path], [Room_Id]) VALUES (N'd4f9d75e-f327-4e7f-192e-08ddcde0cc5a', N'SuiteNov2022_0008_1920.webp', N'6d6d0e86-adce-48ac-196b-08ddcdddda69')
INSERT [dbo].[Pictures] ([Id], [Path], [Room_Id]) VALUES (N'ac057dc7-08b9-4a84-192f-08ddcde0cc5a', N'images.jpeg', N'6d6d0e86-adce-48ac-196b-08ddcdddda69')
INSERT [dbo].[Pictures] ([Id], [Path], [Room_Id]) VALUES (N'55ea0fca-af29-41bb-1930-08ddcde0cc5a', N'pexels-photo-164595.jpeg', N'6d6d0e86-adce-48ac-196b-08ddcdddda69')
INSERT [dbo].[Pictures] ([Id], [Path], [Room_Id]) VALUES (N'7001e6bc-e456-4b4c-4953-08ddcde22bea', N'Featured-images-for-blogs-and-fu-6.webp', N'd274eb95-6ca8-4dc4-1b66-08ddcde223ee')
INSERT [dbo].[Pictures] ([Id], [Path], [Room_Id]) VALUES (N'90fb74ed-42a8-438b-4954-08ddcde22bea', N'2-images-inside-blog-post-563x56-6.webp', N'd274eb95-6ca8-4dc4-1b66-08ddcde223ee')
INSERT [dbo].[Pictures] ([Id], [Path], [Room_Id]) VALUES (N'3d1785f0-5b68-49d1-4955-08ddcde22bea', N'chick_shack_4.webp', N'd274eb95-6ca8-4dc4-1b66-08ddcde223ee')
INSERT [dbo].[Pictures] ([Id], [Path], [Room_Id]) VALUES (N'23b7536c-bcf4-45d0-4956-08ddcde22bea', N'luxurious-tropical-beach-house-private-pool-stunning-ocean-view-vacation-home-image-luxury-boasting-breathtaking-372267447.webp', N'0157697b-1868-492b-1b67-08ddcde223ee')
INSERT [dbo].[Pictures] ([Id], [Path], [Room_Id]) VALUES (N'032325a8-5630-426a-4958-08ddcde22bea', N'images (1).jpeg', N'0157697b-1868-492b-1b67-08ddcde223ee')
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 07/28/2025 18:42:18 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241107142503_InitialCreate', N'8.0.10')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241111145733_RoomMigration', N'8.0.10')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241120095502_ReservationMigration', N'8.0.10')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241218121705_PictureMigration', N'8.0.10')
