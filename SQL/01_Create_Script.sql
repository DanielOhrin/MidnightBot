use [master]
GO

DROP DATABASE IF EXISTS MidnightSkyBot;
GO

CREATE DATABASE MidnightSkyBot
GO

use [MidnightSkyBot]
GO

CREATE TABLE UserProfile (
	Id bigint PRIMARY KEY,
	PlayerId nvarchar(100) UNIQUE,
	ApiKey nvarchar(100) UNIQUE,
	IsTrackingIsland bit DEFAULT(0),
	IsBanned bit DEFAULT(0)
)