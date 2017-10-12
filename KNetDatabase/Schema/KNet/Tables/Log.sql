CREATE TABLE [KNet].[Log] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [Active]          BIT           CONSTRAINT [CS_KNet.Log_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [DateCreated]     DATETIME      CONSTRAINT [CS_KNet.Log_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateModified]    DATETIME      NULL,
    [DateDeactivated] DATETIME      NULL,
    [LogTypeID]       INT           NOT NULL,
    [Message]         VARCHAR (MAX) NULL,
    [Description]     VARCHAR (MAX) NULL,
    CONSTRAINT [PK_KNet.Log] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Packet.Log_Log.Type] FOREIGN KEY ([LogTypeID]) REFERENCES [KNet].[Log.Type] ([ID])
);



