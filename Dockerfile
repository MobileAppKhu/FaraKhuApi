FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build-env
COPY . /app
WORKDIR /app
RUN dotnet publish -c Release -o /build

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /build
COPY --from=build-env /build .
ENTRYPOINT [ "dotnet", "WebApi.dll" ]