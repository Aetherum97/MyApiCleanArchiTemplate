FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["MyApiTemplateCleanArchi.sln", "./"]

COPY ["MyApiTemplateCleanArchi.Application/MyApiTemplateCleanArchi.Application.csproj", "MyApiTemplateCleanArchi.Application/"]
COPY ["MyApiTemplateCleanArchi.Domain/MyApiTemplateCleanArchi.Domain.csproj", "MyApiTemplateCleanArchi.Domain/"]
COPY ["MyApiTemplateCleanArchi.Infrastructure/MyApiTemplateCleanArchi.Infrastructure.csproj", "MyApiTemplateCleanArchi.Infrastructure/"]
COPY ["MyApiTemplateCleanArchi.Shared/MyApiTemplateCleanArchi.Shared.csproj", "MyApiTemplateCleanArchi.Shared/"]
COPY ["MyApiTemplateCleanArchi.Web/MyApiTemplateCleanArchi.Web.csproj", "MyApiTemplateCleanArchi.Web/"]

RUN dotnet restore "MyApiTemplateCleanArchi.Web/MyApiTemplateCleanArchi.Web.csproj"

COPY . .

WORKDIR "/src/MyApiTemplateCleanArchi.Web"

RUN dotnet build "MyApiTemplateCleanArchi.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MyApiTemplateCleanArchi.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyApiTemplateCleanArchi.Web.dll"]
