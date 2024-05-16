FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EventSlideshows/EventSlideshows.csproj", "EventSlideshows/"]
COPY ["DataAccessLibrary/DataAccessLibrary.csproj", "DataAccessLibrary/"]
RUN dotnet restore "EventSlideshows/EventSlideshows.csproj"
COPY . .
WORKDIR "/src/EventSlideshows"
RUN dotnet build "EventSlideshows.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EventSlideshows.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventSlideshows.dll"]
