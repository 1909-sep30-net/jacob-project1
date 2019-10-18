/* Create all of the tables anew. */

CREATE TABLE [Component] (
  [componentId] int PRIMARY KEY IDENTITY(1, 1),
  [name] nvarchar(255),
  [cost] money
)
GO

CREATE TABLE [Product] (
  [productId] int PRIMARY KEY IDENTITY(1, 1),
  [name] nvarchar(255)
)
GO

CREATE TABLE [ProductComponent] (
  [productId] int FOREIGN KEY REFERENCES Product(productId),
  [componentId] int FOREIGN KEY REFERENCES Component(componentId),
  [quantity] int,
  PRIMARY KEY (productId, componentId)
)
GO

CREATE TABLE [Customer] (
  [customerId] int PRIMARY KEY IDENTITY(1, 1),
  [firstName] nvarchar(255),
  [middleName] nvarchar(255),
  [lastName] nvarchar(255),
  [phoneNumber] nvarchar(255)
)
GO

CREATE TABLE [Location] (
  [locationId] int PRIMARY KEY IDENTITY(1, 1),
  [address] nvarchar(255)
)
GO

CREATE TABLE [Inventory] (
  [locationId] int FOREIGN KEY REFERENCES [Location](locationId),
  [componentId] int FOREIGN KEY REFERENCES Component(componentId),
  [quantity] int,
  PRIMARY KEY (locationId, componentId)
)
GO

CREATE TABLE [Order] (
  [orderId] int PRIMARY KEY IDENTITY(1, 1),
  [orderDate] datetime DEFAULT 'GETDATE()',
  [locationId] int FOREIGN KEY REFERENCES [Location](locationId),
  [customerId] int FOREIGN KEY REFERENCES Customer(customerId)
)
GO

CREATE TABLE [OrderDetails] (
  [orderId] int FOREIGN KEY REFERENCES [Order](orderId),
  [productId] int FOREIGN KEY REFERENCES Product(productId),
  [quantity] int,
  PRIMARY KEY (orderId, productId) 
)
GO