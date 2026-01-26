FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["src/ApiGateway/SoftwareConsultingPlatform.ApiGateway/SoftwareConsultingPlatform.ApiGateway.csproj", "ApiGateway/SoftwareConsultingPlatform.ApiGateway/"]
COPY ["src/SoftwareConsultingPlatform.ServiceDefaults/SoftwareConsultingPlatform.ServiceDefaults.csproj", "SoftwareConsultingPlatform.ServiceDefaults/"]

RUN dotnet restore "ApiGateway/SoftwareConsultingPlatform.ApiGateway/SoftwareConsultingPlatform.ApiGateway.csproj"

# Copy source code
COPY src/ .

WORKDIR "/src/ApiGateway/SoftwareConsultingPlatform.ApiGateway"
RUN dotnet build "SoftwareConsultingPlatform.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SoftwareConsultingPlatform.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SoftwareConsultingPlatform.ApiGateway.dll"]
