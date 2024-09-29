/****** Object:  Database [KironTest]    Script Date: 2024/09/28 17:52:45 ******/
CREATE DATABASE [KironTest]  (EDITION = 'GeneralPurpose', SERVICE_OBJECTIVE = 'GP_S_Gen5_1', MAXSIZE = 32 GB) WITH CATALOG_COLLATION = SQL_Latin1_General_CP1_CI_AS, LEDGER = OFF;
GO
ALTER DATABASE [KironTest] SET COMPATIBILITY_LEVEL = 160
GO
ALTER DATABASE [KironTest] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [KironTest] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [KironTest] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [KironTest] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [KironTest] SET ARITHABORT OFF 
GO
ALTER DATABASE [KironTest] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [KironTest] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [KironTest] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [KironTest] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [KironTest] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [KironTest] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [KironTest] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [KironTest] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [KironTest] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [KironTest] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [KironTest] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [KironTest] SET  MULTI_USER 
GO
ALTER DATABASE [KironTest] SET ENCRYPTION ON
GO
ALTER DATABASE [KironTest] SET QUERY_STORE = ON
GO
ALTER DATABASE [KironTest] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 100, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
/*** The scripts of database scoped configurations in Azure should be executed inside the target database connection. ***/
GO
-- ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 8;
GO
/****** Object:  UserDefinedTableType [dbo].[HolidayType]    Script Date: 2024/09/28 17:52:45 ******/
CREATE TYPE [dbo].[HolidayType] AS TABLE(
	[Id] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Date] [date] NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[RegionHolidayType]    Script Date: 2024/09/28 17:52:45 ******/
CREATE TYPE [dbo].[RegionHolidayType] AS TABLE(
	[RegionId] [int] NOT NULL,
	[HolidayId] [int] NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[RegionType]    Script Date: 2024/09/28 17:52:45 ******/
CREATE TYPE [dbo].[RegionType] AS TABLE(
	[Id] [int] NOT NULL,
	[Region] [varchar](50) NOT NULL
)
GO
/****** Object:  Table [dbo].[Holidays]    Script Date: 2024/09/28 17:52:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Holidays](
	[Id] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Date] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Navigation]    Script Date: 2024/09/28 17:52:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Navigation](
	[Id] [int] NOT NULL,
	[ParentId] [int] NOT NULL,
	[Text] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RegionHolidays]    Script Date: 2024/09/28 17:52:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegionHolidays](
	[RegionId] [int] NOT NULL,
	[HolidayId] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Regions]    Script Date: 2024/09/28 17:52:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Regions](
	[Id] [int] NOT NULL,
	[Region] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2024/09/28 17:52:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Username] [varchar](100) NOT NULL,
	[Password] [varchar](100) NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RegionHolidays]  WITH CHECK ADD FOREIGN KEY([HolidayId])
REFERENCES [dbo].[Holidays] ([Id])
GO
ALTER TABLE [dbo].[RegionHolidays]  WITH CHECK ADD FOREIGN KEY([RegionId])
REFERENCES [dbo].[Regions] ([Id])
GO
/****** Object:  StoredProcedure [dbo].[SP_Get_Login]    Script Date: 2024/09/28 17:52:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[SP_Get_Login]
(
    -- Add the parameters for the stored procedure here
    @Username varchar(100),
	@Password varchar(100)
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    SELECT top(1) Username from [dbo].[Users] where Username=@Username and Password=@Password
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Get_Menu]    Script Date: 2024/09/28 17:52:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
create PROCEDURE [dbo].[SP_Get_Menu]

AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    SELECT Id, ParentId, [Text] FROM Navigation order by parentId
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Get_RegionHolidays]    Script Date: 2024/09/28 17:52:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Get_RegionHolidays]
AS
BEGIN
      SET NOCOUNT ON;
		
	  SELECT  h.[Name], h.[Date], rh.[RegionId]
  FROM [dbo].[RegionHolidays] rh
  inner join dbo.Holidays h on h.Id = rh.HolidayId
  ORDER BY rh.RegionId
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Get_RegionHolidaysByRegionId]    Script Date: 2024/09/28 17:52:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Get_RegionHolidaysByRegionId]
      @RegionId int
AS
BEGIN
      SET NOCOUNT ON;
		
	  SELECT  h.[Name], h.[Date]
  FROM [dbo].[RegionHolidays] rh
  inner join dbo.Holidays h on h.Id = rh.HolidayId
  where rh.RegionId = @RegionId
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Insert_Login]    Script Date: 2024/09/28 17:52:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[SP_Insert_Login]
(
    -- Add the parameters for the stored procedure here
    @Username varchar(100),
	@Password varchar(100)
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    INSERT INTO [dbo].[Users] (Username, [Password]) values (@Username, @Password)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Insert_RegionHolidays]    Script Date: 2024/09/28 17:52:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Insert_RegionHolidays]
      @RegionType RegionType READONLY,
      @HolidayType HolidayType READONLY,
      @RegionHolidayType RegionHolidayType READONLY
AS
BEGIN
      SET NOCOUNT ON;
	  
	  delete from RegionHolidays;
	  delete from Holidays;
	  delete from  Regions;

      MERGE INTO Regions R1
      USING @RegionType R2
      ON R1.Id=R2.Id
      WHEN NOT MATCHED THEN
      INSERT VALUES(R2.Id, R2.Region);

	  MERGE INTO Holidays E1
      USING @HolidayType E2
      ON E1.Id=E2.Id
      WHEN NOT MATCHED THEN
      INSERT VALUES(E2.Id, E2.[Name], E2.[Date]);	  

	  MERGE INTO RegionHolidays RH1
      USING @RegionHolidayType RH2
      ON RH1.RegionId=RH2.RegionId and RH1.HolidayId=RH2.HolidayId
      WHEN NOT MATCHED THEN
      INSERT VALUES(RH2.RegionId, RH2.HolidayId);
END
GO
ALTER DATABASE [KironTest] SET  READ_WRITE 
GO

INSERT [dbo].[Navigation] ([Id], [ParentId], [Text]) VALUES (1, -1, N'Item1')
GO
INSERT [dbo].[Navigation] ([Id], [ParentId], [Text]) VALUES (2, -1, N'Item2')
GO
INSERT [dbo].[Navigation] ([Id], [ParentId], [Text]) VALUES (3, 1, N'Item1.1')
GO
INSERT [dbo].[Navigation] ([Id], [ParentId], [Text]) VALUES (4, 3, N'Item1.1.1')
GO
INSERT [dbo].[Navigation] ([Id], [ParentId], [Text]) VALUES (5, 1, N'Item1.2')
GO
INSERT [dbo].[Navigation] ([Id], [ParentId], [Text]) VALUES (6, 2, N'Item2.1')
GO
INSERT [dbo].[Navigation] ([Id], [ParentId], [Text]) VALUES (7, 2, N'Item2.2')
GO
INSERT [dbo].[Navigation] ([Id], [ParentId], [Text]) VALUES (8, 7, N'Item2.2.1')
GO
INSERT [dbo].[Navigation] ([Id], [ParentId], [Text]) VALUES (9, -1, N'Item3')
GO
