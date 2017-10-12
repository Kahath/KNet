CREATE TABLE [KNet].[Packet.Log] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [Active]          BIT           CONSTRAINT [CS_KNet.Packet.Log_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [DateCreated]     DATETIME      CONSTRAINT [CS_KNet.Packet.Log_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateModified]    DATETIME      NULL,
    [DateDeactivated] DATETIME      NULL,
    [IP]              VARCHAR (16)  NULL,
    [ClientID]        INT           NULL,
    [Size]            INT           NULL,
    [PacketLogTypeID] INT           NOT NULL,
    [Opcode]          INT           NULL,
    [Message]         VARCHAR (MAX) NULL,
    CONSTRAINT [PK_KNet.Packet.Log] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Packet.Log_Packet.Log.Type] FOREIGN KEY ([PacketLogTypeID]) REFERENCES [KNet].[Packet.Log.Type] ([ID])
);

