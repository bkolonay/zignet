USE ZigNet

BACKUP DATABASE ZigNetTestResults
TO DISK = 'C:\zignet\db-backups\ZigNet-baseline--1-0-0--12-17-2017-12-30-pm.bak' WITH FORMAT

-- https://docs.microsoft.com/en-us/sql/relational-databases/backup-restore/create-a-full-database-backup-sql-server
-- By default, BACKUP appends the backup to an existing media set, preserving existing backup sets
-- Use the FORMAT clause when you are using media for the first time or you want to overwrite all existing data
