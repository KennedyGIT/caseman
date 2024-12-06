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
COPY caseman.organisation.api/caseman.organisation.api  caseman.organisation.api/caseman.organisation.api
COPY caseman.organisation.api/core caseman.organisation.api/core
COPY caseman.organisation.api/infrastructure caseman.organisation.api/infrastructure
COPY caseman.organisation.api/OrganisationControllerTests caseman.organisation.api/OrganisationControllerTests

# Set the working directory to /api/caseman.user.api/caseman.user.api
WORKDIR /api/caseman.organisation.api/caseman.organisation.api

# Build the application in Release mode and output to /api/caseman.user.api/caseman.user.api/out
RUN dotnet publish -c Release -o /api/caseman.organisation.api/caseman.organisation.api/out

# Switch to a different base image for the runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set the working directory to /api
WORKDIR /api

# Copy the published output from the build stage to the runtime image
COPY --from=build /api/caseman.organisation.api/caseman.organisation.api/out .

# Specify the entry point for the container (the main application)
ENTRYPOINT ["dotnet", "caseman.organisation.api.dll"]

# Command to build  this docker file will be ->  docker build -f caseman.role.api/caseman.role.api/Dockerfile -t [NameoftheImageInLowerCase] .
# Make sure you run the command from the root folder i.e where the solution file is or where the dockercompose file will be which is usually 
# where the project solution file is 