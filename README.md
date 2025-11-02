# 📚 BookStore API

A RESTful ASP.NET Core Web API for managing books, members, and borrowing operations in a digital library system.

---

## 🚀 Features
- Add, update, and delete books and members  
- Borrow and return books with availability tracking  
- Role-based authentication (optional, via AuthController)  
- Entity Framework Core integration with SQL Server  
- Comprehensive unit tests using xUnit and EF InMemory  

---

## 🛠️ Technologies Used
| Category | Technology |
|-----------|-------------|
| Backend | ASP.NET Core Web API (.NET 9) |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Testing | xUnit, EFCore InMemory Provider |
| Tools | Visual Studio , Git, Postman |

---

## 🧭 How to Clone & Run Locally

### 1️⃣ Clone Repository
```bash
git clone https://github.com/Rohit-Baranwal/BookStoreAPI.git
cd BookStoreAPI
```
## 2. Configure the Database Connection

Open `appsettings.json` and set your SQL Server connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your-server-name;Database=your-database-name;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

## 3. Set Up the migration and Database

Open the package manager console and run the following commands

```bash
Add-migration your-migration-name
Update-Database
```
## 4. Run the application
After that run the application by using `dotnet run ` command
