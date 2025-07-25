# Fragomen Order API

This project is a high-level design prototype for handling orders and managing their state via a stateless RESTful API.

## Project Structure

### fragomen_order_api/Data

* Contains all data access logic using Entity Framework Core.

* Includes the SQL Server database script and backup files.

### fragomen_order_api 

* A stateless RESTful API for interacting with the Fragomen database.

* Provides endpoints for order management and state transitions.

## Import SQL Backup

1. Locate the backup file:
{projectRoot}/fragomen_order_api/Data/*.bak

2. Restore it using SSMS or the following SQL command:

```sql
RESTORE DATABASE fragomen_orders_db
FROM DISK = 'C:\Path\To\Your\Project\fragomen_order_api\Data\fragomen_orders_db.bak'
WITH MOVE 'YourDataFilePathName' TO 'C:\Your\Path\fragomen_orders_db.mdf',
     MOVE 'YourDataFilePathName' TO 'C:\Your\Path\fragomen_orders_db.ldf',
     REPLACE;
```
