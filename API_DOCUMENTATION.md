# Unit Conversion API Documentation

## Overview

The Unit Conversion API provides high-performance unit conversions across multiple categories such as Length, Weight, and Temperature. All conversion rules are loaded into memory at application startup, enabling fast and efficient calculations without database dependencies.

**Base URL**

```text
http://localhost:5000
```

**API Version**

```text
v1
```

---

# Response Format

All successful API responses follow a consistent response contract:

```json
{
  "success": true,
  "message": "Success",
  "data": {},
  "timestamp": "2026-06-15T02:30:00Z"
}
```

---

# Endpoints

## 1. Get All Conversion Categories

Returns all available conversion categories along with their configured units and base unit information.

### Request

```http
GET /api/v1/conversion/categories
Accept: application/json
```

### Success Response (200 OK)

```json
{
  "success": true,
  "message": "Success",
  "data": {
    "length": {
      "baseUnit": "meter",
      "units": {
        "meter": 1.0,
        "kilometer": 0.001,
        "mile": 0.000621371
      }
    },
    "temperature": {
      "baseUnit": "celsius",
      "units": {}
    }
  },
  "timestamp": "2026-06-15T02:31:12Z"
}
```

---

## 2. Get Available Units for a Category

Returns all supported units for a specific conversion category.

### Request

```http
GET /api/v1/conversion/{category}/units
```

### Example

```http
GET /api/v1/conversion/length/units
```

### Success Response (200 OK)

```json
{
  "success": true,
  "message": "Success",
  "data": [
    "meter",
    "kilometer",
    "mile"
  ],
  "timestamp": "2026-06-15T02:31:45Z"
}
```

### Error Response (404 Not Found)

Returned when the specified category does not exist.

```json
{
  "title": "Resource Not Found",
  "status": 404,
  "detail": "Conversion category 'speed' was not found.",
  "instance": "/api/v1/conversion/speed/units"
}
```

---

## 3. Execute Single Conversion

Performs a conversion between two units within the same category.

### Request

```http
POST /api/v1/conversion
Content-Type: application/json
```

### Request Body

```json
{
  "category": "length",
  "fromUnit": "kilometer",
  "toUnit": "meter",
  "value": 2.5
}
```

### Success Response (200 OK)

```json
{
  "success": true,
  "message": "Success",
  "data": 2500.0,
  "timestamp": "2026-06-15T02:32:01Z"
}
```

### Error Response (400 Bad Request)

Returned when an invalid category or unit is provided.

```json
{
  "title": "Bad Request",
  "status": 400,
  "detail": "The unit 'invalid_unit' is not supported under category 'length'.",
  "instance": "/api/v1/conversion"
}
```

---

## 4. Execute Batch Conversion

Processes multiple conversion requests in a single API call.

Each conversion request is executed independently. If one conversion fails, the remaining conversions continue to execute.

### Request

```http
POST /api/v1/conversion/batch
Content-Type: application/json
```

### Request Body

```json
{
  "conversions": [
    {
      "category": "length",
      "fromUnit": "kilometer",
      "toUnit": "meter",
      "value": 1.5
    },
    {
      "category": "weight",
      "fromUnit": "bad_unit",
      "toUnit": "kilogram",
      "value": 10
    }
  ]
}
```

### Success Response (200 OK)

```json
{
  "success": true,
  "message": "Success",
  "data": {
    "results": [
      {
        "success": true,
        "request": {
          "category": "length",
          "fromUnit": "kilometer",
          "toUnit": "meter",
          "value": 1.5
        },
        "result": 1500.0,
        "errorMessage": null
      },
      {
        "success": false,
        "request": {
          "category": "weight",
          "fromUnit": "bad_unit",
          "toUnit": "kilogram",
          "value": 10
        },
        "result": null,
        "errorMessage": "Invalid input mappings provided for category 'weight'."
      }
    ]
  },
  "timestamp": "2026-06-15T02:32:44Z"
}
```

---

# Error Handling

The API uses standardized error responses based on HTTP status codes.

## 400 Bad Request

```json
{
  "title": "Bad Request",
  "status": 400,
  "detail": "Request validation failed.",
  "instance": "/api/v1/conversion"
}
```

## 404 Not Found

```json
{
  "title": "Resource Not Found",
  "status": 404,
  "detail": "Requested resource was not found.",
  "instance": "/api/v1/conversion"
}
```

## 429 Too Many Requests

Returned when the configured rate limit threshold is exceeded.

```json
{
  "title": "Too Many Requests",
  "status": 429,
  "detail": "Rate limit exceeded. Please wait before making more requests.",
  "instance": "/api/v1/conversion"
}
```

## 500 Internal Server Error

```json
{
  "title": "Internal Server Error",
  "status": 500,
  "detail": "An unexpected error occurred while processing the request.",
  "instance": "/api/v1/conversion"
}
```

---

# Notes

* All unit names are case-insensitive.
* Batch conversions are processed independently and support partial success scenarios.
* Conversion definitions are loaded into memory during application startup for maximum performance.
* API versioning is implemented through URL routing (`/api/v1/...`).
* Rate limiting is enabled to protect the API from excessive traffic.
