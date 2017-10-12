CREATE TABLE [KNet].[Server] (
    [ID]              INT      IDENTITY (1, 1) NOT NULL,
    [Active]          BIT      CONSTRAINT [CS_KNet.Server_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [DateCreated]     DATETIME CONSTRAINT [CS_KNet.Server_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateModified]    DATETIME NULL,
    [DateDeactivated] DATETIME NULL,
    [IsSuccessful]    BIT      NOT NULL,
    CONSTRAINT [PK_KNet.Server] PRIMARY KEY CLUSTERED ([ID] ASC)
);

