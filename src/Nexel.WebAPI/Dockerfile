FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Nexel.WebAPI/Nexel.WebAPI.csproj", "src/Nexel.WebAPI/"]
COPY ["src/Nexel.Application/Nexel.Application.csproj", "src/Nexel.Application/"]
COPY ["src/Nexel.Domain/Nexel.Domain.csproj", "src/Nexel.Domain/"]
COPY ["src/Nexel.Persistence/Nexel.Persistence.csproj", "src/Nexel.Persistence/"]
RUN dotnet restore "src/Nexel.WebAPI/Nexel.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Nexel.WebAPI"
RUN dotnet build "Nexel.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nexel.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nexel.WebAPI.dll"]
