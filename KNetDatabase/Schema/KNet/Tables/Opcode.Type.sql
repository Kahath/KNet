CREATE TABLE [KNet].[Opcode.Type] (
    [ID]              INT          IDENTITY (1, 1) NOT NULL,
    [Active]          BIT          CONSTRAINT [CS_KNet.Opcode.Type_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [DateCreated]     DATETIME     CONSTRAINT [CS_KNet.Opcode.Type_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateModified]    DATETIME     NULL,
    [DateDeactivated] DATETIME     NULL,
    [Name]            VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_KNet.Opcode.Type] PRIMARY KEY CLUSTERED ([ID] ASC)
);

