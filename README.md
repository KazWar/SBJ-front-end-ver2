# Version 2 of the front-end

This version is an improvement on the https://github.com/KazWar/SBJ-front-end-ver1. It's still currently a WIP, but:
- It doesn't use a hybrid data model (As I also redesigned the API endpoints)
- There's a shared controller for all websites (Previously each website had a copy of an identical controller, which caused a lot of API issues and was painful to maintain as you had to propagate changes to all the websites)
- Will use https://formkit.com instead of https://github.com/CyCraft/blitzar for dynamic form generation. At the time I made the choice, FormKit wasn't available yet, but i'm choosing it right now as the library writers made a very popular Vue 2 form library and are actively working on the project as opposed to Blitzar. It also features more powerful form generation options which I was looking for.
