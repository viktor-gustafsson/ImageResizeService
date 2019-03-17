ARG DOTNET_VERSION=2.2

FROM microsoft/dotnet:${DOTNET_VERSION}-sdk-alpine AS build
WORKDIR /src
COPY . .

FROM build AS publish
RUN dotnet publish ImageResizeService.csproj -c Release -o /app

FROM microsoft/dotnet:${DOTNET_VERSION}-aspnetcore-runtime AS final

RUN apt update && apt install -y \
libc6-dev \
libgdiplus

WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "ImageResizeService.dll"]