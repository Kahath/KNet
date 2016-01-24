CREATE TABLE [Application].[Log] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [Active]          BIT           CONSTRAINT [CS_Application.Log_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [DateCreated]     DATETIME      CONSTRAINT [CS_Application.Log_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateModified]    DATETIME      NULL,
    [DateDeactivated] DATETIME      NULL,
    [LogTypeID]       INT           NOT NULL,
    [Message]         VARCHAR (MAX) NULL,
    [Description]     VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Application.Log] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Packet.Log_Log.Type] FOREIGN KEY ([LogTypeID]) REFERENCES [Application].[Log.Type] ([ID])
);



