FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src/Web

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /src/Web
COPY --from=build-env /src/Web/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "BevMan.Web.dll"]