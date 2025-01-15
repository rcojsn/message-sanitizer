# BleepGuard

## Description

This project implements two microservices; one for managing sensitive words (used within an internal environment) and
another for sanitizing strings (messages sent from another system / microservice) by replacing or removing any sensitive
words. The **AdminService API (Sensitive Words API)** allows adding, removing, and listing sensitive words, while the
**Censorship API** receives a string input and returns a sanitized version of the string

Both services follow the **Clean Architecture** pattern, ensuring separation of concerns and maintainable code 

### Key Features:
- **Sensitive Word Detection**: Identifies and replaces sensitive words in the provided message
- **Caching**: Uses **Redis** to cache sensitive words and prevent database round trips

---

## Technologies Used

- **C#** (for building the API)
- **.NET 8.0** (Framework)
- **SQL** (for storing sensitive words)
- **Docker** (for containerization)
- **Redis** (for caching sensitive words)
- **Docker Compose** (for managing the applications, database and cache layer within a containerized environment)

---

## Architecture Overview

![System Context Diagram](./assets/architectural-designs/SystemContext.drawio.png)

---

## Prerequisites

Before getting started, ensure that you have the following installed:

- **Docker** and **Docker Compose** (for containerization)
- **.NET SDK** (to build the project)
- **Git** (for version control)

---

## Local Development

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/sensitive-words-microservice.git
cd sensitive-words-microservice
