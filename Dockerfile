# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

RUN dotnet restore
RUN dotnet build ./ImpowerRetro.csproj --no-restore -c Release -o /app/build
RUN dotnet publish ./ImpowerRetro.csproj --no-restore -c Release -o /app/publish

# Run stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENV PORT=8080
ENV ASPNETCORE_URLS=http://[::]:${PORT}
ENV ASPNETCORE_HTTPS_PORT=443
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
EXPOSE ${PORT}
ENTRYPOINT ["dotnet", "ImpowerRetro.dll"]