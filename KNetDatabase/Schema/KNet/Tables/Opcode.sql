CREATE TABLE [KNet].[Opcode] (
    [ID]              INT           IDENTITY (1, 1) NOT NULL,
    [Active]          BIT           CONSTRAINT [CS_KNet.Opcode_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [DateCreated]     DATETIME      CONSTRAINT [CS_KNet.Opcode_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateModified]    DATETIME      NULL,
    [DateDeactivated] DATETIME      NULL,
    [Code]            INT           NOT NULL,
    [TypeID]          INT           NULL,
    [Version]         INT           NOT NULL,
    [Author]          VARCHAR (50)  NULL,
    [AssemblyName]    VARCHAR (255) NOT NULL,
    [TypeName]        VARCHAR (500) NOT NULL,
    [MethodName]      VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_KNet.Opcode] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Opcode_Opcode.Type] FOREIGN KEY ([TypeID]) REFERENCES [KNet].[Opcode.Type] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_UF_Opcode.Code_Opcode.Version_Opcode.TypeID_Opcode.Active]
    ON [KNet].[Opcode]([Code] ASC, [Version] ASC, [TypeID] ASC) WHERE ([Active]=(1));

