FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# Default port in .net 8 changed to 8080 instead of 80 
# however if using WebHost.CreateDefaultBuilder() default port will be 5000
# but docker wont expose 5000 so I set ASPNETCORE_URLS=http://*:8080 in config
EXPOSE 8080 
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TheResistanceOnline.Server/TheResistanceOnline.Server.csproj", "TheResistanceOnline.Server/"]
RUN dotnet restore "TheResistanceOnline.Server/TheResistanceOnline.Server.csproj"
COPY . .
WORKDIR "/src/TheResistanceOnline.Server"
RUN dotnet build "TheResistanceOnline.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TheResistanceOnline.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheResistanceOnline.Server.dll"]
