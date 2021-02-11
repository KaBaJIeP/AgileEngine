todo
----

- [] investigate task  
    - [x] check api contracts  
    - [x] check restrictions  
        - [x] No redundant REST API calls should be triggered by the app.  
        - [] The app should fetch the entire load of images information upon initialization and perform cache reload once in a defined (configurable) period of time.  
        - [] The app should provide a new endpoint: GET /search/${searchTerm}, that will return all the photos with any of the meta fields (author, camera, tags, etc) matching the search term. The info should be fetched from the local cache, not the external API.
    
- [] make some plan  
    - [] add base structure  
    - [] choose cqrs approach
    - [] add fetch logic on start  
      - [] add basic approach but extensible  
    - [] add save logic  
    - [] add public api contract  
    - [] add public api controllers  
    - [] add public fetch logic  
    - [] add docker + docker-compose
    - [] add swagger
    - [] add error handling
    - [] add logging
    - [] add unit tests
