USE [NASCAR]
GO
/****** Object:  Table [dbo].[Leaderboard]    Script Date: 4/29/2024 8:38:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Leaderboard](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Score] [int] NOT NULL,
 CONSTRAINT [PK_Leaderboard] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 4/29/2024 8:38:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[TypeID] [int] NOT NULL,
	[Avatar] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserType]    Script Date: 4/29/2024 8:38:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](6) NOT NULL,
 CONSTRAINT [PK_UserType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[UserType] ON 

INSERT [dbo].[UserType] ([ID], [Type]) VALUES (1, N'player')
INSERT [dbo].[UserType] ([ID], [Type]) VALUES (2, N'driver')
SET IDENTITY_INSERT [dbo].[UserType] OFF
GO
ALTER TABLE [dbo].[Leaderboard]  WITH CHECK ADD  CONSTRAINT [FK_Leaderboard_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Leaderboard] CHECK CONSTRAINT [FK_Leaderboard_User]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_UserType] FOREIGN KEY([TypeID])
REFERENCES [dbo].[UserType] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_UserType]
GO
/****** Object:  StoredProcedure [dbo].[Leaderboard_GetByUserID]    Script Date: 4/29/2024 8:38:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Leaderboard_GetByUserID] 
	-- Add the parameters for the stored procedure here
	@UserID int
AS
BEGIN
	
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Leaderboard
	WHERE UserID = @UserID
END
GO
/****** Object:  StoredProcedure [dbo].[Leaderboard_GetPaged]    Script Date: 4/29/2024 8:38:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Leaderboard_GetPaged]
	@rowOffset int = 0,
	@fetchNextRows int = 5
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
	L.ID,
	L.Score,
	U.Username,
	U.Avatar,
	UT.Type

	FROM Leaderboard as L
	INNER JOIN [dbo].[User] as U ON L.UserID = U.ID
	INNER JOIN [dbo].[UserType] as UT ON U.TypeID = UT.ID
	ORDER BY Score asc
	OFFSET @rowOffset ROWS FETCH NEXT @fetchNextRows ROWS ONLY;
END
GO
/****** Object:  StoredProcedure [dbo].[Leaderboard_Insert]    Script Date: 4/29/2024 8:38:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Leaderboard_Insert] 
	@UserID int,
	@Score int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF EXISTS(SELECT 1 FROM [dbo].[Leaderboard] as l WHERE l.UserID = @UserID) AND NOT ((SELECT Score FROM [dbo].[Leaderboard] WHERE UserID = @UserID) < @Score)
	BEGIN
		UPDATE [dbo].[Leaderboard]
		SET Score = @Score
		WHERE UserID = @UserID;
	END
	ELSE IF NOT EXISTS(SELECT 1 FROM [dbo].[Leaderboard] as l WHERE l.UserID = @UserID)
	BEGIN
		INSERT INTO [dbo].[Leaderboard](UserID, Score)
		VALUES(@UserID, @Score);
	END
END
GO
/****** Object:  StoredProcedure [dbo].[Leaderboard_UpdateScoreByUser]    Script Date: 4/29/2024 8:38:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Leaderboard_UpdateScoreByUser] 
	-- Add the parameters for the stored procedure here
	@UserID int,
	@Score int
AS
BEGIN
	
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Leaderboard
	Set Score = @Score
	WHERE UserID = @UserID;
END
GO
/****** Object:  StoredProcedure [dbo].[User_GetById]    Script Date: 4/29/2024 8:38:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[User_GetById]
	@UserID int
AS
BEGIN

	SET NOCOUNT ON;

   SELECT
	U.[ID],
	U.[Username],
	UT.[Type],
	U.[Avatar]
	FROM [dbo].[User] as U
	INNER JOIN [dbo].[UserType] UT ON U.TypeID = UT.ID
	WHERE U.[ID] = @UserID;

END
GO
/****** Object:  StoredProcedure [dbo].[User_GetByUsername]    Script Date: 4/29/2024 8:38:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[User_GetByUsername]
	@Username nvarchar(50)
AS
BEGIN

	SET NOCOUNT ON;

   SELECT
	U.[ID],
	U.[Username],
	UT.[Type],
	U.[Avatar]
	FROM [dbo].[User] as U
	INNER JOIN [dbo].[UserType] UT ON U.TypeID = UT.ID
	WHERE U.[Username] = @Username;
END
GO
/****** Object:  StoredProcedure [dbo].[User_Insert]    Script Date: 4/29/2024 8:38:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nick Eisner
-- Create date: 4/20/24
-- Description:	Insert record into Users table
-- =============================================
CREATE PROCEDURE [dbo].[User_Insert]
	-- Add the parameters for the stored procedure here
	@Username nvarchar(50),
	@TypeID int,
	@Avatar int,
	@ID int OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	INSERT INTO [dbo].[User] (
		[Username],
		[TypeID],
		[Avatar])
	VALUES(
		@Username,
		@TypeID,
		@Avatar
		)
	SET @ID = SCOPE_IDENTITY();
END
GO
