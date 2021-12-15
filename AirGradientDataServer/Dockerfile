# Build builder image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ./ ./
RUN dotnet publish -c Release -o out

# Install dotnet-ef tool and use it to create empty SQLite database
RUN dotnet tool install --global dotnet-ef
ENV PATH $PATH:/root/.dotnet/tools
RUN dotnet ef database update

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime-env
WORKDIR /app

COPY --from=build-env /app/out .
COPY --from=build-env /app/MeasurementsDB.db* .
ENTRYPOINT ["dotnet", "AirGradientDataServer.dll", "--urls", "http://*:5000"]
