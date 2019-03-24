ARG DOTNET_VERSION=2.1

FROM microsoft/dotnet:${DOTNET_VERSION}-sdk-alpine AS build
WORKDIR /src
COPY . .

FROM build AS publish
RUN dotnet publish ImageResizeService/ImageResizeService.csproj -c Release -o /app

FROM microsoft/dotnet:${DOTNET_VERSION}-aspnetcore-runtime-alpine AS final

WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "ImageResizeService.dll"]