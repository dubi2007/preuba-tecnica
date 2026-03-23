-- ============================================================
-- Base de Datos: LoginAppTestDb
-- Descripción: Aplicativo de Login y Gestión de Perfiles de Usuarios
-- Fecha: 2026
-- ============================================================

-- Crear la tabla de Usuarios
CREATE TABLE [Users] (
    -- Identificador Principal
    [Username] NVARCHAR(MAX) NOT NULL PRIMARY KEY,
    
    -- Credenciales
    [Password] NVARCHAR(MAX) NOT NULL,
    [Role] NVARCHAR(MAX) NULL,
    
    -- Información Personal Básica
    [FirstName] NVARCHAR(MAX) NULL,
    [LastName1] NVARCHAR(MAX) NULL,
    [LastName2] NVARCHAR(MAX) NULL,
    
    -- Información de Identidad
    [DocumentType] NVARCHAR(MAX) NULL,          -- DNI, Pasaporte, etc.
    [DocumentNumber] NVARCHAR(MAX) NULL,
    [BirthDate] NVARCHAR(MAX) NULL,             -- Formato: "15 / 04 / 1944"
    
    -- Información Demográfica
    [Nationality] NVARCHAR(MAX) NULL,           -- Peruana, etc.
    [Gender] NVARCHAR(MAX) NULL,                -- Femenino, Masculino, etc.
    
    -- Información de Contacto
    [MainEmail] NVARCHAR(MAX) NULL,
    [SecondaryEmail] NVARCHAR(MAX) NULL,
    [MobilePhone] NVARCHAR(MAX) NULL,
    [SecondaryPhoneType] NVARCHAR(MAX) NULL,    -- Tipo de teléfono secundario
    [SecondaryPhone] NVARCHAR(MAX) NULL,
    
    -- Información de Contratación
    [ContractType] NVARCHAR(MAX) NULL,          -- CAS, Nombrado, Contratado, etc.
    [ContractDate] NVARCHAR(MAX) NULL,          -- Formato: "09 / 03 / 2015"
    
    -- Información Institucional
    [Institution] NVARCHAR(MAX) NULL,           -- 011 Ministerio de Salud, etc.
    [Status] NVARCHAR(MAX) NULL,                -- Activo, Inactivo, etc.
    
    -- Control de Seguridad
    [AccessFailedCount] INT NOT NULL DEFAULT 0, -- Contador de intentos fallidos
    [LockoutEnd] DATETIME2 NULL                 -- Fecha/hora de bloqueo de cuenta
);

-- ============================================================
-- Insertar Usuario de Prueba
-- ============================================================
INSERT INTO [Users] (
    [Username],
    [Password],
    [Role],
    [FirstName],
    [LastName1],
    [LastName2],
    [DocumentType],
    [DocumentNumber],
    [BirthDate],
    [Nationality],
    [Gender],
    [MainEmail],
    [SecondaryEmail],
    [MobilePhone],
    [SecondaryPhoneType],
    [SecondaryPhone],
    [ContractType],
    [ContractDate],
    [Institution],
    [Status],
    [AccessFailedCount],
    [LockoutEnd]
) VALUES (
    N'07079879',                              -- Username
    N'123',                                    -- Password
    N'Administrador de Recursos',             -- Role
    N'July Camila',                            -- FirstName
    N'Mendoza',                                -- LastName1
    N'Quispe',                                 -- LastName2
    N'DNI',                                    -- DocumentType
    N'07079879',                               -- DocumentNumber
    N'15 / 04 / 1944',                        -- BirthDate
    N'Peruana',                                -- Nationality
    N'Femenino',                               -- Gender
    N'test@minsa.gob.pe',                     -- MainEmail
    NULL,                                      -- SecondaryEmail
    N'+51 999 999 999',                        -- MobilePhone
    N'Tipo',                                   -- SecondaryPhoneType
    NULL,                                      -- SecondaryPhone
    N'CAS',                                    -- ContractType
    N'09 / 03 / 2015',                        -- ContractDate
    N'011 Ministerio de Salud',                -- Institution
    N'Activo',                                 -- Status
    0,                                         -- AccessFailedCount
    NULL                                       -- LockoutEnd
);

-- ============================================================
-- Vista: Resumen de Usuarios
-- ============================================================
CREATE VIEW vw_UsuariosResumen AS
SELECT
    [Username],
    [FirstName] + ' ' + [LastName1] + ' ' + [LastName2] AS [NombreCompleto],
    [Role],
    [MainEmail],
    [MobilePhone],
    [Institution],
    [Status],
    [AccessFailedCount],
    [LockoutEnd]
FROM [Users];

 -- indiices para ayuda de busqueda 
CREATE INDEX [IX_Users_Username] ON [Users] ([Username]);
CREATE INDEX [IX_Users_MainEmail] ON [Users] ([MainEmail]);
CREATE INDEX [IX_Users_Status] ON [Users] ([Status]);

