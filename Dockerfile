FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["RestaurantApi/RestaurantApi.csproj", "RestaurantApi/"]
RUN dotnet restore "RestaurantApi/RestaurantApi.csproj"

COPY RestaurantApi/. RestaurantApi/
WORKDIR /src/RestaurantApi
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "RestaurantApi.dll"]
