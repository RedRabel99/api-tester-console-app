FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["api-tester-console-app/api-tester-console-app.csproj", "api-tester-console-app/"]
RUN dotnet restore "api-tester-console-app/api-tester-console-app.csproj"
COPY . .
WORKDIR "/src/api-tester-console-app"
RUN dotnet build "api-tester-console-app.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "api-tester-console-app.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api-tester-console-app.dll"]
