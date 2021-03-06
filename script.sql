USE [master]
GO
/****** Object:  Database [SinavPortal]    Script Date: 05.03.2018 23:42:17 ******/
CREATE DATABASE [SinavPortal]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SinavPortal', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\SinavPortal.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SinavPortal_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\SinavPortal_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [SinavPortal] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SinavPortal].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SinavPortal] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SinavPortal] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SinavPortal] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SinavPortal] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SinavPortal] SET ARITHABORT OFF 
GO
ALTER DATABASE [SinavPortal] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SinavPortal] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SinavPortal] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SinavPortal] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SinavPortal] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SinavPortal] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SinavPortal] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SinavPortal] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SinavPortal] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SinavPortal] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SinavPortal] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SinavPortal] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SinavPortal] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SinavPortal] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SinavPortal] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SinavPortal] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SinavPortal] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SinavPortal] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SinavPortal] SET  MULTI_USER 
GO
ALTER DATABASE [SinavPortal] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SinavPortal] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SinavPortal] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SinavPortal] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [SinavPortal] SET DELAYED_DURABILITY = DISABLED 
GO
USE [SinavPortal]
GO
/****** Object:  Table [dbo].[cevaplar]    Script Date: 05.03.2018 23:42:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cevaplar](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[KTG_id] [int] NOT NULL,
	[soruID] [int] NOT NULL,
	[cevap] [varchar](200) NOT NULL,
	[cevapDokuman] [varchar](100) NULL,
 CONSTRAINT [PK_cevaplar] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[degerlendirme]    Script Date: 05.03.2018 23:42:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[degerlendirme](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[katId] [int] NOT NULL,
	[kullanici_id] [int] NOT NULL,
	[puan] [int] NOT NULL,
	[yorum] [varchar](256) NOT NULL,
 CONSTRAINT [PK_degerlendirme] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[girisler]    Script Date: 05.03.2018 23:42:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[girisler](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[kid] [int] NOT NULL,
	[kullanici_adi] [varchar](50) NOT NULL,
	[parola] [varchar](40) NOT NULL,
	[durum] [varchar](20) NOT NULL,
 CONSTRAINT [PK_girisler] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[kategoriler]    Script Date: 05.03.2018 23:42:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[kategoriler](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[kategoriAdi] [varchar](50) NOT NULL,
	[parentId] [int] NULL,
 CONSTRAINT [PK_kategoriler] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[kullanici_cevaplar]    Script Date: 05.03.2018 23:42:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[kullanici_cevaplar](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[klnc_id] [int] NOT NULL,
	[KTGID] [int] NOT NULL,
	[soruid] [int] NOT NULL,
	[klnc_cevap] [varchar](200) NOT NULL,
 CONSTRAINT [PK_kullanici_cevaplar] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[kullanicilar]    Script Date: 05.03.2018 23:42:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[kullanicilar](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ad] [varchar](50) NOT NULL,
	[soyad] [varchar](50) NOT NULL,
	[e_mail] [varchar](60) NOT NULL,
	[uyelik_tarihi] [varchar](15) NOT NULL,
 CONSTRAINT [PK_kullanicilar] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[resimler]    Script Date: 05.03.2018 23:42:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[resimler](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[KTG_ID] [int] NOT NULL,
	[srd_id] [int] NOT NULL,
	[cvp_id] [int] NOT NULL,
	[soruResim] [varchar](100) NULL,
 CONSTRAINT [PK_resimler] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[secenekler]    Script Date: 05.03.2018 23:42:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[secenekler](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Ktg_id] [int] NOT NULL,
	[soru_id] [int] NOT NULL,
	[secenek1] [varchar](150) NOT NULL,
	[secenek2] [varchar](150) NOT NULL,
	[secenek3] [varchar](150) NOT NULL,
	[secenek4] [varchar](150) NOT NULL,
 CONSTRAINT [PK_secenekler] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[sinavIstatistik]    Script Date: 05.03.2018 23:42:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[sinavIstatistik](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Kullanici_ID] [int] NOT NULL,
	[KAT_ID] [int] NOT NULL,
	[dogruSay] [int] NULL,
	[yanlisSay] [int] NULL,
	[puan] [int] NULL,
 CONSTRAINT [PK_sinavIstatistik] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[sorular]    Script Date: 05.03.2018 23:42:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[sorular](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[kategoriId] [int] NOT NULL,
	[soru] [varchar](1000) NOT NULL,
	[soruZorluk] [varchar](20) NOT NULL,
 CONSTRAINT [PK_sorular] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[cevaplar]  WITH CHECK ADD  CONSTRAINT [FK_cevaplar_kategoriler] FOREIGN KEY([KTG_id])
REFERENCES [dbo].[kategoriler] ([id])
GO
ALTER TABLE [dbo].[cevaplar] CHECK CONSTRAINT [FK_cevaplar_kategoriler]
GO
ALTER TABLE [dbo].[cevaplar]  WITH CHECK ADD  CONSTRAINT [FK_cevaplar_sorular] FOREIGN KEY([soruID])
REFERENCES [dbo].[sorular] ([id])
GO
ALTER TABLE [dbo].[cevaplar] CHECK CONSTRAINT [FK_cevaplar_sorular]
GO
ALTER TABLE [dbo].[degerlendirme]  WITH CHECK ADD  CONSTRAINT [FK_degerlendirme_kategoriler] FOREIGN KEY([katId])
REFERENCES [dbo].[kategoriler] ([id])
GO
ALTER TABLE [dbo].[degerlendirme] CHECK CONSTRAINT [FK_degerlendirme_kategoriler]
GO
ALTER TABLE [dbo].[degerlendirme]  WITH CHECK ADD  CONSTRAINT [FK_degerlendirme_kullanicilar] FOREIGN KEY([kullanici_id])
REFERENCES [dbo].[kullanicilar] ([id])
GO
ALTER TABLE [dbo].[degerlendirme] CHECK CONSTRAINT [FK_degerlendirme_kullanicilar]
GO
ALTER TABLE [dbo].[girisler]  WITH CHECK ADD  CONSTRAINT [FK_girisler_kullanicilar] FOREIGN KEY([kid])
REFERENCES [dbo].[kullanicilar] ([id])
GO
ALTER TABLE [dbo].[girisler] CHECK CONSTRAINT [FK_girisler_kullanicilar]
GO
ALTER TABLE [dbo].[kullanici_cevaplar]  WITH CHECK ADD  CONSTRAINT [FK_kullanici_cevaplar_kategoriler] FOREIGN KEY([KTGID])
REFERENCES [dbo].[kategoriler] ([id])
GO
ALTER TABLE [dbo].[kullanici_cevaplar] CHECK CONSTRAINT [FK_kullanici_cevaplar_kategoriler]
GO
ALTER TABLE [dbo].[kullanici_cevaplar]  WITH CHECK ADD  CONSTRAINT [FK_kullanici_cevaplar_kullanicilar] FOREIGN KEY([klnc_id])
REFERENCES [dbo].[kullanicilar] ([id])
GO
ALTER TABLE [dbo].[kullanici_cevaplar] CHECK CONSTRAINT [FK_kullanici_cevaplar_kullanicilar]
GO
ALTER TABLE [dbo].[kullanici_cevaplar]  WITH CHECK ADD  CONSTRAINT [FK_kullanici_cevaplar_sorular] FOREIGN KEY([soruid])
REFERENCES [dbo].[sorular] ([id])
GO
ALTER TABLE [dbo].[kullanici_cevaplar] CHECK CONSTRAINT [FK_kullanici_cevaplar_sorular]
GO
ALTER TABLE [dbo].[resimler]  WITH CHECK ADD  CONSTRAINT [FK_resimler_cevaplar] FOREIGN KEY([cvp_id])
REFERENCES [dbo].[cevaplar] ([id])
GO
ALTER TABLE [dbo].[resimler] CHECK CONSTRAINT [FK_resimler_cevaplar]
GO
ALTER TABLE [dbo].[resimler]  WITH CHECK ADD  CONSTRAINT [FK_resimler_kategoriler] FOREIGN KEY([KTG_ID])
REFERENCES [dbo].[kategoriler] ([id])
GO
ALTER TABLE [dbo].[resimler] CHECK CONSTRAINT [FK_resimler_kategoriler]
GO
ALTER TABLE [dbo].[resimler]  WITH CHECK ADD  CONSTRAINT [FK_resimler_resimler] FOREIGN KEY([id])
REFERENCES [dbo].[resimler] ([id])
GO
ALTER TABLE [dbo].[resimler] CHECK CONSTRAINT [FK_resimler_resimler]
GO
ALTER TABLE [dbo].[resimler]  WITH CHECK ADD  CONSTRAINT [FK_resimler_sorular] FOREIGN KEY([srd_id])
REFERENCES [dbo].[sorular] ([id])
GO
ALTER TABLE [dbo].[resimler] CHECK CONSTRAINT [FK_resimler_sorular]
GO
ALTER TABLE [dbo].[secenekler]  WITH CHECK ADD  CONSTRAINT [FK_secenekler_kategoriler] FOREIGN KEY([Ktg_id])
REFERENCES [dbo].[kategoriler] ([id])
GO
ALTER TABLE [dbo].[secenekler] CHECK CONSTRAINT [FK_secenekler_kategoriler]
GO
ALTER TABLE [dbo].[secenekler]  WITH CHECK ADD  CONSTRAINT [FK_secenekler_sorular] FOREIGN KEY([soru_id])
REFERENCES [dbo].[sorular] ([id])
GO
ALTER TABLE [dbo].[secenekler] CHECK CONSTRAINT [FK_secenekler_sorular]
GO
ALTER TABLE [dbo].[sinavIstatistik]  WITH CHECK ADD  CONSTRAINT [FK_sinavIstatistik_kategoriler] FOREIGN KEY([KAT_ID])
REFERENCES [dbo].[kategoriler] ([id])
GO
ALTER TABLE [dbo].[sinavIstatistik] CHECK CONSTRAINT [FK_sinavIstatistik_kategoriler]
GO
ALTER TABLE [dbo].[sinavIstatistik]  WITH CHECK ADD  CONSTRAINT [FK_sinavIstatistik_kullanicilar] FOREIGN KEY([Kullanici_ID])
REFERENCES [dbo].[kullanicilar] ([id])
GO
ALTER TABLE [dbo].[sinavIstatistik] CHECK CONSTRAINT [FK_sinavIstatistik_kullanicilar]
GO
ALTER TABLE [dbo].[sorular]  WITH CHECK ADD  CONSTRAINT [FK_sorular_kategoriler] FOREIGN KEY([kategoriId])
REFERENCES [dbo].[kategoriler] ([id])
GO
ALTER TABLE [dbo].[sorular] CHECK CONSTRAINT [FK_sorular_kategoriler]
GO
USE [master]
GO
ALTER DATABASE [SinavPortal] SET  READ_WRITE 
GO
