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

Nagyon jó ötlet! Itt egy frissített, letisztult és informatív verzió, amiben benne van az is, hogy a jegyzetet másnak küldöd el — és az automatikus törlés is érthető marad:

---

## 🎞️ Live Demo (GIF)

Below is a quick preview showing how **DisparoNote** works in action:

### ✍️ A user creates a secure note and shares a one-time link with someone else. 
### Once it’s opened, the note is automatically deleted forever.

<img src="https://github.com/user-attachments/assets/5174fae0-7853-4cf4-81a5-2d92c4029dc7" width="100%" />



> 🔐 This is just one of the core flows — DisparoNote also supports features like:
> - time-based expiration
> - AES-GCM encryption
> - JWT-authenticated access to ensure that your private notes stay private.

---

## 🔧 Built With

- [![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-5C2D91?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/en-us/)
- [![Next.js](https://img.shields.io/badge/Next.js-000000?style=for-the-badge&logo=nextdotjs&logoColor=white)](https://nextjs.org/)
- [![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white)](https://jwt.io/)
- [![React](https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)](https://reactjs.org/)
- [![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/en-us/ef/)
- [![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
- [![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/)
- [![NUnit](https://img.shields.io/badge/NUnit-009040?style=for-the-badge&logo=dotnet&logoColor=white)](https://nunit.org/)

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

