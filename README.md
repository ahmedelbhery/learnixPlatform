## 🚀 Overview

**Learnix** is an online learning platform similar to Udemy and Coursera.  
It allows **students** to browse and enroll in courses, **instructors** to create and manage content, and **admins** to control and monitor the entire system through a complete dashboard.

---

## 👥 System Roles

### **1️⃣ Student**
Students can:
- View all available courses  
- Access course details (price, description, sections, lessons…)  
- Enroll in (purchase) courses  
- Access purchased course content  
- Receive welcome emails upon registration  

---

### **2️⃣ Instructor**
Instructors can:
- Create courses, sections, and lessons  
- Manage and edit their course content  
- Track their course status (Pending, Approved, Rejected)  
- Receive email notifications for approval or rejection  

---

### **3️⃣ Admin**
Admins have a dedicated dashboard with full control:
- Manage all users (Students / Instructors)  
- Approve or reject instructor accounts  
- Track instructor performance and earnings  
- View number of enrolled students per course  
- Review and approve/reject courses before publishing  
- Inspect course content before approval  
- Access platform analytics and insights  

---

## 📧 Email Notifications (SMTP)

The system automatically sends emails for:
- New user registration (Welcome email)  
- Instructor approval/rejection  
- Course approval/rejection  
- Important instructor notifications  

---

## 🧱 Architecture

### **📂 Repository Pattern**
All database operations are abstracted into repositories:
- Courses  
- Sections  
- Lessons  
- Users  
- Enrollments  
- InstructorRequests  

---

### **🧠 Service Layer**
Handles the business logic:
- Course creation & publishing flow  
- Instructor validation  
- Enrollment logic  
- Email sending  
- Admin approval workflows  

---

### **🎨 ViewModels & Views**
- Custom ViewModels for clean separation from the database  
- Secure data transfer between layers  
- Optimized view rendering  

---

### **🏛 Areas Structure**
The project is organized into multiple areas:
- `/Admin` — Admin dashboard  
- `/Instructor` — Instructor panel  
- `/Student` — Student interface  
- `/Auth` — Register & Login  

---

### **🌱 Seed Data**
System seeds:
- Default Admin account  
- User roles (Admin, Instructor, Student)  
- Basic initial data  

---

## 🔐 Authentication & Authorization
- ASP.NET Identity  
- Role-Based Authorization (RBAC)  
- Middleware for protecting routes  
- JWT Ready (optional extension for API integration)  

---

## 📦 Tech Stack

| Technology | Purpose |
|-----------|----------|
| **ASP.NET MVC** | Main web framework |
| **Entity Framework Core** | ORM |
| **SQL Server** | Database |
| **Repository Pattern** | Clean data access |
| **Service Layer** | Business logic |
| **Bootstrap** | UI styling |
| **SMTP** | Email sending |
| **Identity** | Authentication & roles |
| **Areas** | Application structure |

---

## 🖼 Key Features Summary
- Multi-role platform (Admin, Instructor, Student)  
- Course approval workflow  
- Instructor verification  
- Email notifications  
- Course creation & management panel  
- Full CRUD operations  
- Modern UI with Bootstrap  
- Scalable architecture  

---

## 🛠 How to Run the Project

1. Update **appsettings.json**:
   - Database connection string  
   - SMTP settings  

2. Run Entity Framework migrations:
   ```bash
   update-database
