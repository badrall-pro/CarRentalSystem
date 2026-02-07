# CarRentalSystem
# 🚗 Car Rental Management System

## Technical Documentation & Project Report  

A comprehensive digital solution designed to automate and optimize car rental business operations.

---

## 📌 Executive Summary

The **Car Rental Management System** is a full-stack application composed of two main platforms:

- **Back-Office (WPF Desktop Application)**  
  Used by administrators and employees to manage operations.

- **Front-Office (ASP.NET MVC Web Application)**  
  Used by customers to browse vehicles and make reservations online.

---

## ✨ Key Features

### 🔐 Administrative Features

- User management with role-based access (Administrator, Employee, Customer)
- Vehicle fleet management with real-time availability
- Reservation workflow (Booking → Confirmation → Rental → Completion)
- Payment processing and tracking
- Dashboard with business analytics
- Excel export
- PDF receipt generation

---

### 👤 Customer Features

- User registration and authentication
- Vehicle catalog with advanced search
- Online booking with availability checking
- Reservation history and management
- PDF receipt download

---

## 🛠 Technology Stack

| Component        | Technology               | Version |
|------------------|--------------------------|---------|
| Backend          | ASP.NET Core Web API     | .NET 8.0 |
| Database         | PostgreSQL               | 16.0 |
| ORM              | Entity Framework Core    | 8.0 |
| Desktop App      | WPF                      | .NET 8.0 |
| Web App          | ASP.NET Core MVC         | .NET 8.0 |
| Authentication  | JWT Tokens               | — |
| Architecture    | Clean Architecture + CQRS| — |

---

## 📊 Project Metrics

- **Lines of Code:** ~15,000+
- **Database Tables:** 8 entities
- **User Roles:** 3 (Administrator, Employee, Customer)

---

## 🎯 Project Objectives

### Business Objectives

1. Digitize operations by replacing manual processes
2. Improve efficiency and reduce administrative overhead
3. Enhance customer experience with 24/7 online booking
4. Enable data-driven decisions using real-time analytics
5. Build a scalable foundation for future growth

---

### Technical Objectives

- Implement Clean Architecture with clear separation of concerns
- Ensure data security using JWT authentication and password hashing
- Build responsive and user-friendly interfaces
- Provide comprehensive API documentation

---

## 📦 Project Scope

### Included Features

- Vehicle and reservation management
- Customer registration and authentication
- Payment tracking and reporting
- PDF receipts with QR codes
- Email notifications
- Excel data export

---

### 🚀 Future Enhancements

- Online payment gateway integration
- Mobile applications
- GPS vehicle tracking
- Multi-location support

---

## 🔄 Functional Requirements

### Use Case: Customer Registration & Login

**Actor:** Customer  

**Flow:**

1. Customer navigates to registration page  
2. Fills form with personal and license information  
3. System validates data (age ≥ 18, unique email)  
4. Account is created and confirmation email sent  
5. Customer logs in using credentials  

**Postcondition:**  
Customer account is active and ready for reservations.

---

## 🧱 Architecture Overview

- Clean Architecture
- CQRS Pattern
- ASP.NET Web API for backend services
- WPF Desktop App for Back-Office
- MVC Web App for Front-Office
- PostgreSQL as relational database
- JWT-based authentication

---

## 📁 Applications

### Back-Office (WPF)

- Employee & admin management
- Vehicle CRUD
- Reservation handling
- Payment monitoring
- Analytics dashboard

### Front-Office (Web MVC)

- Customer registration/login
- Vehicle browsing
- Online reservations
- Receipt downloads

---

## 🔐 Security

- JWT authentication
- Role-based authorization
- Password hashing
- Secure API endpoints

---

## 📄 Reporting

- PDF receipts
- QR codes
- Excel exports
- Business analytics dashboard

---

## 📌 Conclusion

This project delivers a complete, scalable, and secure car rental management platform.  
It modernizes business workflows while providing a smooth customer experience and a strong technical foundation for future expansion.

---
