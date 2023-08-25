#!/bin/bash -e

#Cleanup
docker stop auroracontainer || true
docker rm auroracontainer || true

#Run container
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Hdf3@fwvR355g" -p 1434:1433 --name auroracontainer -d mcr.microsoft.com/mssql/server:2022-latest

#Wait for container
sleep 15

#Create DB
docker cp db.sql auroracontainer:db.sql

docker exec auroracontainer 'opt/mssql-tools/bin/sqlcmd' -S localhost -U sa -P Hdf3@fwvR355g -i db.sql


#Run DB Migrations
cd src/Aurora.Migrations
dotnet run --project Aurora.Migrations.csproj --configuration Debug
cd ..
cd ..