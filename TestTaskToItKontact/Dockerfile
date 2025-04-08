FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TestTaskToItKontact/TestTaskToItKontact.csproj", "TestTaskToItKontact/"]
RUN dotnet restore "TestTaskToItKontact/TestTaskToItKontact.csproj"
COPY . .
WORKDIR "/src/TestTaskToItKontact"
RUN dotnet build "TestTaskToItKontact.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TestTaskToItKontact.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestTaskToItKontact.dll"]
