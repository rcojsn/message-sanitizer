# Bleep Guard

![build workflow](https://github.com/rcojsn/flash/actions/workflows/build.yml/badge.svg)
![unit testing workflow](https://github.com/rcojsn/flash/actions/workflows/test.yml/badge.svg)

## Description

This project implements two microservices; one for managing sensitive words (used within an internal environment) and
another for sanitizing strings (messages sent from another system / microservice) by replacing or removing any sensitive
words. The **AdminService API (Sensitive Words API)** allows adding, removing, and listing sensitive words, while the
**Censorship API** receives a string input and returns a sanitized version of the string

Both services follow the **Clean Architecture** pattern, ensuring separation of concerns and maintainable code 

## Technologies Used

- **C#** (for building the API)
- **.NET 8.0** (Framework)
- **Microsoft SQL Server** (for storing sensitive words)
- **Docker** (for containerization)
- **Redis** (for caching sensitive words)
- **Docker Compose** (for managing the applications, database and cache layer within a containerized environment)

---

## Checklist

### Restful API
  - [x] CRUD endpoints for managing sensitive words (***AdminService***)
  - [x] Endpoint for sanitizing strings (***CensorshipService***)
  - [x] API Documentation (Swagger)
  - [x] Deployment strategy

#### Extras
  - Redis Cache
  - ErrorOr<> package for error handling
  - Built-in logging package
  - CI/CD pipeline (GitHub Actions)
  - Dockerized applications (Docker Compose)
    - AdminService
    - CensorshipService
    - MSSQL
    - Redis
    - Redis Commander (for managing the cache)
    - Database seeding (using a container)
    - Censorship worker (gets all sensitive words through admin service and stores them in redis)
  - Diagrams for architectural overview can be found within this file
---

### What I would do to enhance performance?

- **Caching**: Implement caching to reduce database round trips
  - **Redis**: (***Already added***)
    - Instead of querying the database for sensitive words on every request (through the admin service), 
    I implemented a censorship worker that on startup, fetches all sensitive words, and stores them in a cache (Redis)
    - The censorship service then queries the cache for sensitive words, instead of the database
    - In order to keep the cache in sync with the database, redis gets updated from the admin service whenever a 
    sensitive word is added, removed or updated in the database
    - Redis is being used alongside MSSQL because MSSQL is used for persisting the sensitive words, while Redis is used
    for caching the sensitive words and providing faster lookup times
  - ***Message Queue & Local Cache***:
    - To enhance the cache layer further, I would use a pub/sub model to update the local cache (MemoryCache) whenever a
    sensitive word is added, removed or updated in the database, the censorship service would subscribe to these events 
    as a ***consumer*** and the admin service would publish these events as a ***producer*** removing the need to query redis
    

### What additional enhancements would I add to make the project more complete?

- ***Message Queue***:
    - ***RabbitMQ***: To prevent any messages from being ignored and not sanitized, I would implement a message queue
      to ensure that ***ALL*** messages get sanitized, even if the censorship service is down
        - I would also add this to the ***Admin Service*** to ensure that any management on sensitive words is not lost
          and added to the queue for processing
- ***An AI Model***:
    - I would store all the raw messages(before sanitizations) in a table BUT as a base64 to prevent any ***sql injection***
    and use this data to train an AI model to detect sensitive words that may not be in the database and compile a list of 
    words that it thinks could be sensitive, this list can then be reviewed by an admin and added to the database if necessary
      - This allows the system to be more proactive in detecting sensitive words and not just rely on manual input from the admin
      - This would also allow the system to be more dynamic and adapt to new sensitive words that may be introduced in the future
    - The model can also detect sensitive words in different languages [***This would link into the current functionality***]
- ***Rate Limiting***:
    - I would implement rate limiting to prevent any abuse of the API, this would prevent any user from sending too many
    requests in a short period of time
- ***Paywall***:
    - I would implement a paywall on the censorship api to prevent abusing the service, this would allow the service to be
    monetized and generate revenue. A free tier would be available for a limited number of requests per day, and a premium
    tier would be available for unlimited requests (this would be managed through a subscription model)
    - This would also assist in paying for infrastructure costs
- ***Admin Dashboard***:
    - I would implement an angular / react dashboard to manage the sensitive words instead of through the endpoints on 
    swagger
- ***Real-time sanitization***:
    - Sanitize messages in real-time as they are being typed
- ***Message History***:
    - Store all messages (raw and sanitized) in a table (ensuring that the raw messages are bas64 to prevent sql injection)
---

## Architecture Overview

### System Context

### Container Context [Internal]

<img src="./assets/architectural-designs/AdminContainerDiagram.png" alt="Admin Container Context">

### Container Context [External]

<img src="./assets/architectural-designs/ContainerDiagram.png" alt="Container Context">

---

## Deployment Strategy

- The project (both services) will be containerized using Docker (for development)
- Once work is completed, the codebase will run through GitHub Actions
- A CI/CD pipeline (GitHub Actions) will be used to build (applications) and test (unit tests)
  the project
- A merge request will be created per feature branch
- Once the merge request is approved, the feature branch will be merged into the develop branch
- The develop branch will be deployed to a staging environment (for testing)
- Once testing is completed, the develop branch will be merged into a release branch (for production) and backtrack into
the main branch
- The pipeline will then deploy the release branch to a production environment (for live use)
  - The production environment will be hosted on AWS
    - Since the project is containerized, it can be deployed to AWS ECS
      - The admin service will be behind a private subnet and only accessible through a VPN for internal use,
      where all sensitive words can be managed
      - A shared key with a 3 month rotation policy will be used to access the admin service from the censorship service
- The project will be monitored using AWS CloudWatch
  - Metrics will be set up to monitor the health of the services
  - Alarms will be set up to notify the team of any issues
  - Logs will be stored in AWS S3
    (Grafana can be setup to provide a more detailed view of the metrics)
- All secrets (database connection strings and redis connection strings) will be stored in AWS Secrets Manager

---

## Prerequisites

Before getting started, ensure that you have the following installed:

- **Docker** and **Docker Compose** (for containerization)
- **.NET SDK** (to build the project)
- **Git** (for version control)

---

## Local Development

> **NOTE:**
> Seems to work seamlessly within rider (***docker wsl 2***), issues with pathing through the terminal

### 1. Clone the Repository

```bash
git clone https://github.com/rcojsn/message-sanitizer.git
cd message-sanitizer
```

### 2.a Run the projects using Docker Compose

```bash
docker-compose up -d
```