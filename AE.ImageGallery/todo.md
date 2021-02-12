todo
----

- [] investigate task  
    - [x] check api contracts  
    - [+/-] check restrictions  
        - [x] No redundant REST API calls should be triggered by the app.  
        - [x] implement invalid token handler and renewal
        - [x] The app should load and cache photos from our API endpoint http://interview.agileengine.com  
        - [+/-] The app should fetch the entire load of images information upon initialization and perform cache reload once in a defined (configurable) period of time.  
        - [x] The app should provide a new endpoint: GET /search/${searchTerm}, that will return all the photos with any of the meta fields (author, camera, tags, etc) matching the search term. The info should be fetched from the local cache, not the external API.
    
- [] make some plan  
    - [x] add base structure  
    - [x] choose cqrs approach
    - [x] add fetch logic on start  
    - [x] add save logic  
    - [x] add public api contract  
    - [x] add public api controllers  
    - [x] add public fetch logic  
    - [x] add docker + docker-compose  
    - [] add Dockerfile  
    - [x] add swagger
    - [x] add error handling
    - [x] add logging
    - [+/-] add unit tests
    - [x] Add retry auth policy  
    - [] Add retry fail request policy  
    - [x] Support graceful shutdown  
