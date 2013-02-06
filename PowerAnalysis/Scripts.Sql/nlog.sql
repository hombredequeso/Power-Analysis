CREATE TABLE [dbo].[NLog_Output](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[time_stamp] [datetime] NULL,
	[host] [nvarchar](max) NULL,
	[type] [nvarchar](50) NULL,
	[source] [nvarchar](50) NULL,
	[message] [nvarchar](max) NULL,
	[level] [nvarchar](50) NULL,
	[logger] [nvarchar](50) NULL,
	[stacktrace] [nvarchar](max) NULL,
	[allxml] [ntext] NULL,
 CONSTRAINT [PK_NLogError] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[NLog_Output] ADD  CONSTRAINT [DF_NLogError_time_stamp]  DEFAULT (getdate()) FOR [time_stamp]

GO
