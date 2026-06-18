# Multi-stage Dockerfile for .NET 10 Blazor Server app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copy csproj and restore
COPY ConnectFour.csproj ./
RUN dotnet restore ConnectFour.csproj

# copy everything else and publish
COPY . ./
RUN dotnet publish ConnectFour.csproj -c Release -o /app/publish

# runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

# ensure App_Data exists and is writable for SQLite DB
RUN mkdir -p /app/App_Data && chmod 777 /app/App_Data || true

# Use environment variable for ASPNETCORE_URLS
ENV ASPNETCORE_URLS="http://+:80"
EXPOSE 80

ENTRYPOINT ["dotnet", "ConnectFour.dll"]
