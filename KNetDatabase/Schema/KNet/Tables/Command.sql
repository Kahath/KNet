CREATE TABLE [KNet].[Command] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [Active]          BIT           CONSTRAINT [CS_KNet.Command_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [DateCreated]     DATETIME      CONSTRAINT [CS_KNet.Command_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateModified]    DATETIME      NULL,
    [DateDeactivated] DATETIME      NULL,
    [Name]            VARCHAR (50)  NOT NULL,
    [CommandLevelID]  INT           NULL,
    [ParentID]        INT           NULL,
    [Description]     TEXT          NULL,
    [AssemblyName]    VARCHAR (255) NULL,
    [TypeName]        VARCHAR (500) NULL,
    [MethodName]      VARCHAR (255) NULL,
    CONSTRAINT [PK_KNet.Command] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_KNet.Command_Command] FOREIGN KEY ([ParentID]) REFERENCES [KNet].[Command] ([ID]),
    CONSTRAINT [FK_KNet.Command_Command.Level] FOREIGN KEY ([CommandLevelID]) REFERENCES [KNet].[Command.Level] ([ID])
);

