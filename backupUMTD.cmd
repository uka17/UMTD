echo off
cls
echo Clouding of DB backup
"C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\110\Tools\Binn\SQLCMD.EXE" -S localhost\SQLEXPRESS -i D:\Files\UMTD\backupUMTDdb.sql
echo Done!
pause