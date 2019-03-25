ARG DOTNET_VERSION=2.1

FROM microsoft/dotnet:${DOTNET_VERSION}-sdk-alpine AS build
WORKDIR /src
COPY . .

FROM build AS publish
RUN dotnet publish ImageResizeService/ImageResizeService.csproj -c Release -o /app

FROM ubuntu:16.04
#This is needed for SkiaSharp
RUN apt-get -qq update && apt-get install -y libfontconfig1

# Install utilities
RUN apt-get update && \
	apt-get install -y curl apt-transport-https wget unzip && \
	rm -rf /var/lib/apt/lists/*
#Add microsoft packages
RUN wget -q https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb

#Install dotnet runtime
RUN apt-get update && \
	apt-get install -y \
	apt-transport-https \
	aspnetcore-runtime-2.1 && \
	rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "ImageResizeService.dll"]
