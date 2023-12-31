USE [PROG260FA23]
GO
/****** Object:  Table [dbo].[Grocer]    Script Date: 10/18/2023 11:20:17 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Grocer]') AND type in (N'U'))
DROP TABLE [dbo].[Grocer]
GO
/****** Object:  Table [dbo].[Grocer]    Script Date: 10/18/2023 11:20:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Grocer](
	[Name] [nchar](20) NOT NULL,
	[Location] [nchar](10) NOT NULL,
	[Price] [float] NOT NULL,
	[UoM] [nchar](10) NOT NULL,
	[SellByDate] [datetime] NOT NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Grocer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
