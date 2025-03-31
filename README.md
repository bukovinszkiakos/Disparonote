# 📝 DisparoNote

## 📌 About The Project

**DisparoNote - Secure Self-Destructing Notes**

DisparoNote is a secure and minimalistic web application that allows users to create and share **self-destructing notes**. Once a note is viewed via its unique access link, it is **deleted permanently**, ensuring complete privacy.

The application features a **Next.js frontend** and an **ASP.NET Core backend** with **JWT-based authentication** and **AES encryption**. Everything runs in **Docker**, including the database, making the app easily deployable and reproducible in any environment.

---

### ✨ Core Features

- ✅ Secure, one-time-access notes  
- ✅ Notes expire after being read  
- ✅ Optional expiration by time  
- ✅ AES-GCM encryption for stored notes  
- ✅ User authentication & JWT tokens  
- ✅ Clean RESTful API design  
- ✅ Responsive, modern UI (Next.js)  
- ✅ Dockerized backend and frontend
- ✅ Dockerized SQL Server for local development

---

## 📸 Screenshot

_Work in progress – UI preview coming soon._

---

## 🔧 Built With

- ![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-5C2D91?style=for-the-badge&logo=dotnet&logoColor=white)
- ![Next.js](https://img.shields.io/badge/Next.js-000000?style=for-the-badge&logo=nextdotjs&logoColor=white)
- ![JWT](https://img.shields.io/badge/JWT-black?style=for-the-badge&logo=JSON%20web%20tokens)
- ![React](https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)
- ![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
- ![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
- ![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
- ![NUnit](https://img.shields.io/badge/NUnit-009040?style=for-the-badge&logo=dotnet&logoColor=white)

---

## 🚀 Getting Started

### 1️⃣ Prerequisites

Make sure you have the following installed on your machine:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [Node.js (v18+)](https://nodejs.org/en) *(only needed if you want to run frontend outside Docker)*  
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

---

### 2️⃣ Clone the repository

```bash
git clone https://github.com/bukovinszkiakos/disparonote.git
cd disparonote
```

---

### 3️⃣ Start the application using Docker

```bash
docker compose up -d
```

> This will automatically start:
> -  SQL Server database (port 1433)
> - Backend ASP.NET Core API (port 5000) 
> - Frontend Next.js app (port 3000)

---




## 🧪 Testing

### ✅ Unit Tests

```bash
dotnet test DisparonoteUnitTests
```

### ✅ Integration Tests

```bash
dotnet test DisparonoteIntegrationTests
```

---

## 📅 Roadmap

### ✅ Completed

- [x] JWT Authentication & cookie support  
- [x] AES encrypted note storage  
- [x] One-time note retrieval & deletion  
- [x] Docker support for SQL Server  
- [x] Full unit & integration test coverage  

### 🚧 Planned

- [ ] Email notification for opened notes
- [ ] Email verification on user registration
- [ ] Password-protected notes
- [ ] Light/dark mode toggle in frontend UI   

---

## 👨‍💻 Developer

- **Ákos Bukovinszki**  
  [GitHub Profile](https://github.com/bukovinszkiakos)

---

## 🛡️ License

This project is licensed under the MIT License.

---

## 📬 Contact

📂 Repository: [https://github.com/bukovinszkiakos/disparonote](https://github.com/bukovinszkiakos/disparonote)

---

<p align="right">(<a href="#top">Back to top</a>)</p>
```
