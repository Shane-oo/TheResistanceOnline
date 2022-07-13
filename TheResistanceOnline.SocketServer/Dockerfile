FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TheResistanceOnline.SocketServer/TheResistanceOnline.SocketServer.csproj", "TheResistanceOnline.SocketServer/"]
RUN dotnet restore "TheResistanceOnline.SocketServer/TheResistanceOnline.SocketServer.csproj"
COPY . .
WORKDIR "/src/TheResistanceOnline.SocketServer"
RUN dotnet build "TheResistanceOnline.SocketServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TheResistanceOnline.SocketServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheResistanceOnline.SocketServer.dll"]
