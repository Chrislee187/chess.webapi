﻿# Overview
A simple RESTlike web-api wrapping the core functionality of the [chess.engine][link.ChessEngine].

Swagger has been implemented using [NSwag][link.NSwag], a live example should be available via it's [Swagger UI][link.swaggerui] or it's simple templated "player" page a [templated "player"][link.chess.webapi.index.live]

A simple wrapper around the webapi calls can be seen at [link.rest-wrapper.live][link.rest-wrapper.live] which uses [handlebars][link.handlebars] to render the JSON api results in to the simplest of boards and interactions for a game chess.

# Introduction
Uses standard .NET Core/AspNET/MVC approaches to expose a very simple web-api for interaction with the [chess.engine][link.ChessEngine]

# Notes/Comments/Observations
There are currently no tests around the Controller or the Service. They are pretty much acting a proxies directly to the [chess.engine][link.ChessEngine] with not logic of there own. This may of course change in the future if I add more complex API's.

# TODOS
* BUG - Highlighted in the react app
```
https://localhost:5001/api/chessgame/rnbqkbnrppppppp........p...E....................PPP.PPPPRNBQKBNRB0000/e7e5
returns a valid enpassant take at d5d6
```

however requesting the move list for the board result in the previous requests i.e.
```
https://localhost:5001/api/chessgame/rnbqkbnrpppp.pp........p...Pe...................PPP.PPPPRNBQKBNRW0000
```

returns a different list of available moves


* Flesh out this doc more
* Some basic integration tests on the API (basic requests work, invalid request return expected codes)





[link.BoardEngine]: ../board.engine
[link.ChessEngine]: ../chess.engine
[link.swaggerui]: https://chess-web-api.azurewebsites.net/swagger/index.html
[link.chess.webapi.index.live]: https://chess-web-api.azurewebsites.net/index.html
