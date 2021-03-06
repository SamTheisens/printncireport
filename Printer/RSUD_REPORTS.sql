IF NOT EXISTS (SELECT * 
           FROM   INFORMATION_SCHEMA.TABLES 
           WHERE  TABLE_SCHEMA = N'dbo' 
                  AND TABLE_NAME = N'RSUD_REPORTS' 
                  AND TABLE_TYPE = 'BASE TABLE') 
  BEGIN 
	CREATE TABLE [dbo].[RSUD_REPORTS](
		[KD_KASIR] [varchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[PENDAFTARAN] [bit] NOT NULL,
		[KD_CUSTOMER] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[KD_CUSTOMER_REPORT] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[NAMA_SP] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[PRINTER] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	 CONSTRAINT [PK_RSUD_REPORTS] PRIMARY KEY CLUSTERED 
	(
		[KD_KASIR] ASC,
		[PENDAFTARAN] ASC,
		[KD_CUSTOMER] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
  END
