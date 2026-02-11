#!/bin/bash

# Wait for the SQL Server to start up
until /opt/mssql-tools/bin/sqlcmd -S db -U SA -P "YourStrong@Password123" -Q 'SELECT 1' > /dev/null 2>&1; do
  echo "Waiting for SQL Server to be available..."
  sleep 5
done

echo "SQL Server is up - executing initialization scripts"

/opt/mssql-tools/bin/sqlcmd -S db -U SA -P "YourStrong@Password123" -d master -i /usr/local/bin/create-db.sql
/opt/mssql-tools/bin/sqlcmd -S db -U SA -P "YourStrong@Password123" -d master -i /usr/local/bin/create-user.sql

echo "SQL Server initialization complete"