FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["src/Services/Services/SoftwareConsultingPlatform.Services.Api/SoftwareConsultingPlatform.Services.Api.csproj", "Services/Services/SoftwareConsultingPlatform.Services.Api/"]
COPY ["src/Services/Services/SoftwareConsultingPlatform.Services.Core/SoftwareConsultingPlatform.Services.Core.csproj", "Services/Services/SoftwareConsultingPlatform.Services.Core/"]
COPY ["src/Services/Services/SoftwareConsultingPlatform.Services.Infrastructure/SoftwareConsultingPlatform.Services.Infrastructure.csproj", "Services/Services/SoftwareConsultingPlatform.Services.Infrastructure/"]
COPY ["src/SoftwareConsultingPlatform.ServiceDefaults/SoftwareConsultingPlatform.ServiceDefaults.csproj", "SoftwareConsultingPlatform.ServiceDefaults/"]
COPY ["src/Shared/Shared.Messages/Shared.Messages.csproj", "Shared/Shared.Messages/"]
COPY ["src/Shared/Shared.Contracts/Shared.Contracts.csproj", "Shared/Shared.Contracts/"]
COPY ["src/Shared/Shared.Core/Shared.Core.csproj", "Shared/Shared.Core/"]

RUN dotnet restore "Services/Services/SoftwareConsultingPlatform.Services.Api/SoftwareConsultingPlatform.Services.Api.csproj"

# Copy source code
COPY src/ .

WORKDIR "/src/Services/Services/SoftwareConsultingPlatform.Services.Api"
RUN dotnet build "SoftwareConsultingPlatform.Services.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SoftwareConsultingPlatform.Services.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SoftwareConsultingPlatform.Services.Api.dll"]
