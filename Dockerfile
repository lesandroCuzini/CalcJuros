# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY *.sln .
COPY CalcJuros.Api/*.csproj CalcJuros.Api/
RUN dotnet restore
COPY . .

# testing
FROM build AS testing
WORKDIR /src/CalcJuros.Api
RUN dotnet build
WORKDIR /src/CalcJuros.UnitTest
RUN dotnet test

# publish
FROM build AS publish
WORKDIR /src/CalcJuros.Api
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
# ENTRYPOINT ["dotnet", "CalcJuros.Api.dll"]
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet CalcJuros.Api.dll