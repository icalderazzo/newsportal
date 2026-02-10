# News Portal
## Introduction
This is a small project that showcases the integration of a Web API and Single Page Application (SPA) to provide users with a seamless experience accessing the newest stories. The stories presented in the feed are retrieved from the Hacker News API: https://github.com/HackerNews/API

The development of this project revolves around two core technologies:
* Back-end: .NET 10 with C#15
* Front-end: Angular 21.0

The primary objective of this endeavor is to demonstrate my development skills in both frameworks, incorporating essential modern application features, such as:

* A paging mechanism
* A search feature
* Server Caching of the newest stories

## Assumptions
* Stories shown in the feed are retrieved from the **/v0/newstories** endpoint (500 newest stories)
* The stories that do not have a hyperlink will not have a button to redirect to original source, but they will show a title and a text if they have
* Some AskHN posts come with the newstories endpoint, they will not be filtered.

## Solution structure
The solution consits in two separate projects: backend & frontend. In this section there will be an explanation of the structure of both code projects.

### Backend
The API consits in three layers: 
* `WebApi`: The enty point of the application. This layer defines endpoints, routing, dependencies, error handling, etc. It depends on the Application Layer.

* `Application`: This layer houses all the application logic, including use cases, features, and mapping. It is organized into modules, with the current implementation featuring the ItemsModule, which includes the Stories sub-module. Additionally, this layer includes background services and other relevant components that will be explained later. It relies on the Infrastructure Layer.
    
* `Infrastructure`: Also referred to as the data access layer, the Infrastructure layer focuses on data retrieval. In the case of this solution, the only data access involves the Hacker News API. As a result, this layer incorporates an HttpClient specifically designed for interacting with the API.

The solution includes a `Contracts` project that defines the API contract, specifying the response structure and Data Transfer Objects (DTOs).
Within the Contracts project, the API contract outlines the expected response format, enabling API consumers to understand the data they will receive. The WebApi project does not include the API contract definition because it is required by the Application Layer for mapping purposes. Additionally, if a .NET HttpClient needs to be created for this API, it only needs to rely on the Contracts project.

**Dependencies:** RestSharp/113.1.0, Newtonsoft/13.0.4, Riok.Mapperly/4.3.0


### Frontend
The frontend project is a basic Angular application using the template provided by the `ng new` command. The structure inside the app folder is the following:

* `Core`: This folder houses all the interfaces and services responsible for communication with external APIs.
* `Stories`: This is the stories module which contains all the components of the stories feed: storycard, table, paginator, searchbar. Each module will have its own separate folder.
* `AppModule`: This module represents the heart of the application, handling the feed functionality.

In addition to the default template, an extra folder named environments has been added. It serves to define environment configurations, including API routes, keys, and other relevant settings. Besides, the angular.json has been modified to replace the environment files according to selected configuration to start the app. 

**Dependencies:** Angular Material/21.0

## Server-side caching
The solution leverages memory caching through the `IMemoryCache` interface offered by the .NET Core Framework. It employs a caching strategy where the 500 most recent stories are stored in memory for a duration of 10 minutes.

When a new request is made to fetch the latest stories from the API, the solution interacts with the Hacker News API to retrieve the IDs of the 500 newest stories. It then proceeds to either fetch the corresponding items from the cache or call the **/v0/item/{id}** endpoint of the Hacker News API to create and store the respective "new" objects in the cache.

### Implementation
**ItemsCacheService**: The application service that takes charge of cache management. It utilizes generics to support various types of items, such as Stories, Polls, Comments, and more. While currently focusing on storing Stories, the class is designed with future extensibility in mind, allowing for the potential inclusion of additional item types in the cache.

**StoriesService**: This is the only type of ItemService that required implementation for this solution. Any ItemService will have to relay on the ItemsCacheService and the HackerNewsClient. This class has two methods: `GetNewestStories` and `Search`. Both methods interact with the ItemsCacheService by sending a message to retrieve existing Stories from the cache.
In the case of the `GetNewestStories` method, the service also defines a Delegate, which enables the ItemsCacheService to create a new Story object if it is not present in the cache. By doing so, the class adheres to the Single Responsibility Principle (SRP), ensuring a clear separation of concerns.

<hr>

Let's move on to discussing the **Background services** within the solution. These services have the responsibility of loading the cache during application startup and updating stories. They implement the `IHostedService` interface, which defines two methods: StartAsync and StopAsync.

StartAsync: Contains the logic to start a background task and it's executed on application starup.

StopAsync: Contains the logic to end a background task.

By implementing the IHostedService interface and utilizing these two methods, the Background services ensure the seamless initialization and termination of background tasks, contributing to the overall functionality and reliability of the solution.

**NewestStoriesBackgroundService**: This service utilizes the `IServiceProvider` to create a new scope and retrieve an instance of the StoriesService. It then calls the `GetNewestStories` method from the StoriesServic to the cache is loaded during the application startup. To ensure that the service performs its work only once, it includes a flag that indicates whether the task has been completed or not. This allows the service to avoid redundant processing if it has already fulfilled its purpose.

**UpdateStoriesBackgroundService**: Similar to the previous service, it utilizes the `IServiceProvider` and creates a new scope to obtain an instance of the StoriesService. In this case, the service invokes a method from the base class, ItemService, called `UpdateItems`. Inside this method, the service makes a request to the Hacker News API endpoint **/v0/updates** to retrieve the latest updates. Then, it then interacts with the ItemsCacheService to remove outdated instances of stories and create new ones based on the updated data.

## Run the application locally
1. In the backend folder, open the `NewsPortal.Backend.sln` with your IDE of preference.
2. Make sure the `NewsPortal.Backend.WebApi` is selected as startup project and you have selcted `NewsPortal.Backend.WebApi` profile before running the application.
3. The application should start running in **https:localhost:7105** and you will be prompted with a new browser window in a Swagger UI - you may need to trust local dotnet certs on your system.
4. Open a Terminal and go the ../frontend folder.
5. Execure the following command to install dependencies `npm i`.
6. Excecute the following command `ng serve -o`.
7. The Angular app should start running in **https:localhost:4200**. A new browser window should pop-up with the web running.
