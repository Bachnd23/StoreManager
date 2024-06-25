-- Create database
CREATE DATABASE StoreManager;
GO

-- Use the newly created database
USE StoreManager;
GO

-- Create Products table
CREATE TABLE Products (
    id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(255) NOT NULL,
	MeasureUnit NVARCHAR(255) NOT NULL,
    cost INT NOT NULL,
    created_at DATETIME NULL DEFAULT NULL,
    updated_at DATETIME NULL DEFAULT NULL
);
GO

-- Create Customers table
CREATE TABLE Customers (
    id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(MAX) NOT NULL,
    address NVARCHAR(MAX) NOT NULL,
    phone NVARCHAR(MAX) NOT NULL,
    note NVARCHAR(MAX) NOT NULL,
    active BIT NOT NULL,
    created_at DATETIME NULL DEFAULT NULL,
    updated_at DATETIME NULL DEFAULT NULL
);
GO

-- Create Orders table
CREATE TABLE Orders (
    id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    customer_id INT NOT NULL,
    product_id INT NOT NULL,
    volume INT NOT NULL,
    date DATE NOT NULL,
    complete BIT NOT NULL,
    created_at DATETIME NULL DEFAULT NULL,
    updated_at DATETIME NULL DEFAULT NULL,
    FOREIGN KEY (customer_id) REFERENCES Customers(id),
    FOREIGN KEY (product_id) REFERENCES Products(id)
);
GO

-- Create UserRoles table
CREATE TABLE UserRoles (
    id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL
);
GO

-- Create Users table
CREATE TABLE Users (
    id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    role INT NOT NULL,
    remember_token NVARCHAR(100) DEFAULT NULL,
    created_at DATETIME NULL DEFAULT NULL,
    updated_at DATETIME NULL DEFAULT NULL,
    FOREIGN KEY (role) REFERENCES UserRoles(id)
);
GO

-- Create Reports table
CREATE TABLE Reports (
    id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    customer_id INT NOT NULL,
    TotalPrice DECIMAL(18, 2) NOT NULL,
    created_at DATETIME NULL DEFAULT NULL,
    updated_at DATETIME NULL DEFAULT NULL,
    FOREIGN KEY (customer_id) REFERENCES Customers(id)
);
GO

-- Create ReportsOrdersMapping table
CREATE TABLE ReportsOrdersMapping (
    id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    report_id INT NOT NULL,
    order_id INT NOT NULL,
    FOREIGN KEY (report_id) REFERENCES Reports(id),
    FOREIGN KEY (order_id) REFERENCES Orders(id)
);
GO
