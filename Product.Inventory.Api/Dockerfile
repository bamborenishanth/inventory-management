##See https://aka.ms/containerfastmode to understand how Visual Studio uses this  Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ARG bin=out
WORKDIR /app
EXPOSE 5000
COPY ${bin} .
ENTRYPOINT ["dotnet", "Product.Inventory.Api.dll"]
