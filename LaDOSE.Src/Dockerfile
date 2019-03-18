FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy everything else and build
COPY . ./

RUN dotnet publish LaDOSE.linux.sln -c Release -o out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/LaDOSE.Api/out/ .
EXPOSE 5000
ENTRYPOINT ["dotnet", "LaDOSE.Api.dll"]

