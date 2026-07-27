# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files first for layer caching
# Since context is /src, we copy from cli/*.csproj
COPY ./cli/*.csproj ./cli/

# Copy all project files (handles nested .csproj files)
COPY ./cli ./cli
COPY ./lib ./lib

# Set working directory to the project root for publishing
WORKDIR /src/cli

# Restore dependencies
# Use secrets to configure nuget/registry authentication
RUN --mount=type=secret,id=GITHUB_USER,target=/run/secrets/GITHUB_USER \
    --mount=type=secret,id=GITHUB_PAT,target=/run/secrets/GITHUB_PAT \
    ls -l /run/secrets
RUN --mount=type=secret,id=GITHUB_USER,target=/run/secrets/GITHUB_USER \
    cat /run/secrets/GITHUB_USER
RUN --mount=type=secret,id=GITHUB_PAT,target=/run/secrets/GITHUB_PAT \
    cat /run/secrets/GITHUB_PAT

RUN --mount=type=secret,id=GITHUB_USER,target=/run/secrets/GITHUB_USER \
    --mount=type=secret,id=GITHUB_PAT,target=/run/secrets/GITHUB_PAT \
    dotnet nuget add source \
        --username "$(cat /run/secrets/GITHUB_USER)" \
        --password "$(cat /run/secrets/GITHUB_PAT)" \
        --store-password-in-clear-text \
        --name github "https://nuget.pkg.github.com/ichyd/index.json"
RUN dotnet restore

# Publish the application
RUN dotnet publish -c Release -o /app/publish

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