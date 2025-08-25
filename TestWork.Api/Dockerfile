FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TestWork.Api/TestWork.Api.csproj", "TestWork.Api/"]
COPY ["TestWork.Data/TestWork.Data.csproj", "TestWork.Data/"]
COPY ["TestWork/TestWork.csproj", "TestWork/"]
RUN dotnet restore "TestWork.Api/TestWork.Api.csproj"
COPY . .
WORKDIR "/src/TestWork.Api"
RUN dotnet build "./TestWork.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TestWork.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestWork.Api.dll"]
