# Use the official .NET SDK 8.0 image as the base image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

# Set the working directory inside the container to /api
WORKDIR /api

# Expose port 80 (this is a hint to anyone running the container)
EXPOSE 80

# Copy the solution file and project files to the container
COPY caseman.sln caseman.sln

# USER API
COPY caseman.user.api/caseman.user.api/caseman.user.api/caseman.user.api.csproj caseman.user.api/caseman.user.api/caseman.user.api/caseman.user.api.csproj
COPY caseman.user.api/caseman.user.api/core/core.csproj caseman.user.api/caseman.user.api/core/core.csproj
COPY caseman.user.api/caseman.user.api/infrastructure/infrastructure.csproj caseman.user.api/caseman.user.api/infrastructure/infrastructure.csproj

# ROLE API  
COPY caseman.role.api/caseman.role.api/caseman.role.api.csproj caseman.role.api/caseman.role.api/caseman.role.api.csproj
COPY caseman.role.api/core/core.csproj caseman.role.api/core/core.csproj
COPY caseman.role.api/infrastructure/infrastructure.csproj caseman.role.api/infrastructure/infrastructure.csproj
COPY caseman.role.api/RoleControllerTests/RoleControllerTests.csproj caseman.role.api/RoleControllerTests/RoleControllerTests.csproj

# ORGANISATION API 
COPY caseman.organisation.api/caseman.organisation.api/caseman.organisation.api.csproj caseman.organisation.api/caseman.organisation.api/caseman.organisation.api.csproj
COPY caseman.organisation.api/infrastructure/infrastructure.csproj caseman.organisation.api/infrastructure/infrastructure.csproj 
COPY caseman.organisation.api/core/core.csproj caseman.organisation.api/core/core.csproj
COPY caseman.organisation.api/OrganisationControllerTests/OrganisationControllerTests.csproj caseman.organisation.api/OrganisationControllerTests/OrganisationControllerTests.csproj

# GATEWAY API
COPY caseman.gateway.api/caseman.gateway.api/caseman.gateway.api.csproj caseman.gateway.api/caseman.gateway.api/caseman.gateway.api.csproj

# Restore package dependencies using the .NET CLI
RUN dotnet restore caseman.sln

# Copy the application folders from the host to the container
COPY caseman.user.api/caseman.user.api/caseman.user.api   caseman.user.api/caseman.user.api/caseman.user.api
COPY caseman.user.api/caseman.user.api/caseman.user.api/Content /api/caseman.user.api/caseman.user.api/caseman.user.api/Content
COPY caseman.user.api/caseman.user.api/core caseman.user.api/caseman.user.api/core
COPY caseman.user.api/caseman.user.api/infrastructure caseman.user.api/caseman.user.api/infrastructure

# Set the working directory to /api/caseman.user.api/caseman.user.api
WORKDIR /api/caseman.user.api/caseman.user.api/caseman.user.api

# Build the application in Release mode and output to /api/caseman.user.api/caseman.user.api/out
RUN dotnet publish -c Release -o /api/caseman.user.api/caseman.user.api/caseman.user.api/out

# Switch to a different base image for the runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set the working directory to /api
WORKDIR /api

# Copy the published output from the build stage to the runtime image
COPY --from=build /api/caseman.user.api/caseman.user.api/caseman.user.api/out .

# Copy the Content directory to the runtime image
COPY --from=build /api/caseman.user.api/caseman.user.api/caseman.user.api/Content /api/Content

# Specify the entry point for the container (the main application)
ENTRYPOINT ["dotnet", "caseman.user.api.dll"]

# Command to build this Dockerfile
# docker build -f caseman.user.api/caseman.user.api/Dockerfile -t [NameoftheImageInLowerCase] .