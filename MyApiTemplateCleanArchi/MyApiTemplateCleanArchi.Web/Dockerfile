
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["MyApiTemplateCleanArchi/MyApiTemplateCleanArchi.sln", "MyApiTemplateCleanArchi.sln"]
COPY ["MyApiTemplateCleanArchi/MyApiTemplateCleanArchi.Application/MyApiTemplateCleanArchi.Application.csproj", "MyApiTemplateCleanArchi/MyApiTemplateCleanArchi.Application/"]
COPY ["MyApiTemplateCleanArchi/MyApiTemplateCleanArchi.Domain/MyApiTemplateCleanArchi.Domain.csproj", "MyApiTemplateCleanArchi/MyApiTemplateCleanArchi.Domain/"]
COPY ["MyApiTemplateCleanArchi/MyApiTemplateCleanArchi.Infrastructure/MyApiTemplateCleanArchi.Infrastructure.csproj", "MyApiTemplateCleanArchi/MyApiTemplateCleanArchi.Infrastructure/"]
COPY ["MyApiTemplateCleanArchi/MyApiTemplateCleanArchi.Shared/MyApiTemplateCleanArchi.Shared.csproj", "MyApiTemplateCleanArchi/MyApiTemplateCleanArchi.Shared/"]
COPY ["MyApiTemplateCleanArchi/MyApiTemplateCleanArchi.Web/MyApiTemplateCleanArchi.Web.csproj", "MyApiTemplateCleanArchi/MyApiTemplateCleanArchi.Web/"]

RUN dotnet restore "MyApiTemplateCleanArchi/MyApiTemplateCleanArchi.Web/MyApiTemplateCleanArchi.Web.csproj"

COPY . .  

COPY ["certs", "certs"]

WORKDIR "/src/MyApiTemplateCleanArchi/MyApiTemplateCleanArchi.Web"
RUN dotnet build "MyApiTemplateCleanArchi.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MyApiTemplateCleanArchi.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["certs", "certs"]

ENTRYPOINT ["dotnet", "MyApiTemplateCleanArchi.Web.dll"]
