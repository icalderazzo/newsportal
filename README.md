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
* The ones that do not have a hyperlink will not have a button to redirect to original source, but they will show a title and a text if they have
