USE [Zavrsni]
GO
/****** Object:  Table [Application].[Log.Type]    Script Date: 5/5/2015 22:00:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Application].[Log.Type](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [CS_Application.Log.Type_Active]  DEFAULT (CONVERT([bit],(1))),
	[DateCreated] [datetime] NOT NULL CONSTRAINT [CS_Application.Log.Type_DateCreated]  DEFAULT (getdate()),
	[DateModified] [datetime] NULL,
	[DateDeactivated] [datetime] NULL,
	[Name] [varchar](10) NULL,
 CONSTRAINT [PK_Application.Log.Type] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [Application].[Log.Type] ON 

GO
INSERT [Application].[Log.Type] ([ID], [Active], [DateCreated], [DateModified], [DateDeactivated], [Name]) VALUES (1, 1, CAST(N'2015-04-21 19:58:11.817' AS DateTime), NULL, NULL, N'Normal')
GO
INSERT [Application].[Log.Type] ([ID], [Active], [DateCreated], [DateModified], [DateDeactivated], [Name]) VALUES (2, 1, CAST(N'2015-04-21 19:58:11.817' AS DateTime), NULL, NULL, N'Init')
GO
INSERT [Application].[Log.Type] ([ID], [Active], [DateCreated], [DateModified], [DateDeactivated], [Name]) VALUES (4, 1, CAST(N'2015-04-21 19:58:11.817' AS DateTime), NULL, NULL, N'Command')
GO
INSERT [Application].[Log.Type] ([ID], [Active], [DateCreated], [DateModified], [DateDeactivated], [Name]) VALUES (8, 1, CAST(N'2015-04-21 19:58:11.817' AS DateTime), NULL, NULL, N'DB')
GO
INSERT [Application].[Log.Type] ([ID], [Active], [DateCreated], [DateModified], [DateDeactivated], [Name]) VALUES (16, 1, CAST(N'2015-04-21 19:58:11.817' AS DateTime), NULL, NULL, N'Info')
GO
INSERT [Application].[Log.Type] ([ID], [Active], [DateCreated], [DateModified], [DateDeactivated], [Name]) VALUES (32, 1, CAST(N'2015-04-21 19:58:11.817' AS DateTime), NULL, NULL, N'Warning')
GO
INSERT [Application].[Log.Type] ([ID], [Active], [DateCreated], [DateModified], [DateDeactivated], [Name]) VALUES (64, 1, CAST(N'2015-04-21 19:58:11.817' AS DateTime), NULL, NULL, N'Error')
GO
INSERT [Application].[Log.Type] ([ID], [Active], [DateCreated], [DateModified], [DateDeactivated], [Name]) VALUES (128, 1, CAST(N'2015-04-21 19:58:11.817' AS DateTime), NULL, NULL, N'Critical')
GO
SET IDENTITY_INSERT [Application].[Log.Type] OFF
GO
