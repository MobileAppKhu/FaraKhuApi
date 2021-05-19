﻿FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build-env
COPY . /app
WORKDIR /app
RUN dotnet publish -c Release -o /build

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /build
COPY --from=build-env /build .
RUN apt update
RUN apt install -y procps
ENTRYPOINT [ "dotnet", "WebApi.dll" ]