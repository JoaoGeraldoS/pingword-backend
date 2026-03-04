FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore


COPY . ./
RUN dotnet publish "pingword.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /runtime-app
COPY --from=build /app/out .

EXPOSE 8080
ENTRYPOINT [ "dotnet", "pingword.dll" ]
