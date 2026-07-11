# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files first for layer caching
# Since context is /src, we copy from cli/marksapi/*.csproj
COPY ./cli/marksapi/*.csproj ./cli/marksapi/

# Copy all project files (handles nested .csproj files)
COPY ./cli ./cli
COPY ./lib ./lib

# Set working directory to the project root for publishing
WORKDIR /src/cli/marksapi
# Restore dependencies
RUN dotnet restore
# Publish the application
RUN dotnet publish -c Release -o /app/publish --no-build

# Stage 2: Final Runtime Image
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app
ENV PATH="$PATH:/app"

# Copy the published output from the build stage
COPY --from=build /app/publish .

# No ENTRYPOINT defined; container runs interactively
# Example usage: docker run -it <image-name> bash
# Within container:
# ```bash
# $ marksapi --version
# ```