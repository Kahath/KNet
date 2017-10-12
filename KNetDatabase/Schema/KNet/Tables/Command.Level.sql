CREATE TABLE [KNet].[Command.Level] (
    [ID]              INT          IDENTITY (1, 1) NOT NULL,
    [Active]          BIT          CONSTRAINT [CS_KNet.Command.Level_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [DateCreated]     DATETIME     CONSTRAINT [CS_KNet.Command.Level_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateModified]    DATETIME     NULL,
    [DateDeactivated] DATETIME     NULL,
    [Name]            VARCHAR (10) NULL,
    CONSTRAINT [PK_KNet.Command.Level] PRIMARY KEY CLUSTERED ([ID] ASC)
);

