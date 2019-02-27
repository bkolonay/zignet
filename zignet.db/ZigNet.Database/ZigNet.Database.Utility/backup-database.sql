USE ZigNet

BACKUP DATABASE ZigNet
TO DISK = 'C:\code\db-backups\zignet-backup-04-06-2019-10-45am.bak' WITH FORMAT

-- https://docs.microsoft.com/en-us/sql/relational-databases/backup-restore/create-a-full-database-backup-sql-server
-- By default, BACKUP appends the backup to an existing media set, preserving existing backup sets
-- Use the FORMAT clause when you are using media for the first time or you want to overwrite all existing data
