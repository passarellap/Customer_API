#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS sdk
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY *.sln .
COPY ["Anagrafica.Api/Anagrafica.Api.csproj", "Anagrafica.Api/"]
COPY ["Anagrafica.Domain/Anagrafica.Domain.csproj", "Anagrafica.Domain/"]
COPY ["Anagrafica.Entity/Anagrafica.Entity.csproj", "Anagrafica.Entity/"]
COPY ["UnitTest.AnagraficaAPI/UnitTest.AnagraficaAPI.csproj", "UnitTest.AnagraficaAPI/"]

# add nuget server to container host
ARG NUGETSERVERS
COPY add-nuget-server.sh .
RUN chmod +x "/src/add-nuget-server.sh"
RUN bash -c "/src/add-nuget-server.sh $NUGETSERVERS"

# restore nuget della solution
RUN dotnet restore --disable-parallel

# copia la cartella della solution
COPY . .

# effettua la build della solution impostando la version passata come parametro nella variabile BUILDVERSIONNUMBER
ARG BUILDVERSIONNUMBER
WORKDIR "/src/Anagrafica.Api"
RUN dotnet build "Anagrafica.Api.csproj" -c Release -o /app/build /p:Version=$BUILDVERSIONNUMBER

# esegue i test
FROM build AS testrunner
WORKDIR "/src/UnitTest.AnagraficaAPI"
CMD ["dotnet", "test", "console;verbosity=normal"]

# esegue gli unit tests
FROM build AS test
WORKDIR "/src/UnitTest.AnagraficaAPI"
RUN dotnet test --logger "console;verbosity=normal"

# effettua la publish impostando la version passata come parametro nella variabile BUILDVERSIONNUMBER
FROM build AS publish
ARG BUILDVERSIONNUMBER
RUN dotnet publish "Anagrafica.Api.csproj" -c Release -o /app/publish /p:Version=$BUILDVERSIONNUMBER

# setta l'entrypoint per docker
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AnagraficaAPI.dll"]