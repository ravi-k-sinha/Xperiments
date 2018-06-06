From microsoft/aspnetcore-build:2.0 as build-env
WORKDIR /app

ADD ./Xperiments.Api /app/Xperiments.Api
WORKDIR /app/Xperiments.Api
RUN dotnet restore -s http://build.lendfoundry.co/guestAuth/app/nuget/v1/FeedService.svc -s https://api.nuget.org/v3/index.json -s https://www.myget.org/F/ubsr4/auth/22e30ffa-b35f-4427-a4ee-58928a7aba13/api/v3/index.json
RUN dotnet publish --no-restore -c Release -o out

#Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/Xperiments.Api/out .
ENTRYPOINT ["dotnet", "Xperiments.Api.dll"]