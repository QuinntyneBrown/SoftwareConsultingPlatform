FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["src/Services/CaseStudies/SoftwareConsultingPlatform.CaseStudies.Api/SoftwareConsultingPlatform.CaseStudies.Api.csproj", "Services/CaseStudies/SoftwareConsultingPlatform.CaseStudies.Api/"]
COPY ["src/Services/CaseStudies/SoftwareConsultingPlatform.CaseStudies.Core/SoftwareConsultingPlatform.CaseStudies.Core.csproj", "Services/CaseStudies/SoftwareConsultingPlatform.CaseStudies.Core/"]
COPY ["src/Services/CaseStudies/SoftwareConsultingPlatform.CaseStudies.Infrastructure/SoftwareConsultingPlatform.CaseStudies.Infrastructure.csproj", "Services/CaseStudies/SoftwareConsultingPlatform.CaseStudies.Infrastructure/"]
COPY ["src/SoftwareConsultingPlatform.ServiceDefaults/SoftwareConsultingPlatform.ServiceDefaults.csproj", "SoftwareConsultingPlatform.ServiceDefaults/"]
COPY ["src/Shared/Shared.Messages/Shared.Messages.csproj", "Shared/Shared.Messages/"]
COPY ["src/Shared/Shared.Contracts/Shared.Contracts.csproj", "Shared/Shared.Contracts/"]
COPY ["src/Shared/Shared.Core/Shared.Core.csproj", "Shared/Shared.Core/"]

RUN dotnet restore "Services/CaseStudies/SoftwareConsultingPlatform.CaseStudies.Api/SoftwareConsultingPlatform.CaseStudies.Api.csproj"

# Copy source code
COPY src/ .

WORKDIR "/src/Services/CaseStudies/SoftwareConsultingPlatform.CaseStudies.Api"
RUN dotnet build "SoftwareConsultingPlatform.CaseStudies.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SoftwareConsultingPlatform.CaseStudies.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SoftwareConsultingPlatform.CaseStudies.Api.dll"]
