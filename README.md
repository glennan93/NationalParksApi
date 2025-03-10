# NationalParksApi

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Authentication and Authorization](#authentication-and-authorization)
- [Pagination](#pagination)
- [API Usage](#api-usage)
  - [Base URL](#base-url)
  - [Endpoints](#endpoints)
    - [1. Get All Parks](#1-get-all-parks)
    - [2. Create a New Park](#2-create-a-new-park)
    - [3. Update an Existing Park](#3-update-an-existing-park)
    - [4. Delete a Park](#4-delete-a-park)
    - [5. Get Current Weather for a Park](#5-get-current-weather-for-a-park)
- [License](#license)
- [Contact](#contact)

---

## Overview

**NationalParksApi** is a RESTful Web API designed to manage and provide information about various national parks. Built with **ASP.NET Core**, **Entity Framework Core**, and **SQLite**, this API allows users to perform CRUD (Create, Read, Update, Delete) operations on national park data. It also provides real-time weather information for specific parks and includes robust authentication and authorization using JWT and ASP.NET Core Identity.

---

## Features

- **CRUD Operations:** Create, retrieve, update, and delete national park records.
- **Data Seeding:** Automatically seeds the database with predefined park data from a JSON file.
- **Search and Filtering:** Query parks by name, state, year established, or ID.
- **Real-Time Weather:** Fetches the current weather for a specified park using Open-Meteo API.
- **Authentication and Authorization:**
  - User registration and login with JWT-based authentication.
  - Role-based authorization with two roles: `Admin` and `User`.
  - Admins can access all endpoints, while Users have limited access.
- **Pagination:** Retrieve parks in a paged manner, including metadata like total records and total pages.
- **Database Integration:** Utilizes SQLite for lightweight data storage, managed through Entity Framework Core.
- **Repository Pattern:** Implements a repository pattern for clean separation of concerns and easier maintenance.

---

## Technologies Used

- [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [SQLite](https://www.sqlite.org/index.html)
- [Open-Meteo API](https://open-meteo.com/) for weather data
- [JWT](https://jwt.io/) for authentication
- [Serilog](https://serilog.net/) for logging
- [GitHub](https://github.com/) for version control

---

## Getting Started

Follow these instructions to set up and run the **NationalParksApi** project locally.

### Prerequisites

- [.NET 6 SDK or later](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/downloads)
- [Postman](https://www.postman.com/) or another API client (optional for testing)

### Installation

1. **Clone the Repository:**

    ```bash
    git clone https://github.com/glennan93/NationalParksApi.git
    ```

2. **Navigate to the Project Directory:**

    ```bash
    cd NationalParksApi
    ```

3. **Set Environment Variables:**

    Create a `.env` file in the project root with the following keys:
    ```
    ADMIN_EMAIL=admin@example.com
    ADMIN_PASSWORD=SecureAdminPassword123!
    JWT_KEY=YourSuperSecretKey
    WEATHER_API_BASE_URL=https://api.open-meteo.com/v1/
    ```

4. **Restore Dependencies:**

    ```bash
    dotnet restore
    ```

5. **Apply Migrations and Seed the Database:**

    ```bash
    dotnet ef database update -c ParkContext
    dotnet ef database update -c ApplicationDbContext
    ```

6. **Run the Application:**

    ```bash
    dotnet run
    ```

7. **Access the API:**

    The API will be running at `https://localhost:7113` by default.

---

## Authentication and Authorization

- **Roles:**
  - `Admin`: Has full access to all endpoints.
  - `User`: Can only view parks and current weather, and can update/delete parks they created.
- **Policy-Based Authorization:**  
  A custom policy ensures that only the creator of a park (or an Admin) can update or delete it.

- **Endpoints for Authentication:**
  - **Register:**
    ```bash
    POST https://localhost:7113/api/Auth/Register
    ```
    Request body:
    ```json
    {
      "email": "user@example.com",
      "password": "SecurePassword123!"
    }
    ```
  - **Login:**
    ```bash
    POST https://localhost:7113/api/Auth/Login
    ```
    Request body:
    ```json
    {
      "email": "user@example.com",
      "password": "SecurePassword123!"
    }
    ```
    The response includes a JWT token to use in subsequent requests.

---

## Pagination

When retrieving parks, you can control which page of results is returned. Use the following query parameters:

- **pageNumber:** The page to retrieve (default is 1).
- **pageSize:** The number of items per page (default is 10).

The API will also return metadata including the total number of records and total pages.

---

## API Usage

### Base URL


http://localhost:7113/


### Endpoints

#### 1. Get All Parks (with Pagination)

- **URL:** `/api/Parks`
- **Method:** `GET`
- **Roles:** `Admin`, `User`
- **Query Parameters (optional):**
  - `name`: Filter parks by name.
  - `year`: Filter parks by the year established.
  - `state`: Filter parks by state.
  - `id`: Retrieve a specific park by its ID.
  - `pageNumber`: The page number to retrieve.
  - `pageSize`: The number of parks per page.
- **Example Request:**
    ```bash
    GET https://localhost:7113/api/Parks?pageNumber=2&pageSize=10
    Authorization: Bearer <Your JWT Token>
    ```
- **Example Response:**
    ```json
    {
        "data": [
            {
                "id": 11,
                "name": "Carlsbad Caverns National Park",
                "state": "New Mexico",
                "yearEstablished": 1930,
                "latitude": 32.147,
                "longitude": -104.556
            },
             ... 9 more parks for this page
        ],
        "pageNumber": 2,
        "pageSize": 10,
        "totalRecords": 63,
        "totalPages": 7
    }
    ```

#### 2. Create a New Park

- **URL:** `/api/Parks`
- **Method:** `POST`
- **Roles:** `Admin, User`
- **Description:** Adds a new park to the database.  
  The park creator's identity is recorded to enforce ownership for future updates or deletions.
- **Example Request:**
    ```bash
    POST https://localhost:7113/api/Parks
    Authorization: Bearer <Your JWT Token>
    Content-Type: application/json
    ```
    Request Body:
    ```json
    {
        "name": "Enchanted Rock State Natural Area",
        "state": "Texas",
        "yearEstablished": 1850,
        "latitude": 50.0,
        "longitude": 50.0
    }
    ```
- **Example Response:**
    ```json
    {
        "id": 64,
        "name": "Enchanted Rock State Natural Area",
        "state": "Texas",
        "yearEstablished": 1850,
        "latitude": 50.0,
        "longitude": 50.0,
        "createdBy": "user@example.com"
    }
    ```


#### 3. Update an Existing Park

- **URL:** `/api/Parks/{id}`
- **Method:** `PUT`
- **Roles:** `Admin` or the park creator (as enforced by the custom policy)
- **Description:** Updates park details. Only the creator of the park or an Admin can update a park.
- **Example Request:**
    ```bash
    PUT https://localhost:7113/api/Parks/64
    Authorization: Bearer <Your JWT Token>
    Content-Type: application/json
    ```
    Request Body:
    ```json
    {
        "name": "Enchanted Rock State Natural Area - Updated",
        "state": "Texas",
        "yearEstablished": 1850,
        "latitude": 51.0,
        "longitude": 51.0,
        "createdBy": "user@example.com"
    }
    ```
- **Example Response:**
    ```json
    {
        "id": 64,
        "name": "Enchanted Rock State Natural Area - Updated",
        "state": "Texas",
        "yearEstablished": 1850,
        "latitude": 51.0,
        "longitude": 51.0,
        "createdBy": "user@example.com"
    }
    ```

#### 4. Delete a Park

- **URL:** `/api/Parks/{id}`
- **Method:** `DELETE`
- **Roles:** `Admin` or the park creator (per policy)
- **Description:** Deletes the specified park.
- **Example Request:**
    ```bash
    DELETE https://localhost:7113/api/Parks/64
    Authorization: Bearer <Your JWT Token>
    ```
- **Example Response:**
    ```json
    "Park with ID 64 deleted."
    ```

#### 5. Get Current Weather for a Park

- **URL:** `/api/Parks/{id}/currentweather`
- **Method:** `GET`
- **Roles:** `Admin`, `User`
- **Description:** Retrieves the current weather for a park using its latitude and longitude.
- **Example Request:**
    ```bash
    GET https://localhost:7113/api/Parks/1/currentweather
    Authorization: Bearer <Your JWT Token>
    ```
- **Example Response:**
    ```json
    {
        "parkName": "Acadia National Park",
        "weatherData": {
            "latitude": 52.52,
            "longitude": 13.419998,
            "temperature": -2.7
        }
    }
    ```

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.

---