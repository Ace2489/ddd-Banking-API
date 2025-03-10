
# DDD Banking API  

A **banking API** built with **.NET Core**, following **Domain-Driven Design (DDD)** principles. This project is designed to be **modular, scalable, and maintainable**, making it a solid foundation for real-world fintech applications.  

## Why This Exists  
Banking systems aren’t just about moving money—they need to be **secure, reliable, and built to handle complex business rules**. Instead of a typical CRUD-based API, this project follows **DDD and CQRS** to properly model banking operations and keep business logic clean.  

## What’s Inside  
✅ **Clear DDD Structure** – Proper separation of concerns across **Domain, Application, and Infrastructure**  
✅ **CQRS & MediatR** – Commands and Queries are handled separately for better scalability  
✅ **Event-Driven Approach** – Uses **Domain Events** to keep different parts of the system in sync  
✅ **Extensible and Maintainable** – Built with growth in mind—easy to add features without breaking existing logic  
✅ **Unit & Integration Tests** – Making sure things actually work, not just hoping they do  

## Tech Stack  
- **.NET Core** – Backend framework  
- **EF Core** – ORM for database interactions  
- **MediatR** – Helps with CQRS implementation  
- **FluentValidation** – Keeps inputs in check  
- **xUnit, Moq** – For testing, because bugs are expensive  

## Getting Started  
1. **Clone the repo**  
   ```bash
   git clone https://github.com/Ace2489/ddd-Banking-API.git
   cd ddd-Banking-API/src
   ```  
2. **Set up the database** (update `appsettings.json`)  
3. **Run migrations**  
   ```bash
   dotnet ef database update
   ```  
4. **Start the app**  
   ```bash
   dotnet run
   ```  

## Why You Might Like This  
If you’re into **DDD, CQRS, and well-structured banking logic**, this should feel right at home. It’s a work in progress, so feedback and contributions are always welcome.  

