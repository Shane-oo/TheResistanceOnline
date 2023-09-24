FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Install Node.js
RUN apt-get update && apt-get install -y ca-certificates curl gnupg && \
    curl -fsSL https://deb.nodesource.com/gpgkey/nodesource-repo.gpg.key | gpg --dearmor -o /etc/apt/keyrings/nodesource.gpg && \
    NODE_MAJOR=18 && \
    echo "deb [signed-by=/etc/apt/keyrings/nodesource.gpg] https://deb.nodesource.com/node_$NODE_MAJOR.x nodistro main" | tee /etc/apt/sources.list.d/nodesource.list && \
    apt-get update && apt-get install nodejs -y

WORKDIR /src
COPY ["TheResistanceOnline.Web/TheResistanceOnline.Web.csproj", "TheResistanceOnline.Web/"]
RUN dotnet restore "TheResistanceOnline.Web/TheResistanceOnline.Web.csproj"
COPY . .
WORKDIR "/src/TheResistanceOnline.Web"
RUN dotnet build "TheResistanceOnline.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TheResistanceOnline.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheResistanceOnline.Web.dll"]
