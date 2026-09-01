# Stage 1: Build the Angular frontend
FROM node:20-alpine AS frontend-build
WORKDIR /src/frontend
COPY frontend/package*.json ./
RUN npm ci
COPY frontend/ ./
RUN npm run build -- --configuration production

# Stage 2: Build the .NET backend (uses the Angular output in wwwroot/browser)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS backend-build
WORKDIR /src/backend
COPY backend/ ./
# Bring in the Angular build output (written to ../backend/.../wwwroot from the frontend workdir)
COPY --from=frontend-build /src/backend/BrewInventory.App/wwwroot/ BrewInventory.App/wwwroot/
RUN dotnet restore BrewInventory.App/BrewInventory.App.csproj
RUN dotnet publish BrewInventory.App/BrewInventory.App.csproj -c Release -o /app/publish --no-restore

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
# SQLite database lives here; mount a volume at /app for persistence
COPY --from=backend-build /app/publish ./
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
ENTRYPOINT ["dotnet", "BrewInventory.App.dll"]
