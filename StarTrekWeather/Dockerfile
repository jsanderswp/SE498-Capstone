FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src


COPY StarTrekWeather/*.csproj ./StarTrekWeather/


RUN dotnet restore StarTrekWeather/StarTrekWeather.csproj


COPY . .


RUN dotnet publish StarTrekWeather/StarTrekWeather.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "StarTrekWeather.dll"]