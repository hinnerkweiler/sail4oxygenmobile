FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env
WORKDIR /app
COPY . /app
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /s4oserver
COPY --from=build-env /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "Sailing4oxygenApi.dll"]
