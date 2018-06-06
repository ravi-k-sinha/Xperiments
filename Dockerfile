From microsoft/aspnetcore-build:2.0 as build-env
WORKDIR /app

ADD ./Xperiments.Api /app/Xperiments.Api
WORKDIR /app/Xperiments.Api
RUN dotnet restore
RUN dotnet publish --no-restore -c Release -o out

#Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/Xperiments.Api/out .
ENTRYPOINT ["dotnet", "Xperiments.Api.dll"]