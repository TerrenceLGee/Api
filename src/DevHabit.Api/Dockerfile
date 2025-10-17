# Base stage for the final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

# Copy the solution file and the props files from the root context
# This is crucial for dotnet restore to work correctly
COPY ["DevHabit.sln", "Directory.Build.props", "Directory.Packages.props", "./"]

# Copy the API project source code
COPY ["src/DevHabit.Api/", "./src/DevHabit.Api/"]

# Run restore on the entire solution from the context of the project folder
WORKDIR "/build/src/DevHabit.Api"
RUN dotnet restore "DevHabit.Api.csproj"

# Now, build the project
RUN dotnet build "DevHabit.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DevHabit.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --chown=app:app aspnetapp.pfx .
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevHabit.Api.dll"]