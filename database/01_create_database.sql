/* =====================================================================
   Fenix UNIP - Atlética
   Script 01 - Criação do banco de dados
   Alvo: SQL Server 2019+ (Express/Developer/Standard) via SSMS
   =====================================================================
   Execução: rodar este script conectado à instância do SQL Server
   (ex: localhost\SQLEXPRESS), FORA do contexto de qualquer banco
   específico (pode rodar com o contexto em "master").
   ===================================================================== */

USE master;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'FenixUnipDB')
BEGIN
    PRINT 'Criando banco de dados FenixUnipDB...';
    CREATE DATABASE FenixUnipDB;
END
ELSE
BEGIN
    PRINT 'Banco de dados FenixUnipDB já existe. Nenhuma ação realizada.';
END
GO

-- Ajusta o nível de compatibilidade para um valor moderno (opcional, mas recomendado)
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = N'FenixUnipDB')
BEGIN
    ALTER DATABASE FenixUnipDB SET RECOVERY SIMPLE;
END
GO

PRINT 'Script 01_create_database.sql concluído.';
GO
