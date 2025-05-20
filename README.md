# 🐾 PawFund Platform

PawFund là nền tảng web hỗ trợ nhận nuôi và gây quỹ cho các động vật bị bỏ rơi hoặc trong các trại cứu hộ.  
Người dùng có thể đăng ký, gửi đơn xin nhận nuôi, quản lý danh sách thú cưng, quyên góp và nhiều tính năng khác.

---

### 🌟 Features

-  **User registration and authentication**  
-  **Adoption request with approval system**  
-  **Admin dashboard to manage user approvals**  
-  **Email notifications for approval/denial status**  
-  **Pet listing and management by shelter staff**  
-  **Donation system for supporting pets**  
-  **Role-based access control:** Admin, Shelter Staff, Adopter, Guest  

---

### 🛠️ Tech Stack

- Backend: ASP.NET Core Web API (.NET 8)  
- Frontend: React  
- Database: Microsoft SQL Server  
- ORM: Entity Framework Core  
- Email Service: SMTP-based custom mail service  

---

### 📁 Project Structure

- `Models/` – Entity classes representing database tables  
- `Services/` – Business logic and email sending service  
- `Controllers/` – API endpoints for frontend consumption  

---

### ⚙️ Quick Setup Instructions

1. Clone this repository  
2. Configure your database connection string in `appsettings.json`  
3. Build and run the application with your preferred IDE or CLI  
---

### 🚀 Usage

- Register an account or login via the frontend  
- Browse available pets for adoption  
- Submit adoption requests and track approval status  
- Shelter staff can add/edit pet listings  
- Donate to support shelter animals via integrated payment system  
- Admin manages users, adoption approvals, and oversees platform activities  

---

### 📡 API Endpoints Overview

| Endpoint                   | Method | Description                        | Roles Allowed              |
|----------------------------|--------|----------------------------------|----------------------------|
| `/api/auth/register`        | POST   | Register a new user              | Guest                      |
| `/api/auth/login`           | POST   | User login                      | Guest                      |
| `/api/pets`                 | GET    | List all available pets          | All                        |
| `/api/pets`                 | POST   | Add a new pet listing            | Shelter Staff, Admin       |
| `/api/adoptions`            | POST   | Submit adoption request          | Adopter                    |
| `/api/adoptions/approve`    | POST   | Approve or deny adoption request | Admin                      |
| `/api/donations`            | POST   | Make a donation                  | Logged-in users            |
| ...                        | ...    | ...                              | ...                        |

> For detailed API docs, please refer to Swagger UI.

---

### 🤝 Contributing

We welcome contributions!  
- Fork the repo  
- Create a feature branch 
- Commit your changes
- Push to branch
- Open a Pull Request  

Please make sure to follow the coding conventions and include relevant tests.

---

### 📬 Contact

For questions, feedback, or collaboration opportunities:  [Email](ghoul1645@gmail.com)

---

### ⚠️ Notes

- This repo contains only the **backend API**.  
- The **frontend React app** is available at: [Font-end](https://github.com/tamFEDev/pawfundFE)  
- Make sure to configure your SMTP settings for email notifications.  

---

Thank you for supporting PawFund and helping animals find loving homes! 🐕🐈❤️
