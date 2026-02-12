# News Portal
## Introduction
This is a small project that showcases the integration of a Web API and Single Page Application (SPA) to provide users with a seamless experience accessing the newest stories. The stories presented in the feed are retrieved from the Hacker News API: https://github.com/HackerNews/API

The development of this project revolves around the following technologies:
* Back-end: .NET 10 with C#15
* Front-end: Angular 21.0
* SQL Server 2022
* Docker

The primary objective of this endeavor is to demonstrate my development skills in both frameworks, incorporating essential modern application features, such as:

* A paging mechanism
* A search feature
* Server-side caching of the newest stories

## Assumptions
* Stories shown in the feed are derived from the `/v0/newstories` endpoint (which returns IDs for the 500 newest items).
* Stories without a URL do not show the “open source” button. The title (and text, if present) is still displayed.
* Some Ask HN posts appear in `/v0/newstories`; they are intentionally not filtered out.

## Solution structure
The solution consists of two separate projects: `backend` and `frontend`. This section explains the structure of both.
There is also a SQL Server instance used by the API to support features like Bookmarks (with a seeded/mock user).

### Backend
The API consists of four layers:
* `WebApi`: The entry point of the application. This layer defines endpoints, routing, dependencies, error handling, etc. It depends on the Application Layer.

* `Application`: This layer houses all the application logic, including use cases, features, and mapping. It is organized into modules, with the current implementation featuring the ItemsModule, which includes the Stories sub-module. Additionally, this layer includes background services and other relevant components that will be explained later. It relies on the Infrastructure Layer.
    
* `Infrastructure`: Also referred to as the data access layer, the Infrastructure layer focuses on data retrieval. In the case of this solution, the data access involves the Hacker News API and the SQL Server database. As a result, this layer incorporates an HttpClient specifically designed for interacting with the HackerNews API + Repositories for database communication.

* `Core` (Domain): Based on Domain-Driven Design (DDD) patterns, this layer defines core models and repository interfaces for working with data sources. All other layers depend on it.

The solution includes a `Contracts` project that defines the API contract, specifying the response structure and Data Transfer Objects (DTOs).
Within the Contracts project, the API contract outlines the expected response format, enabling API consumers to understand the data they will receive. The WebApi project does not include the API contract definition because it is required by the Application Layer for mapping purposes. Additionally, if a .NET HttpClient needs to be created for this API, it only needs to rely on the Contracts project.

**Dependencies:** RestSharp/113.1.0, Newtonsoft.Json/13.0.4, Riok.Mapperly/4.3.0, EF Core/10.0.3


### Frontend
The frontend project is a basic Angular application using the template provided by the `ng new` command. The structure inside the app folder is the following:

* `Core`: This folder houses all the interfaces and services responsible for communication with external APIs.
* `Stories`: This is the stories module which contains all the components of the stories feed: storycard, table, paginator, searchbar. Each module will have its own separate folder.
* `AppModule`: This module represents the heart of the application, handling the feed functionality.

In addition to the default template, an extra folder named environments has been added. It serves to define environment configurations, including API routes, keys, and other relevant settings. Besides, the angular.json has been modified to replace the environment files according to selected configuration to start the app. 

**Dependencies:** Angular Material/21.0

## Server-side caching
The solution leverages memory caching through the `IMemoryCache` interface offered by the .NET Core Framework. It employs a caching strategy where the 500 most recent stories are stored in memory for a duration of 10 minutes.

When a request is made to fetch the latest stories, the solution first retrieves the list of IDs from Hacker News and then:
1) serves the story from cache (if present), or
2) calls `/v0/item/{id}` to fetch it and store it in the cache.

### Implementation
**ItemsCacheService**: The application service that takes charge of cache management. It utilizes generics to support various types of items, such as Stories, Polls, Comments, and more. While currently focusing on storing Stories, the class is designed with future extensibility in mind, allowing for the potential inclusion of additional item types in the cache.

**StoriesService**: This is the only `ItemService` implementation required for this solution. Any `ItemService` relies on the `ItemsCacheService` and the `HackerNewsClient`. This class has two methods: `GetNewestStories` and `Search`. Both methods interact with the `ItemsCacheService` to retrieve existing stories from the cache.
In the case of the `GetNewestStories` method, the service also defines a Delegate, which enables the ItemsCacheService to create a new Story object if it is not present in the cache. By doing so, the class adheres to the Single Responsibility Principle (SRP), ensuring a clear separation of concerns.

<hr>

Let's move on to discussing the **Background services** within the solution. These services have the responsibility of loading the cache during application startup and updating stories. They implement the `IHostedService` interface, which defines two methods: StartAsync and StopAsync.

StartAsync: Contains the logic to start a background task and it is executed on application startup.

StopAsync: Contains the logic to end a background task.

By implementing the IHostedService interface and utilizing these two methods, the Background services ensure the seamless initialization and termination of background tasks, contributing to the overall functionality and reliability of the solution.

**NewestStoriesBackgroundService**: This service utilizes `IServiceProvider` to create a new scope and retrieve an instance of `StoriesService`. It then calls `GetNewestStories` so the cache is warmed during application startup. To ensure the service performs its work only once, it includes a flag indicating whether the task has completed.

**UpdateStoriesBackgroundService**: Similar to the previous service, it utilizes the `IServiceProvider` and creates a new scope to obtain an instance of the StoriesService. In this case, the service invokes a method from the base class, ItemService, called `UpdateItems`. Inside this method, the service makes a request to the Hacker News API endpoint **/v0/updates** to retrieve the latest updates. Then, it then interacts with the ItemsCacheService to remove outdated instances of stories and create new ones based on the updated data.

## Additional features
### Bookmarks
The Bookmarks feature allows the user to bookmark stories and view them on a separate page. Bookmarks preserve the original story URL so the user can visit the source at any time.

## Run the application locally
### Prerequisites
* .NET 10 SDK
* Node.js 21 or higher
* Angular CLI 21
* Docker

### Step-by-step
1. Docker engine must be running
2. In the backend folder, open the `NewsPortal.Backend.sln` with your IDE of preference.
3. In the backend folder, run `make init`. This will initialize a SQL Server 2022 instance in Docker. Two containers will be started; wait for the one called `DatabaseSetup` to exit.
4. Make sure `NewsPortal.Backend.WebApi` is selected as the startup project and you have selected the `NewsPortal.Backend.WebApi` launch profile.
5. Start the API. It should run at **https://localhost:7105** and open Swagger UI (you may need to trust local .NET dev certificates on your system).
6. Open a terminal and go to the `frontend` folder.
7. Install dependencies with `npm i`.
8. Start the SPA with `ng serve -o`.
9. The Angular app should run at **http://localhost:4200** (and open a browser window automatically).
