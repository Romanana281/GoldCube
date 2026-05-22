FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY GoldCube.csproj ./
RUN dotnet restore GoldCube.csproj

COPY . .
RUN dotnet publish GoldCube.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "GoldCube.dll"]
