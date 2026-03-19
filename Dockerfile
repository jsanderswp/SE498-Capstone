FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /src
COPY . .

RUN dotnet restore StarTrekWeather/StarTrekWeather.csproj
RUN dotnet build StarTrekWeather/StarTrekWeather.csproj -c Release

WORKDIR /src/StarTrekWeather
CMD ["dotnet", "run"]