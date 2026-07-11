# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file(s) first for layer caching
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the source code
COPY . ./

# Publish the application
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Final Runtime Image
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/publish .

# No ENTRYPOINT defined; container runs interactively
# Example usage: docker run -it <image-name> /app/marksapi <ARGS>