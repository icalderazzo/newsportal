# News Portal
## Introduction
This is a small project that showcases the integration of a Web API and Single Page Application (SPA) to provide users with a seamless experience accessing the newest stories. The stories presented in the feed are retrieved from the Hacker News API: https://github.com/HackerNews/API

The development of this project revolves around two core technologies:
* Back-end: .NET 6 with C#11
* Front-end: Angular 16.0.1

The primary objective of this endeavor is to demonstrate my development skills in both frameworks, incorporating essential modern application features, such as:

* A paging mechanism
* A search feature
* Server Caching of the newest stories

## Assumptions
* Stories shown in the feed are retrieved from the **/v0/newstories** endpoint
* The stories that do not have a hyperlink will not have a button to redirect to original source, but they will show a title and a text if they have
* Some AskHN posts come with the newstories endpoint, they will not be filtered.

## Solution structure
The solution consits in two separate projects: backend & frontend. In this section there will be an explanation of the structure of both code projects.

### Backend
The API consits in three layers: 
* ```WebApi```: The enty point of the application. This layer defines endpoints, routing, dependencies, error handling, etc. It depends on the Application Layer.

* ```Application```: This layer houses all the application logic, including use cases, features, and mapping. It is organized into modules, with the current implementation featuring the ItemsModule, which includes the Stories sub-module. Additionally, this layer includes background services and other relevant components that will be explained later. It relies on the Infrastructure Layer.
    
* ```Infrastructure```: Also referred to as the data access layer, the Infrastructure layer focuses on data retrieval. In the case of this solution, the only data access involves the Hacker News API. As a result, this layer incorporates an HttpClient specifically designed for interacting with the API.

The solution includes a ```Contracts``` project that defines the API contract, specifying the response structure and Data Transfer Objects (DTOs).
Within the Contracts project, the API contract outlines the expected response format, enabling API consumers to understand the data they will receive. The WebApi project does not include the API contract definition because it is required by the Application Layer for mapping purposes. Additionally, if a .NET HttpClient needs to be created for this API, it only needs to rely on the Contracts project.

**Dependencies:** RestSharp/110.2.0, Newtonsoft/13.0.3, AutoMapper/12.0.1


### Frontend
The frontend project is a basic Angular application using the template provided by the ```ng new``` command. The structure inside the app folder is the following:

* ```Core```: This folder houses all the interfaces and services responsible for communication with external APIs.
* ```Stories```: This is the stories module which contains all the components of the stories feed: storycard, table, paginator, searchbar. Each module will have its own separate folder.
* ```AppModule```: This module represents the heart of the application, handling the feed functionality.

In addition to the default template, an extra folder named environments has been added. It serves to define environment configurations, including API routes, keys, and other relevant settings. Besides, the angular.json has been modified to replace the environment files according to selected configuration to start the app. 

**Dependencies:** Angular Material/16.0.1
