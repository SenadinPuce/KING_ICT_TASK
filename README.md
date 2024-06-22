# .NET Web API with Redis Caching

This project is a .NET Web API that utilizes Redis for caching. The solution is dockerized and can be easily started using Docker Compose. 
It includes JWT authentication and policy-based authorization.

## Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)
- [Git](https://git-scm.com/downloads) (required for cloning the repository using Git command)

### Clone the Repository

First, clone the repository to your local machine:

`git clone https://github.com/SenadinPuce/KING_ICT_TASK.git`

### Running the Application
To build and run the application using Docker Compose, navigate to the solution folder and use the following command:

`docker-compose up --build`

This will start the application and its dependencies. The API will be accessible at `http://localhost:8080`.

## API Documentation

The API endpoints are documented using Swagger. Once the application is running, you can access the documentation at:

`http://localhost:8080/swagger/index.html`

## Authentication

The API uses JWT authentication. Use the following test users to authenticate:

### Admin User

- **Username:** emilys
- **Password:** emilyspass
- **Role:** admin

### Regular User

- **Username:** averyp
- **Password:** averyppass

After logging in using the `/login` endpoint, you will receive a token. 
Copy this token and use it in the Swagger UI by clicking the **Authorize** button and pasting the token.

## Users Endpoint

- **Admin-Only Endpoint:** `/api/Users`
   
This is an endpoint to get all users, which can be useful for testing endpoints with different users. 
Some endpoints require only authentication, while others require the user to have the admin role.

**Note: Only users with the admin role can access this endpoint.**

## Redis Commander

Redis Commander is available for viewing cached data. You can access it at:

`http://localhost:8081/`

### Credentials

- **Username:** root
- **Password:** secret

## Running Unit Tests

To run the unit tests, navigate to the solution folder and use the following command:

`dotnet test`

## Summary of Endpoints

- **Authenticated Endpoints:** Require the user to be logged in.
- **Admin-Only Endpoints:** Require the user to have the admin role.
- **Public Endpoints:** Can be accessed without authentication.

### Example Endpoints

- **Login:** `/api/Users/login`
- **Get Product by ID:** `api/Products/{id}` (requires authentication)
- **Search Products by name:** `/api/Products/search`

### Contact

For any questions or issues, please contact me at [senadin.puce@gmail.com](mailto:senadin.puce@gmail.com).
