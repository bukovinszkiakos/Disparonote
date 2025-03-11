# DisparoNote

## About The Project
DisparoNote is a secure and private self-destructing notes application. Users can create notes that generate a unique shareable link. Once the note is viewed, it gets deleted permanently. The backend is built with **ASP.NET Core**, and the frontend is built using **Next.js**.

## Core Features
- ✅ Secure self-destructing notes
- ✅ User authentication & authorization using JWT
- ✅ Note expiration based on time
- ✅ Modern UI built with Next.js
- ✅ Backend API with authentication and encryption
- ✅ Dockerized database for easy deployment

## Screenshot
![DisparoNote Screenshot]()

## Built With / Tech Stack
- **Backend:** ASP.NET Core 8
- **Frontend:** Next.js
- **Database:** SQL Server (running in Docker)
- **Authentication:** JWT-based authentication
- **Containerization:** Docker
- **Encryption:** AES-GCM encryption for secure notes

## Prerequisites / Dependencies
Before running the application, ensure you have:
- 🛠 **.NET 8+ installed**
- 🛠 **Docker installed and running**
- 🛠 **SQL Server configured in a Docker container**
- 🛠 **Node.js installed (for Next.js frontend)**

## 🚀 How to Run
### 1️⃣ Clone the repository
```sh
git clone https://github.com/yourusername/disparonote.git
cd disparonote
```

### 2️⃣ Start the Database (Docker)
```sh
docker run --name disparonote-db -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=yourStrong(!)Password' -p 1433:1433 -d mcr.microsoft.com/mssql/server:latest
```

### 3️⃣ Configure the Backend
Modify `appsettings.json` if needed:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DisparoNoteDb;User Id=sa;Password=yourStrong(!)Password;Encrypt=False;TrustServerCertificate=True;"
  },
  "Jwt": {
    "ValidIssuer": "apiWithAuthBackend",
    "ValidAudience": "apiWithAuthBackend",
    "IssuerSigningKey": "!SomethingSecret!!SomethingSecret!"
  }
}
```

### 4️⃣ Run the Backend
```sh
cd backend
dotnet restore
dotnet run
```

### 5️⃣ Run the Frontend
```sh
cd frontend
npm install
npm run dev
```

## 👥 Contributors
- **Ákos Bukovinszki** ([github](https://github.com/bukovinszkiakos))

## 🔧 Key Configuration Values
- 🔑 Connection string is stored in `appsettings.json`
- 🔑 JWT secrets should be stored securely (e.g., environment variables)
- 🔑 Database runs inside a Docker container

## 📌 Additional Notes
- ❗ Avoid port **5000** on MacOS as it's commonly used
- ✅ The application follows **Clean Code principles**
- ✅ **Automated tests** are implemented in the backend
- 🔐 **AES-GCM encryption** is used for note security



