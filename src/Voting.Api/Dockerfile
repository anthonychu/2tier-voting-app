FROM microsoft/dotnet:2.0.7-sdk-2.1.105 AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# build runtime image
FROM microsoft/aspnetcore:2.0.7
WORKDIR /app
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "Voting.Api.dll"]
