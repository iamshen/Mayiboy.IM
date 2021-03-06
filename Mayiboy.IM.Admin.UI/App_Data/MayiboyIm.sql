USE [master]
GO
/****** Object:  Database [IMSystem]    Script Date: 2019/7/23 11:04:14 ******/
CREATE DATABASE [IMSystem]

ALTER DATABASE [IMSystem] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [IMSystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [IMSystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [IMSystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [IMSystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [IMSystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [IMSystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [IMSystem] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [IMSystem] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [IMSystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [IMSystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [IMSystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [IMSystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [IMSystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [IMSystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [IMSystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [IMSystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [IMSystem] SET  DISABLE_BROKER 
GO
ALTER DATABASE [IMSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [IMSystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [IMSystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [IMSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [IMSystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [IMSystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [IMSystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [IMSystem] SET RECOVERY FULL 
GO
ALTER DATABASE [IMSystem] SET  MULTI_USER 
GO
ALTER DATABASE [IMSystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [IMSystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [IMSystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [IMSystem] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'IMSystem', N'ON'
GO
USE [IMSystem]
GO
/****** Object:  Table [dbo].[ChannelInfo]    Script Date: 2019/7/23 11:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChannelInfo](
	[ChannelId] [uniqueidentifier] NOT NULL,
	[ChannelName] [varchar](1024) NULL,
	[StorageStatus] [int] NOT NULL,
	[Remark] [varchar](1024) NULL,
	[UpdateTime] [datetime] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_ChannelInfo] PRIMARY KEY CLUSTERED 
(
	[ChannelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ChannelMessage]    Script Date: 2019/7/23 11:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChannelMessage](
	[CmessageId] [uniqueidentifier] NOT NULL,
	[ChannelId] [uniqueidentifier] NOT NULL,
	[ImUserId] [uniqueidentifier] NOT NULL,
	[MsgType] [int] NOT NULL,
	[MsgContent] [varchar](max) NULL,
	[DeviceType] [int] NOT NULL,
	[SourceType] [varchar](64) NOT NULL,
	[ClientIp] [varchar](64) NOT NULL,
	[ClientDesc] [varchar](max) NULL,
	[Timestamp] [bigint] NOT NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_ChannelMessage] PRIMARY KEY CLUSTERED 
(
	[CmessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FriendGroup]    Script Date: 2019/7/23 11:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FriendGroup](
	[FriendGroupId] [uniqueidentifier] NOT NULL,
	[FriendGroupName] [varchar](1024) NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_FriendGroup] PRIMARY KEY CLUSTERED 
(
	[FriendGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GroupInfo]    Script Date: 2019/7/23 11:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GroupInfo](
	[GroupId] [uniqueidentifier] NOT NULL,
	[GroupName] [varchar](128) NULL,
	[GroupCode] [varchar](10) NULL,
	[GroupMaxCapacity] [int] NULL,
	[GroupPicture] [varchar](1024) NULL,
	[GroupUserTypes] [varchar](32) NULL,
	[ChannelId] [uniqueidentifier] NULL,
	[Remark] [varchar](2048) NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_GroupInfo] PRIMARY KEY CLUSTERED 
(
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ImUserInfo]    Script Date: 2019/7/23 11:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ImUserInfo](
	[ImUserId] [uniqueidentifier] NOT NULL,
	[ImUserName] [varchar](32) NULL,
	[UserHeadimg] [varchar](2048) NULL,
	[Gender] [int] NULL,
	[UserId] [varchar](64) NULL,
	[UserType] [int] NOT NULL,
	[Remark] [varchar](2048) NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_ImUserInfo] PRIMARY KEY CLUSTERED 
(
	[ImUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ImUserRelation]    Script Date: 2019/7/23 11:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImUserRelation](
	[ImUserRelationId] [uniqueidentifier] NOT NULL,
	[ImUserId] [uniqueidentifier] NOT NULL,
	[FgImUserId] [uniqueidentifier] NOT NULL,
	[ApplyStatus] [int] NOT NULL,
	[FriendGroupId] [uniqueidentifier] NOT NULL,
	[ChannelId] [uniqueidentifier] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_ImUserRelation] PRIMARY KEY CLUSTERED 
(
	[ImUserRelationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserGroup]    Script Date: 2019/7/23 11:04:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserGroup](
	[UserGroupId] [uniqueidentifier] NOT NULL,
	[GroupId] [uniqueidentifier] NOT NULL,
	[NickName] [varchar](100) NULL,
	[ImUserId] [uniqueidentifier] NOT NULL,
	[RoleType] [int] NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_UserGroup] PRIMARY KEY CLUSTERED 
(
	[UserGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通道标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelInfo', @level2type=N'COLUMN',@level2name=N'ChannelId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通道名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelInfo', @level2type=N'COLUMN',@level2name=N'ChannelName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'存储（0：不存储；1：不存储）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelInfo', @level2type=N'COLUMN',@level2name=N'StorageStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelInfo', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelInfo', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelInfo', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效（0：无效；1：有效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelInfo', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通道信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通道消息标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelMessage', @level2type=N'COLUMN',@level2name=N'CmessageId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通道标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelMessage', @level2type=N'COLUMN',@level2name=N'ChannelId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息类型（1：文本；2：语音；3：图片；4：表情；）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelMessage', @level2type=N'COLUMN',@level2name=N'MsgType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'消息内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelMessage', @level2type=N'COLUMN',@level2name=N'MsgContent'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'设备类型（0：PC；1：安卓；2：IOS）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelMessage', @level2type=N'COLUMN',@level2name=N'DeviceType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'来源类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelMessage', @level2type=N'COLUMN',@level2name=N'SourceType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户端Ip' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelMessage', @level2type=N'COLUMN',@level2name=N'ClientIp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'时间戳' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelMessage', @level2type=N'COLUMN',@level2name=N'Timestamp'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效（0：无效；1：有效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelMessage', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通道消息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChannelMessage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'好友组标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FriendGroup', @level2type=N'COLUMN',@level2name=N'FriendGroupId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'好友组名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FriendGroup', @level2type=N'COLUMN',@level2name=N'FriendGroupName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FriendGroup', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FriendGroup', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效（0：无效；1：有效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FriendGroup', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'好友组信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FriendGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'组标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupInfo', @level2type=N'COLUMN',@level2name=N'GroupId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'组名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupInfo', @level2type=N'COLUMN',@level2name=N'GroupName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'组代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupInfo', @level2type=N'COLUMN',@level2name=N'GroupCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'组内最大容量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupInfo', @level2type=N'COLUMN',@level2name=N'GroupMaxCapacity'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'组图片' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupInfo', @level2type=N'COLUMN',@level2name=N'GroupPicture'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'组内用户类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupInfo', @level2type=N'COLUMN',@level2name=N'GroupUserTypes'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupInfo', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupInfo', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupInfo', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效（0：无效；1：有效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupInfo', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'组信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GroupInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'即时通讯用户标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserInfo', @level2type=N'COLUMN',@level2name=N'ImUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserInfo', @level2type=N'COLUMN',@level2name=N'ImUserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户头像' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserInfo', @level2type=N'COLUMN',@level2name=N'UserHeadimg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性别（0：女；1：男）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserInfo', @level2type=N'COLUMN',@level2name=N'Gender'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserInfo', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户类型(1:内部用户，2：会员用户；3：第三方用户；4：临时用户)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserInfo', @level2type=N'COLUMN',@level2name=N'UserType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserInfo', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserInfo', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserInfo', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效（0：无效；1：有效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserInfo', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户信息表' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户关系标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserRelation', @level2type=N'COLUMN',@level2name=N'ImUserRelationId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserRelation', @level2type=N'COLUMN',@level2name=N'ImUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'好友用户标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserRelation', @level2type=N'COLUMN',@level2name=N'FgImUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'(0:待审核；1：申请通过；2：申请驳回)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserRelation', @level2type=N'COLUMN',@level2name=N'ApplyStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'好友组标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserRelation', @level2type=N'COLUMN',@level2name=N'FriendGroupId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'通道标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserRelation', @level2type=N'COLUMN',@level2name=N'ChannelId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserRelation', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效（0：无效；1：有效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserRelation', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户关系' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ImUserRelation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户组标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserGroup', @level2type=N'COLUMN',@level2name=N'UserGroupId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'组标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserGroup', @level2type=N'COLUMN',@level2name=N'GroupId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户群组内昵称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserGroup', @level2type=N'COLUMN',@level2name=N'NickName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserGroup', @level2type=N'COLUMN',@level2name=N'ImUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色类型（0：普通用户；1：组长；2：管理员；）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserGroup', @level2type=N'COLUMN',@level2name=N'RoleType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserGroup', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserGroup', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效（0：无效；1：有效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserGroup', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户组' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserGroup'
GO
USE [master]
GO
ALTER DATABASE [IMSystem] SET  READ_WRITE 
GO
