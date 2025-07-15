# -------------------------------------------------------------------------------------------------
# Dockerfile -- for card.validator.api.v1 (.NET 8 Web API using port 5105)
# -------------------------------------------------------------------------------------------------

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.sln .
COPY card.validator.api.v1/*.csproj ./card.validator.api.v1/
RUN dotnet restore

# Copy the full source and publish
COPY card.validator.api.v1/. ./card.validator.api.v1/
WORKDIR /app/card.validator.api.v1
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Set app to run on port 5105
ENV ASPNETCORE_URLS=http://+:5105
EXPOSE 5105

ENTRYPOINT ["dotnet", "card.validator.api.v1.dll"]
