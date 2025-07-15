# 🧾 Card Validation System

A modern credit card validation system built with:

- ✅ ASP.NET 8 Web API (Backend)
- 🌐 React + TypeScript (Frontend)
- 🐳 Docker Compose (Deployment)

---

## 📁 Project Structure

```
CardValidationSystem/
├── card.validator.api.v1/         # .NET 8 Web API
├── card.validation.app.v1/        # React Frontend App
├── docker-compose.yml             # Docker Multi-Container Setup
```

---

## 🚀 Getting Started

### ✅ Option 1: Run with Docker

#### Prerequisites
- Docker Desktop installed

#### Steps
```bash
cd src
docker compose up --build
```

- React App → http://localhost:3105  
- API Endpoint → http://localhost:5105/api/cards/validate

---

### 🧪 Option 2: Run Locally Without Docker

#### Prerequisites
- .NET 8 SDK
- Node.js 18+

---

### 1️⃣ Run Backend API

```bash
cd card.validator.api.v1
dotnet build
dotnet run
```

API will be available at: `http://localhost:5105`

---

### 2️⃣ Run Frontend React App

```bash
cd card.validation.app.v1
npm install
npm start
```

React app will be available at: `http://localhost:3105`

Make sure the `.env` file inside `card.validation.app.v1` contains:
```
REACT_APP_API_BASE_URL=http://localhost:5105
```

---

## 🔍 API Endpoint

```
POST /api/cards/validate
Content-Type: application/json

{
  "cardNumber": "4111111111111111"
}
```

**Response:**
```json
{
  "cardType": "Visa",
  "isValid": true,
  "formattedNumber": "4111 1111 1111 1111"
}
```


---

## ✅ Tests

```bash
# From project root
dotnet test
```

---

## 🚧 Suggestions to Make the Application Better

If your goal is to harden and polish the system beyond unit testing, consider the following enhancements:

### 🛡️ Validation & Security

- Add input validation using `FluentValidation` or `DataAnnotations`.
- Implement request rate limiting or throttling to prevent abuse.
- Sanitize logs and mask card numbers (e.g., `4111 **** **** 1111`).

### 📈 Observability

- Use structured logging (`Serilog`, or `ILogger`) with correlation IDs.
- Track key metrics: number of validations, success/failure rate, response times.

### 🧪 Testing

- Add integration tests using `WebApplicationFactory`.
- Implement contract/API tests to ensure the Swagger schema aligns with actual behavior.

### ⚙️ Configuration & Environment

- Support centralized configuration via `IOptionsSnapshot`.
- Externalize environment-sensitive settings: CORS, logging, feature flags.

### 🐳 Docker & CI/CD

- Add a `.dockerignore` file to optimize image builds.
- Integrate CI/CD (GitHub Actions, Azure Pipelines) to auto-build/test/deploy.

### 💄 UI/UX Enhancements

- Show real-time card input masking (e.g., `**** **** **** 1234`).
- Add a loading spinner when validating.
- Animate the result area smoothly without shifting UI layout.

---

## ✅ License

MIT License – feel free to use and modify.

---
