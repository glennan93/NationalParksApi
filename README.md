# NationalParksApi


## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [API Usage](#api-usage)
  - [Base URL](#base-url)
  - [Endpoints](#endpoints)
    - [1. Get All Parks](#1-get-all-parks)
    - [2. Create a New Park](#2-create-a-new-park)
    - [3. Update an Existing Park](#3-update-an-existing-park)
    - [4. Delete a Park](#4-delete-a-park)
- [License](#license)
- [Contact](#contact)

## Overview

**NationalParksApi** is a RESTful Web API designed to manage and provide information about various national parks. Built with **ASP.NET Core**, **Entity Framework Core**, and **SQLite**, this API allows users to perform CRUD (Create, Read, Update, Delete) operations on national park data. The project includes data seeding from a JSON file to ensure the database is populated with initial data upon setup.

## Features

- **CRUD Operations:** Create, retrieve, update, and delete national park records.
- **Data Seeding:** Automatically seeds the database with predefined park data from a JSON file.
- **Search and Filtering:** Query parks by name, state, year established, or ID.
- **Database Integration:** Utilizes SQLite for lightweight data storage, managed through Entity Framework Core.
- **Repository Pattern:** Implements a repository pattern for clean separation of concerns and easier maintenance.

## Technologies Used

- [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [SQLite](https://www.sqlite.org/index.html)
- [GitHub](https://github.com/) for version control

## Getting Started

Follow these instructions to set up and run the **NationalParksApi** project locally.

### Prerequisites

- [.NET 6 SDK or later](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/downloads)

### Installation

1. **Clone the Repository:**

    ```bash
    git clone https://github.com/glennan93/NationalParksApi.git
    ```

2. **Navigate to the Project Directory:**

    ```bash
    cd NationalParksApi
    ```

3. **Restore Dependencies:**

    ```bash
    dotnet restore
    ```

4. **Apply Migrations and Seed the Database:**

    ```bash
    dotnet ef database update
    ```

5. **Run the Application:**

    ```bash
    dotnet run
    ```

6. **Access the API:**

    The API will be running at `http://localhost:51135` by default. You can use tools like [Postman](https://www.postman.com/) or [curl](https://curl.se/) to interact with the endpoints.

## API Usage

The **NationalParksApi** provides the following endpoints to manage national park data.

### Base URL

http://localhost:51135/


### Endpoints

#### 1. Get All Parks

- **URL:** `/parks`
- **Method:** `GET`
- **Description:** Retrieves a list of all national parks. Supports optional query parameters for filtering.
- **Query Parameters:**
  - `name` (string): Filter parks by name.
  - `state` (string): Filter parks by state.
  - `year` (integer): Filter parks by the year they were established.
  - `id` (integer): Retrieve a specific park by its ID.

- **Example Request:**

    ```bash
    GET http://localhost:51135/parks
    ```

- **Example Response:**

    ```json
    [
        {
            "id": 1,
            "name": "Acadia National Park",
            "state": "Maine",
            "yearEstablished": 1919
        },
        {
            "id": 2,
            "name": "American Samoa National Park",
            "state": "American Samoa",
            "yearEstablished": 1988
        }
        // ... more parks
    ]
    ```

#### 2. Create a New Park

- **URL:** `/parks`
- **Method:** `POST`
- **Description:** Adds a new national park to the database.
- **Request Body:**

    ```json
    {
        "name": "Yellowstone National Park",
        "state": "Wyoming",
        "yearEstablished": 1872
    }
    ```

- **Example Request:**

    ```bash
    POST http://localhost:51135/parks
    Content-Type: application/json

    {
        "name": "Yellowstone National Park",
        "state": "Wyoming",
        "yearEstablished": 1872
    }
    ```

- **Example Response:**

    ```json
    {
        "id": 64,
        "name": "Yellowstone National Park",
        "state": "Wyoming",
        "yearEstablished": 1872
    }
    ```

#### 3. Update an Existing Park

- **URL:** `/parks/{id}`
- **Method:** `PUT`
- **Description:** Updates the details of an existing national park.
- **Path Parameters:**
  - `id` (integer): The ID of the park to update.
- **Request Body:**

    ```json
    {
        "name": "Yellowstone National Park",
        "state": "Wyoming, Montana, Idaho",
        "yearEstablished": 1872
    }
    ```

- **Example Request:**

    ```bash
    PUT http://localhost:51135/parks/64
    Content-Type: application/json

    {
        "name": "Yellowstone National Park",
        "state": "Wyoming, Montana, Idaho",
        "yearEstablished": 1872
    }
    ```

- **Example Response:**

    ```json
    {
        "id": 64,
        "name": "Yellowstone National Park",
        "state": "Wyoming, Montana, Idaho",
        "yearEstablished": 1872
    }
    ```

#### 4. Delete a Park

- **URL:** `/parks/{id}`
- **Method:** `DELETE`
- **Description:** Removes a national park from the database.
- **Path Parameters:**
  - `id` (integer): The ID of the park to delete.

- **Example Request:**

    ```bash
    DELETE http://localhost:51135/parks/64
    ```

- **Example Response:**

    ```json
    "Park with ID 64 deleted."
    ```