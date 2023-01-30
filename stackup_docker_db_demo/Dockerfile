#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["stackup_docker_db_demo/stackup_docker_db_demo.csproj", "stackup_docker_db_demo/"]
RUN dotnet restore "stackup_docker_db_demo/stackup_docker_db_demo.csproj"
COPY . .
WORKDIR "/src/stackup_docker_db_demo"
RUN dotnet build "stackup_docker_db_demo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "stackup_docker_db_demo.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "stackup_docker_db_demo.dll"]