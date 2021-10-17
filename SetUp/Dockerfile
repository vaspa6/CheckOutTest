
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

COPY /src/ /app/
RUN dotnet restore /app/CheckOutTest.Web/CheckOutTest.Web.csproj

RUN mkdir /dist
RUN dotnet publish /app/CheckOutTest.Web/CheckOutTest.Web.csproj -c release -o ./dist --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build ./dist ./
ENTRYPOINT ["dotnet", "CheckOutTest.Web.dll"]