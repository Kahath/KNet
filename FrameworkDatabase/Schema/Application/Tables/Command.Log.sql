CREATE TABLE [Application].[Command.Log] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [Active]          BIT           CONSTRAINT [CS_Application.Command.Log_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [DateCreated]     DATETIME      CONSTRAINT [CS_Application.Command.Log_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateModified]    DATETIME      NULL,
    [DateDeactivated] DATETIME      NULL,
    [UserID]          INT           NOT NULL,
    [UserName]        VARCHAR (50)  NULL,
    [CommandName]     VARCHAR (255) NULL,
    [CommandID]       INT           NULL,
    CONSTRAINT [PK_Application.Command.Log] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Command.Log_Command] FOREIGN KEY ([CommandID]) REFERENCES [Application].[Command] ([ID])
);

