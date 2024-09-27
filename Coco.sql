-- Create the database
CREATE DATABASE StoreManager;
GO

-- Use the newly created database
USE StoreManager;
GO

-- Create UserRoles table
CREATE TABLE UserRoles (
    id INT NOT NULL IDENTITY(0,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL
);
GO

-- Insert predefined roles into UserRoles
INSERT INTO dbo.UserRoles (name)
VALUES
('Admin'), ('Seller'), ('Customer');
GO

-- Create Users table (General user information)
CREATE TABLE Users (
    id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    role INT NOT NULL,
    status BIT NOT NULL,
    remember_token NVARCHAR(100) DEFAULT NULL,
    reset_password_token NVARCHAR(100) DEFAULT NULL,
    created_at DATETIME NULL DEFAULT NULL,
    updated_at DATETIME NULL DEFAULT NULL,
    FOREIGN KEY (role) REFERENCES UserRoles(id)
);
GO

-- Create UserDetails table (Detailed information separated from Users table)
CREATE TABLE BuyerDetails (
    user_id INT NOT NULL PRIMARY KEY,
    fullname NVARCHAR(255) NOT NULL,
    address NVARCHAR(255) NOT NULL,
    phone NVARCHAR(255) NOT NULL,
    dob DATE NOT NULL,
    gender BIT NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(id)
);
GO

-- Create Products table (General product information)
CREATE TABLE Products (
    id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(255) NOT NULL,
    MeasureUnit NVARCHAR(255) NOT NULL,
    cost DECIMAL(10, 2) NOT NULL,
    status BIT NOT NULL,
    created_at DATETIME NULL DEFAULT NULL,
    updated_at DATETIME NULL DEFAULT NULL,
    seller_id INT NOT NULL,
    FOREIGN KEY (seller_id) REFERENCES Users(id)
);
GO

-- Create ProductDetails table (Additional product information)
CREATE TABLE ProductDetails (
    product_id INT NOT NULL PRIMARY KEY,
    description NVARCHAR(MAX) NULL,
    additional_info NVARCHAR(MAX) NULL,
    FOREIGN KEY (product_id) REFERENCES Products(id)
);
GO

-- Create Customers table (General customer information)
CREATE TABLE Customers (
    id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(MAX) NOT NULL,
    address NVARCHAR(MAX) NOT NULL,
    phone NVARCHAR(MAX) NOT NULL,
    note NVARCHAR(MAX) NULL,
    status BIT NOT NULL,
    created_at DATETIME NULL DEFAULT NULL,
    updated_at DATETIME NULL DEFAULT NULL,
    seller_id INT NOT NULL,
    FOREIGN KEY (seller_id) REFERENCES Users(id)
);
GO

-- Create Orders table (General order information)
CREATE TABLE ExportOrders (
    id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    customer_id INT NOT NULL,
    orderDate DATE NOT NULL,
    complete BIT NOT NULL,
    orderTotal DECIMAL(10, 2) NOT NULL,
    created_at DATETIME NULL DEFAULT NULL,
    updated_at DATETIME NULL DEFAULT NULL,
    seller_id INT NOT NULL,
    FOREIGN KEY (seller_id) REFERENCES Users(id),
    FOREIGN KEY (customer_id) REFERENCES Customers(id)
);
GO

-- Create OrderItems table (Details separated from Orders table)
CREATE TABLE ExportOrderItems (
    id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    order_id INT NOT NULL,
    product_id INT NOT NULL,
    volume INT NOT NULL,
    product_cost DECIMAL(10, 2) NOT NULL,
    total DECIMAL(10, 2) NOT NULL,
    created_at DATETIME NULL DEFAULT NULL,
    updated_at DATETIME NULL DEFAULT NULL,
    seller_id INT NOT NULL,
    FOREIGN KEY (order_id) REFERENCES ExportOrders(id),
    FOREIGN KEY (product_id) REFERENCES Products(id),
    FOREIGN KEY (seller_id) REFERENCES Users(id)
);
GO

-- Create Reports table (General report information)
CREATE TABLE Reports (
    id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    customer_id INT NOT NULL,
    TotalPrice DECIMAL(18, 2) NOT NULL,
    created_at DATETIME NULL DEFAULT NULL,
    updated_at DATETIME NULL DEFAULT NULL,
    seller_id INT NOT NULL,
    FOREIGN KEY (seller_id) REFERENCES Users(id),
    FOREIGN KEY (customer_id) REFERENCES Customers(id)
);
GO

-- Create ReportDetails table (Details separated from Reports table)
CREATE TABLE ReportDetails (
    report_id INT NOT NULL PRIMARY KEY,
    details NVARCHAR(MAX) NULL,
    FOREIGN KEY (report_id) REFERENCES Reports(id)
);
GO

-- Create ReportsOrdersMapping table
CREATE TABLE ReportsExportOrdersMapping (
    id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    report_id INT NOT NULL,
    order_id INT NOT NULL,
    seller_id INT NOT NULL,
    FOREIGN KEY (seller_id) REFERENCES Users(id),
    FOREIGN KEY (report_id) REFERENCES Reports(id),
    FOREIGN KEY (order_id) REFERENCES ExportOrders(id)
);
GO

-- Create SellerDetails table (Separated from Users table specifically for seller details)
CREATE TABLE SellerDetails (
    user_id INT NOT NULL PRIMARY KEY,
    business_name NVARCHAR(255) NULL,
    business_address NVARCHAR(255) NULL,
    FOREIGN KEY (user_id) REFERENCES Users(id)
);
GO
