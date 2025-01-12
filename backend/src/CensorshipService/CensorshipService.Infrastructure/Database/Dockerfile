FROM mcr.microsoft.com/mssql/server:2022-CU13-ubuntu-22.04

COPY ./wait-and-run.sh /wait-and-run.sh
COPY ./Scripts/CreateDatabaseAndSeed.sql /CreateDatabaseAndSeed.sql

CMD /wait-and-run.sh