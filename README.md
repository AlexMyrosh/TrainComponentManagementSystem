# 🚆 Train Component Management API

## 📌 Overview

This is an ASP.NET Core REST API for managing train components. It supports full CRUD operations, soft/hard deletion, validation, caching, and logging. The system ensures stability and performance with tools like MemoryCache, AutoMapper, FluentValidation, and Serilog.

---

## ✅ Features

- CRUD operations for train components
- Soft delete & hard delete
- Quantity assignment rules
- Search and pagination
- Input validation with **FluentValidation**
- Object mapping with **AutoMapper**
- In-memory caching with **MemoryCache**
- Structured logging with **Serilog**

---

## 🛠️ Tech Stack

- **.NET 9.0**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **AutoMapper**
- **FluentValidation**
- **MemoryCache**
- **Serilog**

---

## 🚀 Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/yourusername/train-component-management.git
cd train-component-management
```

### 2. Install dependencies
```bash
dotnet restore
```

### 3. Apply migrations
```bash
dotnet ef database update
```

### 4. Run the app
```bash
dotnet run
```
---

## 📡 API Endpoints

- **GET** `/api/traincomponents`  
  Get all components with optional pagination and search.  
  **Query Parameters:**  
  - `search`: *(optional)* filter by name or unique number  
  - `page`: *(optional)* default is 1  
  - `pageSize`: *(optional)* default is 20

- **GET** `/api/traincomponents/{id}`  
  Get a single component by ID.

- **POST** `/api/traincomponents`  
  Create a new train component.  
  **Body:**  
  ```json
  {
    "name": "Component A",
    "uniqueNumber": "123-ABC",
    "canAssignQuantity": true,
    "quantity": 5
  }
  
- **PUT** `/api/traincomponents/{id}`
Update an existing train component.
**Body**: same as POST.

- **DELETE** `/api/traincomponents/soft/{id}`
Soft delete a component (marks IsDeleted = true).

- **DELETE** `/api/traincomponents/hard/{id}`
Hard delete a component (removes from database).
