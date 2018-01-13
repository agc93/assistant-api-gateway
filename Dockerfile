FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/AssistantAPI.Gateway/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY src/AssistantAPI.Gateway .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "AssistantAPI.Gateway.dll"]