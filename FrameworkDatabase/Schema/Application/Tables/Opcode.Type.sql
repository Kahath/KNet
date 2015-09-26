CREATE TABLE [Application].[Opcode.Type] (
    [ID]              INT          IDENTITY (1, 1) NOT NULL,
    [Active]          BIT          CONSTRAINT [CS_Application.Opcode.Type_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [DateCreated]     DATETIME     CONSTRAINT [CS_Application.Opcode.Type_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateModified]    DATETIME     NULL,
    [DateDeactivated] DATETIME     NULL,
    [Name]            VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Application.Opcode.Type] PRIMARY KEY CLUSTERED ([ID] ASC)
);

