
# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY Carsties.sln ./
COPY AuctionsService/AuctionService.csproj ./AuctionsService/
COPY Contracts/Contracts.csproj ./Contracts/

RUN dotnet restore ./AuctionsService/AuctionService.csproj

COPY . . 
RUN dotnet publish ./AuctionsService/AuctionService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AuctionService.dll"]
