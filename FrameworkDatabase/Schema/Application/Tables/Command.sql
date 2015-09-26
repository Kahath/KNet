CREATE TABLE [Application].[Command] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [Active]          BIT           CONSTRAINT [CS_Application.Command_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [DateCreated]     DATETIME      CONSTRAINT [CS_Application.Command_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateModified]    DATETIME      NULL,
    [DateDeactivated] DATETIME      NULL,
    [Name]            VARCHAR (50)  NOT NULL,
    [CommandLevelID]  INT           NULL,
    [ParentID]        INT           NULL,
    [Description]     TEXT          NULL,
    [AssemblyName]    VARCHAR (255) NULL,
    [TypeName]        VARCHAR (500) NULL,
    [MethodName]      VARCHAR (255) NULL,
    CONSTRAINT [PK_Application.Command] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Application.Command_Command] FOREIGN KEY ([ParentID]) REFERENCES [Application].[Command] ([ID]),
    CONSTRAINT [FK_Application.Command_Command.Level] FOREIGN KEY ([CommandLevelID]) REFERENCES [Application].[Command.Level] ([ID])
);

