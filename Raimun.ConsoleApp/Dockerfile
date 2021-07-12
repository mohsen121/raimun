FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Raimun.ConsoleApp.csproj", "Raimun.ConsoleApp/"]
RUN dotnet restore "Raimun.ConsoleApp/Raimun.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/Raimun.ConsoleApp"
RUN dotnet build "Raimun.ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Raimun.ConsoleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Raimun.ConsoleApp.dll"]