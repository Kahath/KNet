CREATE TABLE [Application].[Log.Type] (
    [ID]              INT          IDENTITY (1, 1) NOT NULL,
    [Active]          BIT          CONSTRAINT [CS_Application.Log.Type_Active] DEFAULT (CONVERT([bit],(1))) NOT NULL,
    [DateCreated]     DATETIME     CONSTRAINT [CS_Application.Log.Type_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateModified]    DATETIME     NULL,
    [DateDeactivated] DATETIME     NULL,
    [Name]            VARCHAR (10) NULL,
    CONSTRAINT [PK_Application.Log.Type] PRIMARY KEY CLUSTERED ([ID] ASC)
);

