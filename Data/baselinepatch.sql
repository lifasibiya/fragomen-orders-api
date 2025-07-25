USE fragomen_orders_db
GO



IF (OBJECT_ID('[dbo].[state_change]') IS NOT NULL)
	DROP TABLE [dbo].[state_change]
GO


IF (OBJECT_ID('[dbo].[order]') IS NOT NULL)
	DROP TABLE [dbo].[order]
GO


IF (OBJECT_ID('[dbo].[temp_order]') IS NOT NULL)
	DROP TABLE [dbo].[temp_order]
GO



IF (OBJECT_ID('[order_state]') IS NOT NULL)
	DROP TABLE [dbo].[order_state]
GO


IF (OBJECT_ID('[dbo].[product]') IS NOT NULL)
	DROP TABLE [dbo].[product]
GO


CREATE TABLE [dbo].[product]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[title] VARCHAR(100) NOT NULL,
	[barcode] VARCHAR(100)
)


CREATE TABLE [dbo].[order_state]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[state] VARCHAR(20) NOT NULL
)



CREATE TABLE [dbo].[temp_order]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[order_json] VARCHAR(MAX),
	[session_id] INT,
	[timestamp] DATETIME NOT NULL
)




CREATE TABLE [dbo].[order]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[description] VARCHAR(MAX),
	[productId] INT NOT NULL REFERENCES [dbo].[product](id),
	[stateId] INT NOT NULL REFERENCES [dbo].[order_state](id),
	[isDeleted] BIT
)



CREATE TABLE [dbo].[state_change]
(
	[id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[order_id] INT NOT NULL REFERENCES [dbo].[order](id),
	[prev_state] VARCHAR(20),
	[new_state] VARCHAR(20) NOT NULL,
	[timestamp] DATETIME NOT NULL
)





INSERT INTO [dbo].[order_state] ([state])
VALUES ('Draft'),('Submitted'),('Completed')


INSERT INTO [dbo].[product] ([title], [barcode]) VALUES ('Wireless Mouse', '123456789012');
INSERT INTO [dbo].[product] ([title], [barcode]) VALUES ('USB-C Charger', '234567890123');
INSERT INTO [dbo].[product] ([title], [barcode]) VALUES ('Bluetooth Speaker', '345678901234');
INSERT INTO [dbo].[product] ([title], [barcode]) VALUES ('LED Monitor 24"', '456789012345');
INSERT INTO [dbo].[product] ([title], [barcode]) VALUES ('Gaming Keyboard', '567890123456');
INSERT INTO [dbo].[product] ([title], [barcode]) VALUES ('External Hard Drive 1TB', '678901234567');
INSERT INTO [dbo].[product] ([title], [barcode]) VALUES ('Smartphone Stand', '789012345678');
INSERT INTO [dbo].[product] ([title], [barcode]) VALUES ('Noise Cancelling Headphones', '890123456789');
INSERT INTO [dbo].[product] ([title], [barcode]) VALUES ('Webcam 1080p', '901234567890');
INSERT INTO [dbo].[product] ([title], [barcode]) VALUES ('HDMI Cable 2m', '012345678901');



