USE [p2p]
GO
/****** Object:  Table [dbo].[Offer]    Script Date: 02/11/2016 13:18:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Offer](
	[ID] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[Name] [varchar](500) NOT NULL,
	[OfferDescription] [varchar](500) NOT NULL,
	[ImagePath] [varchar](500) NULL,
	[StoreId] [int] NULL,
	[OfferCode] [varchar](500) NULL,
 CONSTRAINT [PK_Offer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NotificationStatus]    Script Date: 02/11/2016 13:18:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationStatus](
	[DeviceID] [nvarchar](500) NOT NULL,
	[Date] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Device]    Script Date: 02/11/2016 13:18:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Device](
	[id] [varchar](500) NOT NULL,
	[EmailId] [varchar](500) NOT NULL,
	[Name] [varchar](500) NOT NULL,
	[GcmToken] [varchar](500) NOT NULL,
	[Clubcard] [varchar](500) NULL,
	[Mobile] [varchar](500) NULL,
 CONSTRAINT [PK_Device] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Coupon]    Script Date: 02/11/2016 13:18:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Coupon](
	[Clubcard] [varchar](500) NOT NULL,
	[CouponCode] [varchar](500) NOT NULL,
	[Name] [varchar](500) NULL,
	[CouponDescription] [varchar](500) NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[CouponAvailed] [bit] NOT NULL,
	[ImagePath] [varchar](500) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[RegisterUser]    Script Date: 02/11/2016 13:18:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RegisterUser] 
	-- Add the parameters for the stored procedure here
	@id varchar(500),
	@EmailId varchar(500),
	@Name varchar(500),
	@GcmToken varchar(500),
	@Clubcard varchar(500),
	@Mobile varchar(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   insert into Device(id,EmailId,Name,GcmToken,Clubcard,Mobile) 
   values (@id,@EmailId,@Name,@GcmToken,@Clubcard,@Mobile);
END
GO
