# Product Inventory API

This is a RESTful API for managing products, including product creation, updates, deletion, and stock management. The API is built with ASP.NET Core and uses Dependency Injection to manage the service layer for business logic.

## Features

- Retrieve all products
- Retrieve a specific product by ID
- Add new products
- Update existing products
- Delete products
- Increment or decrement stock quantity

## Prerequisites

To run this project, you will need:

- [.NET Core SDK](https://dotnet.microsoft.com/download) (Version 6.0 or above)
- PostgreSQL (or any other configured database)
- Docker (optional, for containerized PostgreSQL setup)

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/product-inventory-api.git
```

### 2. Set Up the Database
**Option 1: Using Docker**

You can spin up a PostgreSQL container using the following command:

```bash
docker run --name product_inventory_db -e POSTGRES_USER=user -e POSTGRES_PASSWORD=password -e POSTGRES_DB=product_inventory -p 5432:5432 -d postgres
```

Make sure to update your appsettings.json to match the database connection string.

#### Option 2: Local PostgreSQL Setup
If you prefer a locally installed PostgreSQL, configure it with the following:

- **Host**: `localhost`
- **Port**: `5432`
- **Database**: `product_inventory`
- **Username**: `user`
- **Password**: `password`

## 3. Update Connection Strings

Update the connection string in `appsettings.json` with your database credentials:

```json
{
  "ConnectionStrings": {
    "ProductInventoryDb": "Host=localhost;Port=5432;Database=product_inventory;Username=user;Password=password"
  }
}
```

## 3. Build and Run the API

To compile the project and check for any errors, run the following command in your terminal:

```bash
dotnet build
dotnet run
```
This command starts the application, and you should see output in the terminal indicating that the server is running. You can access the API at https://localhost:7221

## API Endpoints

### Products

- **GET** `/api/products`: Retrieves all products.
- **GET** `/api/products/{productId}`: Retrieves a product by ID.
- **POST** `/api/products`: Adds a new product.
- **PUT** `/api/products/{productId}`: Updates an existing product.
- **DELETE** `/api/products/{productId}`: Deletes a product.
- **PUT** `/api/products/add-to-stock/{productId}/{quantity}`: Increments the stock for a specific product.
- **PUT** `/api/products/decrement-stock/{productId}/{quantity}`: Decrements the stock for a specific product.


## Enhancement

First of all thanks for the opportunity and the feedback on this exercise. As part of feedback I have made few changes in the repository.

- Migrated the project to .NET 8 from .NET 6.
- Updated the Product context - removed field validation from DB.
- Product model validation happens at code rather than in the database since it is expensive.
- Fixed failing testcases.
- Added Dockerfile for the microservice.
- Cleaned .gitignore file.

**Note:** There was a suggestion to use SQLite for the testcases, but SQLite doesn't support the sequences which I have used to generate the ProductId. So I had to continue using the in-memory DB for testing.
 



