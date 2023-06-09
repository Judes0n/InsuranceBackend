USE [master]
GO
/****** Object:  Database [InsuranceDB]    Script Date: 29-04-2023 08:07:01 PM ******/
CREATE DATABASE [InsuranceDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'InsuranceDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\InsuranceDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'InsuranceDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\InsuranceDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [InsuranceDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [InsuranceDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [InsuranceDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [InsuranceDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [InsuranceDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [InsuranceDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [InsuranceDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [InsuranceDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [InsuranceDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [InsuranceDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [InsuranceDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [InsuranceDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [InsuranceDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [InsuranceDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [InsuranceDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [InsuranceDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [InsuranceDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [InsuranceDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [InsuranceDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [InsuranceDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [InsuranceDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [InsuranceDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [InsuranceDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [InsuranceDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [InsuranceDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [InsuranceDB] SET  MULTI_USER 
GO
ALTER DATABASE [InsuranceDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [InsuranceDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [InsuranceDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [InsuranceDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [InsuranceDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [InsuranceDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [InsuranceDB] SET QUERY_STORE = OFF
GO
USE [InsuranceDB]
GO
/****** Object:  Table [dbo].[AgentCompany]    Script Date: 29-04-2023 08:07:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentCompany](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[agentID] [int] NOT NULL,
	[companyID] [int] NOT NULL,
	[referral] [varchar](50) NOT NULL,
	[status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Agents]    Script Date: 29-04-2023 08:07:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Agents](
	[agentID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NOT NULL,
	[agentName] [varchar](50) NOT NULL,
	[gender] [varchar](10) NOT NULL,
	[phoneNum] [varchar](15) NOT NULL,
	[dob] [varchar](50) NOT NULL,
	[email] [varchar](50) NOT NULL,
	[address] [varchar](50) NOT NULL,
	[grade] [int] NOT NULL,
	[profilePic] [varchar](50) NOT NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_Agents] PRIMARY KEY CLUSTERED 
(
	[agentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientDeaths]    Script Date: 29-04-2023 08:07:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientDeaths](
	[clientDeathID] [int] IDENTITY(1,1) NOT NULL,
	[clientPolicyID] [int] NOT NULL,
	[dod] [varchar](20) NOT NULL,
	[startDate] [varchar](20) NOT NULL,
	[claimAmount] [money] NOT NULL,
 CONSTRAINT [PK_ClientDeaths] PRIMARY KEY CLUSTERED 
(
	[clientDeathID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientPolicy]    Script Date: 29-04-2023 08:07:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientPolicy](
	[clientPolicyID] [int] IDENTITY(1,1) NOT NULL,
	[clientID] [int] NOT NULL,
	[policyTermID] [int] NOT NULL,
	[nomineeID] [int] NOT NULL,
	[startDate] [varchar](20) NOT NULL,
	[expDate] [varchar](20) NOT NULL,
	[counter] [int] NULL,
	[status] [int] NULL,
	[referral] [varchar](50) NOT NULL,
	[agentID] [int] NOT NULL,
 CONSTRAINT [PK_ClientPolicy] PRIMARY KEY CLUSTERED 
(
	[clientPolicyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 29-04-2023 08:07:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[clientID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NOT NULL,
	[clientName] [varchar](50) NOT NULL,
	[gender] [varchar](10) NOT NULL,
	[dob] [varchar](10) NOT NULL,
	[address] [varchar](50) NOT NULL,
	[profilePic] [varchar](50) NOT NULL,
	[phoneNum] [varchar](15) NOT NULL,
	[email] [varchar](30) NOT NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[clientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Companies]    Script Date: 29-04-2023 08:07:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[companyID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NOT NULL,
	[companyName] [varchar](50) NOT NULL,
	[address] [varchar](50) NOT NULL,
	[email] [varchar](50) NOT NULL,
	[phoneNum] [varchar](15) NOT NULL,
	[profilePic] [varchar](50) NOT NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
	[companyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feedbacks]    Script Date: 29-04-2023 08:07:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedbacks](
	[fid] [int] IDENTITY(1,1) NOT NULL,
	[feed] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Feedbacks] PRIMARY KEY CLUSTERED 
(
	[fid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Maturity]    Script Date: 29-04-2023 08:07:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Maturity](
	[maturityID] [int] IDENTITY(1,1) NOT NULL,
	[clientPolicyID] [int] NOT NULL,
	[maturityDate] [varchar](20) NOT NULL,
	[claimAmount] [money] NOT NULL,
	[startDate] [varchar](20) NOT NULL,
 CONSTRAINT [PK_Maturity] PRIMARY KEY CLUSTERED 
(
	[maturityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Nominees]    Script Date: 29-04-2023 08:07:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nominees](
	[nomineeID] [int] IDENTITY(1,1) NOT NULL,
	[clientID] [int] NOT NULL,
	[nomineeName] [varchar](50) NOT NULL,
	[relation] [varchar](20) NOT NULL,
	[address] [varchar](50) NOT NULL,
	[phoneNum] [varchar](15) NOT NULL,
 CONSTRAINT [PK_Nominees] PRIMARY KEY CLUSTERED 
(
	[nomineeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 29-04-2023 08:07:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[paymentID] [int] IDENTITY(1,1) NOT NULL,
	[clientPolicyID] [int] NOT NULL,
	[transactionID] [varchar](50) NOT NULL,
	[time] [varchar](50) NOT NULL,
	[amount] [money] NOT NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[paymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Policies]    Script Date: 29-04-2023 08:07:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Policies](
	[policyID] [int] IDENTITY(1,1) NOT NULL,
	[companyID] [int] NOT NULL,
	[policytypeID] [int] NOT NULL,
	[policyName] [varchar](50) NOT NULL,
	[timePeriod] [int] NOT NULL,
	[policyAmount] [money] NOT NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_Policies] PRIMARY KEY CLUSTERED 
(
	[policyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PolicyTerms]    Script Date: 29-04-2023 08:07:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PolicyTerms](
	[policyTermID] [int] IDENTITY(1,1) NOT NULL,
	[policyID] [int] NOT NULL,
	[period] [int] NOT NULL,
	[terms] [int] NOT NULL,
	[premiumAmount] [money] NOT NULL,
 CONSTRAINT [PK_PolicyTerms] PRIMARY KEY CLUSTERED 
(
	[policyTermID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PolicyType]    Script Date: 29-04-2023 08:07:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PolicyType](
	[policytypeID] [int] IDENTITY(1,1) NOT NULL,
	[policytypeName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Type] PRIMARY KEY CLUSTERED 
(
	[policytypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Premium]    Script Date: 29-04-2023 08:07:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Premium](
	[premiumID] [int] IDENTITY(1,1) NOT NULL,
	[clientPolicyID] [int] NOT NULL,
	[dateOfPenalty] [varchar](20) NOT NULL,
	[penalty] [money] NOT NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_Premium] PRIMARY KEY CLUSTERED 
(
	[premiumID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 29-04-2023 08:07:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[userID] [int] IDENTITY(1,1) NOT NULL,
	[userName] [varchar](50) NOT NULL,
	[password] [varchar](50) NOT NULL,
	[type] [int] NOT NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_Login] PRIMARY KEY CLUSTERED 
(
	[userID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AgentCompany]  WITH CHECK ADD  CONSTRAINT [FK_AgentCompany_Agents] FOREIGN KEY([agentID])
REFERENCES [dbo].[Agents] ([agentID])
GO
ALTER TABLE [dbo].[AgentCompany] CHECK CONSTRAINT [FK_AgentCompany_Agents]
GO
ALTER TABLE [dbo].[AgentCompany]  WITH CHECK ADD  CONSTRAINT [FK_AgentCompany_Companies] FOREIGN KEY([companyID])
REFERENCES [dbo].[Companies] ([companyID])
GO
ALTER TABLE [dbo].[AgentCompany] CHECK CONSTRAINT [FK_AgentCompany_Companies]
GO
ALTER TABLE [dbo].[Agents]  WITH CHECK ADD  CONSTRAINT [FK_Agents_Users] FOREIGN KEY([userID])
REFERENCES [dbo].[Users] ([userID])
GO
ALTER TABLE [dbo].[Agents] CHECK CONSTRAINT [FK_Agents_Users]
GO
ALTER TABLE [dbo].[ClientDeaths]  WITH CHECK ADD  CONSTRAINT [FK_ClientDeaths_ClientPolicy] FOREIGN KEY([clientPolicyID])
REFERENCES [dbo].[ClientPolicy] ([clientPolicyID])
GO
ALTER TABLE [dbo].[ClientDeaths] CHECK CONSTRAINT [FK_ClientDeaths_ClientPolicy]
GO
ALTER TABLE [dbo].[ClientPolicy]  WITH CHECK ADD  CONSTRAINT [FK_ClientPolicy_Agents] FOREIGN KEY([agentID])
REFERENCES [dbo].[Agents] ([agentID])
GO
ALTER TABLE [dbo].[ClientPolicy] CHECK CONSTRAINT [FK_ClientPolicy_Agents]
GO
ALTER TABLE [dbo].[ClientPolicy]  WITH CHECK ADD  CONSTRAINT [FK_ClientPolicy_Clients] FOREIGN KEY([clientID])
REFERENCES [dbo].[Clients] ([clientID])
GO
ALTER TABLE [dbo].[ClientPolicy] CHECK CONSTRAINT [FK_ClientPolicy_Clients]
GO
ALTER TABLE [dbo].[ClientPolicy]  WITH CHECK ADD  CONSTRAINT [FK_ClientPolicy_Nominees] FOREIGN KEY([nomineeID])
REFERENCES [dbo].[Nominees] ([nomineeID])
GO
ALTER TABLE [dbo].[ClientPolicy] CHECK CONSTRAINT [FK_ClientPolicy_Nominees]
GO
ALTER TABLE [dbo].[ClientPolicy]  WITH CHECK ADD  CONSTRAINT [FK_ClientPolicy_PolicyTerms] FOREIGN KEY([policyTermID])
REFERENCES [dbo].[PolicyTerms] ([policyTermID])
GO
ALTER TABLE [dbo].[ClientPolicy] CHECK CONSTRAINT [FK_ClientPolicy_PolicyTerms]
GO
ALTER TABLE [dbo].[Clients]  WITH CHECK ADD  CONSTRAINT [FK_Clients_Users] FOREIGN KEY([userID])
REFERENCES [dbo].[Users] ([userID])
GO
ALTER TABLE [dbo].[Clients] CHECK CONSTRAINT [FK_Clients_Users]
GO
ALTER TABLE [dbo].[Companies]  WITH CHECK ADD  CONSTRAINT [FK_Companies_Users] FOREIGN KEY([userID])
REFERENCES [dbo].[Users] ([userID])
GO
ALTER TABLE [dbo].[Companies] CHECK CONSTRAINT [FK_Companies_Users]
GO
ALTER TABLE [dbo].[Maturity]  WITH CHECK ADD  CONSTRAINT [FK_Maturity_ClientPolicy] FOREIGN KEY([clientPolicyID])
REFERENCES [dbo].[ClientPolicy] ([clientPolicyID])
GO
ALTER TABLE [dbo].[Maturity] CHECK CONSTRAINT [FK_Maturity_ClientPolicy]
GO
ALTER TABLE [dbo].[Nominees]  WITH CHECK ADD  CONSTRAINT [FK_Nominees_Clients] FOREIGN KEY([clientID])
REFERENCES [dbo].[Clients] ([clientID])
GO
ALTER TABLE [dbo].[Nominees] CHECK CONSTRAINT [FK_Nominees_Clients]
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_ClientPolicy] FOREIGN KEY([clientPolicyID])
REFERENCES [dbo].[ClientPolicy] ([clientPolicyID])
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_ClientPolicy]
GO
ALTER TABLE [dbo].[Policies]  WITH CHECK ADD  CONSTRAINT [FK_Policies_Companies] FOREIGN KEY([companyID])
REFERENCES [dbo].[Companies] ([companyID])
GO
ALTER TABLE [dbo].[Policies] CHECK CONSTRAINT [FK_Policies_Companies]
GO
ALTER TABLE [dbo].[Policies]  WITH CHECK ADD  CONSTRAINT [FK_Policies_PolicyType] FOREIGN KEY([policytypeID])
REFERENCES [dbo].[PolicyType] ([policytypeID])
GO
ALTER TABLE [dbo].[Policies] CHECK CONSTRAINT [FK_Policies_PolicyType]
GO
ALTER TABLE [dbo].[PolicyTerms]  WITH CHECK ADD  CONSTRAINT [FK_PolicyTerms_Policies] FOREIGN KEY([policyID])
REFERENCES [dbo].[Policies] ([policyID])
GO
ALTER TABLE [dbo].[PolicyTerms] CHECK CONSTRAINT [FK_PolicyTerms_Policies]
GO
ALTER TABLE [dbo].[Premium]  WITH CHECK ADD  CONSTRAINT [FK_Premium_ClientPolicy] FOREIGN KEY([clientPolicyID])
REFERENCES [dbo].[ClientPolicy] ([clientPolicyID])
GO
ALTER TABLE [dbo].[Premium] CHECK CONSTRAINT [FK_Premium_ClientPolicy]
GO
USE [master]
GO
ALTER DATABASE [InsuranceDB] SET  READ_WRITE 
GO
