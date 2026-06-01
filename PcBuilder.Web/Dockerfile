FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["PcBuilder.Web/PcBuilder.Web.csproj", "PcBuilder.Web/"]
COPY ["PcBuilder.Application/PcBuilder.Application.csproj", "PcBuilder.Application/"]
COPY ["PcBuilder.Infrastructure/PcBuilder.Infrastructure.csproj", "PcBuilder.Infrastructure/"]
COPY ["PcBuilder.Domain/PcBuilder.Domain.csproj", "PcBuilder.Domain/"]
RUN dotnet restore "PcBuilder.Web/PcBuilder.Web.csproj"

COPY . .
WORKDIR "/src/PcBuilder.Web"


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PcBuilder.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

USER $APP_UID
ENTRYPOINT ["dotnet", "PcBuilder.Web.dll"]