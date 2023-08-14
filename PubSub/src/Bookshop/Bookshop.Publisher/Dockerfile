#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Bookshop.Publisher/Bookshop.Publisher.csproj", "Bookshop.Publisher/"]
COPY ["Bookshop.Common/Bookshop.Common.csproj", "Bookshop.Common/"]
RUN dotnet restore "Bookshop.Publisher/Bookshop.Publisher.csproj"
COPY . .
WORKDIR "/src/Bookshop.Publisher"
RUN dotnet build "Bookshop.Publisher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bookshop.Publisher.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookshop.Publisher.dll"]