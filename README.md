# FeudaAPI
 This is the backend for my project named Feuda, it is a real-time turn based strategy game.

The main goal of this project is to create an online game with the capability for up to 4 people to play together in real time, have interactions with each other that affect the outcome of the games and the experience of the players.

The backend of the aplication is writen using in ASP.NET Core, and the real time client communication is achieved by implementing ASP.NET Cores Comet technology, SignalR into the project both in the backend and the frontend. This is one of the main building stones of the project, since besides the client to client communication, the server does all of the calculations and data handling to ensure that the game runs according to the server side settings and there is no desync between players.

Because of its API like structure, it is possible to create any kind of frontend app to communicate with the server and be able to play games, as long as it implements SignalR.
