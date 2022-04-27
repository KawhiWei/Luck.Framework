#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["sample/Module.Sample/Module.Sample.csproj", "sample/Module.Sample/"]
COPY ["src/framework/Luck.Redis.StackExchange/Luck.Redis.StackExchange.csproj", "src/framework/Luck.Redis.StackExchange/"]
COPY ["src/framework/Luck.Framework/Luck.Framework.csproj", "src/framework/Luck.Framework/"]
COPY ["src/framework/Luck.EntityFrameworkCore/Luck.EntityFrameworkCore.csproj", "src/framework/Luck.EntityFrameworkCore/"]
COPY ["src/framework/Luck.DDD.Domain/Luck.DDD.Domain.csproj", "src/framework/Luck.DDD.Domain/"]
RUN dotnet restore "sample/Module.Sample/Module.Sample.csproj"
COPY . .
WORKDIR "/src/sample/Module.Sample"
RUN dotnet build "Module.Sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Module.Sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Module.Sample.dll"]