USE [Zavrsni]
GO
/****** Object:  Table [Application].[Command.Log]    Script Date: 4/6/2015 19:24:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Application].[Command.Log](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [CS_Application.Command.Log_Active]  DEFAULT (CONVERT([bit],(1))),
	[DateCreated] [datetime] NOT NULL CONSTRAINT [CS_Application.Command.Log_DateCreated]  DEFAULT (getdate()),
	[DateModified] [datetime] NULL,
	[DateDeactivated] [datetime] NULL,
	[UserID] [int] NOT NULL,
	[UserName] [varchar](50) NULL,
	[Command] [varchar](255) NULL,
 CONSTRAINT [PK_Application.Command.Log] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
