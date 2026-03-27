# Spec: Planet Info API

## Authorization
No authorization is required for this API in the current version.

## Glossary
- **Planet**: A record in the database containing a planet's basic information.
- **Solar System**: The planetary system the planet belongs to.
- **min_temp**: The minimum recorded or defined temperature for the planet.
- **max_temp**: The maximum recorded or defined temperature for the planet.
- **Endpoint**: A specific API route that the client can call to retrieve data.
- **JSON**: JavaScript Object Notation, the response format used by this API.

## 1. Overview / Purpose
This API will let the app pull planet temperature range data from the database to display on a planet info page.

## 2. Scope

### Included
- Get a list of planets
- Get one planet’s info (name, solar system, min/max temp)
- Basic error handling (planet not found)

### Excluded
- Creating, updating, or deleting planets
- User authentication and authorization
- Advanced filtering or sorting

## 3. Functional Requirements
1. The API provides an endpoint to return all planets.
2. The API provides an endpoint to return one planet by name.
3. Each planet record includes: `name`, `solar_system`, `max_temp`, `min_temp`.
4. If a planet name does not exist, the API returns a 404 error message.

## 4. Non-Functional Requirements
- Requests should complete in under 1 second under normal load.
- API responses must be valid JSON and consistent across endpoints.

## 5. Error Handling
- If a requested planet does not exist, the API returns an error response with a clear message.
- If the client sends an invalid request, the API returns an appropriate error message in JSON format.
- The API should avoid exposing internal server or database details in error messages.
- All error responses should follow a consistent JSON structure.

### Example Error Response
```json
{
  "error": "Planet not found"
}
```

## 6. Status Codes
- `200 OK` — Request was successful.
- `404 Not Found` — The requested planet does not exist.
- `400 Bad Request` — The request format or parameter is invalid.
- `500 Internal Server Error` — An unexpected server-side error occurred.

## 7. Assumptions and Dependencies
- The database already contains planet records.
- Planet names are unique in the database.
- The API has access to a working database connection.
- The frontend application will call this API and display the returned JSON data.
- Temperature values are already stored in the database and do not need additional calculation.

## 8. Summary
The Planet Info API is a simple read-only API that allows an application to retrieve planet data from a database. It supports getting a full list of planets and retrieving a single planet by name. The API is designed to be fast, return consistent JSON responses, and handle basic errors clearly.
