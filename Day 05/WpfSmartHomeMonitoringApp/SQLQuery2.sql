USE [OpenApiLab]
GO

/****** Object:  Table [dbo].[TblSmartHome]    Script Date: 2022-06-10 ¿ÀÀü 10:04:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TblSmartHome](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Devid] [varchar](10) NOT NULL,
	[CurrTime] [datetime] NOT NULL,
	[Temp] [float] NOT NULL,
	[Humid] [float] NOT NULL,
	[Press] [float] NULL,
 CONSTRAINT [PK_TblSmartHome] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


