# -----------------------------
# Étape 1 : Image de base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 443

# -----------------------------
# Étape 2 : Build de l'application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# --- 1) Copier la solution et les csproj ---
# Comme vous êtes dans le dossier MyApiTemplateCleanArchi au moment du build,
# les chemins doivent être relatifs à ce dossier.
COPY MyApiTemplateCleanArchi.sln ./
COPY MyApiTemplateCleanArchi.Application MyApiTemplateCleanArchi.Application
COPY MyApiTemplateCleanArchi.Domain MyApiTemplateCleanArchi.Domain
COPY MyApiTemplateCleanArchi.Infrastructure MyApiTemplateCleanArchi.Infrastructure
COPY MyApiTemplateCleanArchi.Shared MyApiTemplateCleanArchi.Shared
COPY MyApiTemplateCleanArchi.Web MyApiTemplateCleanArchi.Web

# --- 2) Restaurer les dépendances ---
RUN dotnet restore "MyApiTemplateCleanArchi.Web/MyApiTemplateCleanArchi.Web.csproj"

# --- 3) Copier l'ensemble du code (optionnel si déjà fait ci-dessus) ---
# COPY . .  

# --- 4) Copier le dossier certs (qui est au même niveau que la solution .sln) ---
COPY certs certs

# --- 5) Compiler le projet ---
WORKDIR /src/MyApiTemplateCleanArchi.Web
RUN dotnet build "MyApiTemplateCleanArchi.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# -----------------------------
# Étape 3 : Publication
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MyApiTemplateCleanArchi.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# -----------------------------
# Étape 4 : Image finale
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copier encore les certs si vous en avez besoin dans l'image finale
COPY certs certs

ENTRYPOINT ["dotnet", "MyApiTemplateCleanArchi.Web.dll"]
