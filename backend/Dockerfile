FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY AirlineSystem.API/*.csproj ./AirlineSystem.API/
RUN dotnet restore ./AirlineSystem.API/AirlineSystem.API.csproj
COPY AirlineSystem.API/. ./AirlineSystem.API/
WORKDIR /src/AirlineSystem.API
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "AirlineSystem.API.dll"]
