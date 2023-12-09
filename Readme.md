# Remote JWT Authentication Project

This project demonstrates a simple implementation of remote JWT (JSON Web Token) authentication in .NET. It consists of two separate applications: a Login Server responsible for generating JWT tokens and another application capable of validating these tokens.

## Getting Started

After running `Identify.Api` it will navigate to swagger page. You can create a user to get the token or login to get the token. It uses Sqlite to store the data. After getting the token run the `Weather.Api`, at swagger page, you need to insert the token to use the api.

It is a proof of concept project to learn about JWT. 