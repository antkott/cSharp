FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /build
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o output

FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build /build/output .
ENTRYPOINT ["dotnet", "ConferenceApp.dll"]