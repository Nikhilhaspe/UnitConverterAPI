# 🚀 Unit Conversion API

## 📄 Description of the Solution

This is a high-performance, production-ready **Unit Conversion API** built natively with modern **.NET 9**. It serves as an ultra-fast, lightweight engine designed to handle standard unit conversions (such as Length and Weight) alongside formula-based calculations (like Temperature) instantly.

To maximize speed and efficiency, the API operates entirely **in-memory**. It loads all conversion rules once during startup, completely eliminating database delays and network bottlenecks. This allows the system to process high volumes of concurrent user traffic smoothly while keeping resource usage exceptionally low.

---

## ✨ Key Capabilities

| Capability                   | Description                                                                                                                                                                  |
| ---------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ✅ Consistent Response Format | Automatically wraps all successful API responses into a clean, uniform structural template.                                                                                  |
| ✅ Resilient Batch Processing | Allows users to submit multiple conversions at once; the system isolates processing errors so that valid conversions succeed even if other items in the batch contain typos. |
| ✅ Built-in Security          | Features native traffic regulation to protect the application from automated spam and resource abuse.                                                                        |
| ✅ Clean Error Handling       | Intercepts all inputs and application errors globally, transforming them into clear, standardized error messages rather than exposing raw system crashes.                    |

---

# 🏃 Running the Application Locally

There are two ways to run the application locally:

---

## 1️⃣ Run the Self-Contained Published Version

### Steps

* Navigate to the published output folder.
* Run the executable:

```bash
UnitConvertAPI.exe
```

* Once the application starts, open your browser and navigate to:

```text
http://localhost:5000/api/v1/conversion/categories
```

* This can be used to verify that the API is running successfully.

---

## 2️⃣ 🚀 How To Run The Application Locally?

### Prerequisites

* Install **.NET 9 SDK**.
* Install **Visual Studio Code** (or Visual Studio).

### Steps

#### 1. Open the project in VS Code.

#### 2. Restore and build the project:

```bash
dotnet restore
dotnet build
```

#### 3. Run the application:

```bash
dotnet run
```

#### 4. In **Development** mode, Swagger UI is enabled and can be used to test the APIs interactively.

Navigate to:

```text
http://localhost:5228/swagger/index.html
```

(or the URL displayed in the console when the application starts).

---

# 🧠 Design Decisions & Trade-Offs

---

## 1. Adherence to SOLID Principles

The codebase is structured strictly around SOLID design principles to ensure maintainability and clean separation of concerns:

* **Single Responsibility Principle (SRP):** Controllers are strictly responsible for routing requests, leaving all mathematical algorithms and data validation to a dedicated service layer.
* **Dependency Inversion Principle (DIP):** Components communicate exclusively via abstractions (interfaces), making the system highly testable and loosely coupled.

---

## 2. Leveraging Modern .NET 9 Features for DRY Architecture

To enforce the **DRY (Don't Repeat Yourself)** principle and maximize reusability, the solution leverages modern framework features:

* **Native Middleware & Rate Limiting:** Traffic regulation and security boundaries are handled at the network edge before hitting application code.
* **Action Filters:** Eliminates repetitive scaffolding logic across endpoints by automatically intercepting execution contexts.

---

## 3. Maintainability & Developer Experience (DX)

The code is intentionally decorated with meaningful, self-documenting comments. Rather than explaining *what* the code does (which clean code should show), the comments focus on explaining the *why* behind specific business rules and data fallback sequences, ensuring future developers can onboard and extend the system effortlessly.

---

## 4. Consistent API Contracts via Filters & Global Error Handling

To provide an exceptional integration experience for frontend clients, the API guarantees structural consistency for both successes and failures:

### Success Paths

* A custom `IAsyncResultFilter` automatically wraps successful data into a standardized metadata envelope (`success`, `message`, `data`, `timestamp`).

### Failure Paths

* .NET 9’s native `IExceptionHandler` catches validation, framework, and processing errors globally, reshaping them into uniform, RFC-compliant `ProblemDetails` models instead of exposing unhandled server crashes.

---

## 5. Configurable In-Memory Engine & Extensibility

The conversion architecture balances ultra-fast performance with effortless configurability:

* **JSON-Driven Configurations:** New units and structural categories can be added purely through configuration updates without changing a single line of core C# code.
* **Interface-Driven Calculations:** Core mathematical strategies are bound to decoupled interfaces, allowing developers to introduce entirely new, complex non-linear categories seamlessly.

---

## 6. Architectural Trade-Off: Synchronous Execution for CPU-Bound Math

An intentional performance trade-off was made to keep the mathematical computation engine synchronous. Because these conversions are entirely CPU-bound and execute in fractions of a microsecond, forcing asynchronous state machines (`async/await`) or thread bouncing (`Task.Run`) would introduce unnecessary context-switching overhead. Keeping execution synchronous allows the .NET thread pool to process and return computations instantly, maximizing overall throughput for concurrent traffic.

---

## 🎯 Summary

✔ High-performance in-memory architecture

✔ Consistent API response contracts

✔ Global exception handling

✔ Built-in rate limiting

✔ SOLID-driven design

✔ Easily extensible conversion engine

✔ Optimized for CPU-bound workloads

✔ Production-ready .NET 9 implementation
