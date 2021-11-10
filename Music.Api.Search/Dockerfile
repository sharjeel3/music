#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Music.Api.Search/Music.Api.Search.csproj", "Music.Api.Search/"]
RUN dotnet restore "Music.Api.Search/Music.Api.Search.csproj"
COPY . .
WORKDIR "/src/Music.Api.Search"
RUN dotnet build "Music.Api.Search.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Music.Api.Search.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Music.Api.Search.dll"]