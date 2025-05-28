CREATE DATABASE SolicitudesDB;
GO
USE SolicitudesDB;
GO
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    NombreUsuario NVARCHAR(50) NOT NULL UNIQUE,
    Contrasena NVARCHAR(100) NOT NULL
);
CREATE TABLE Solicitudes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    UsuarioRegistro NVARCHAR(50),
    CodigoSolicitud NVARCHAR(50) UNIQUE,
    Detalle NVARCHAR(MAX),
    Modificado BIT DEFAULT 0,
    Eliminado BIT DEFAULT 0
);

CREATE TABLE Eliminaciones (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FechaEliminacion DATETIME NOT NULL DEFAULT GETDATE(),
    Motivo NVARCHAR(MAX),
    CodigoSolicitud NVARCHAR(50),
    UsuarioSolicitante NVARCHAR(50),
    UsuarioEliminador NVARCHAR(50),
    Detalle NVARCHAR(MAX)
);
INSERT INTO Usuarios (NombreUsuario, Contrasena)
VALUES ('admin', 'admin1024*');

SELECT * FROM Usuarios;
SELECT * FROM Solicitudes;

SELECT * FROM Eliminaciones

