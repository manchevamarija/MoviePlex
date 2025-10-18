<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 4e4bede (Initial clean commit of MoviePlex project)
# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app

# --- Runtime Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "MoviesApp.dll"]




<<<<<<< HEAD
=======
=======
﻿# 🧱 Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копирај solution и project фајлови и restore
COPY *.sln .
COPY MoviesApp/*.csproj MoviesApp/
RUN dotnet restore

# Копирај го остатокот од кодот и изгради
COPY . .
WORKDIR /src/MoviesApp
RUN dotnet publish -c Release -o /app/publish

# 🚀 Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MoviesApp.dll"]
>>>>>>> 285fcfb (Removed nested repo and committed clean structure)
>>>>>>> 4e4bede (Initial clean commit of MoviePlex project)
