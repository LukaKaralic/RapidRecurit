IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'RapidRecruit')
BEGIN
    CREATE DATABASE RapidRecruit;
END
