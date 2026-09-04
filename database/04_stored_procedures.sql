/* =====================================================================
   Fenix UNIP - Atlética
   Script 04 - Stored Procedures
   Alvo: SQL Server 2019+ (Express/Developer/Standard) via SSMS
   =====================================================================
   Execução: rodar após 02_create_tables.sql (e opcionalmente após
   03_seed_data.sql, para já testar as procedures com dados de exemplo).
   ===================================================================== */

USE FenixUnipDB;
GO

-- =====================================================================
-- sp_AprovarTryout
-- Aprova ou reprova a inscrição de um candidato em um tryout.
-- Uso:
--   EXEC dbo.sp_AprovarTryout @TryoutId = 1, @NovoStatus = N'Aprovado';
-- =====================================================================
IF OBJECT_ID(N'dbo.sp_AprovarTryout', N'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_AprovarTryout;
GO
CREATE PROCEDURE dbo.sp_AprovarTryout
    @TryoutId   INT,
    @NovoStatus NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;

    IF @NovoStatus NOT IN (N'Pendente', N'Aprovado', N'Reprovado')
    BEGIN
        RAISERROR(N'Status inválido. Utilize: Pendente, Aprovado ou Reprovado.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.Tryouts WHERE Id = @TryoutId)
    BEGIN
        RAISERROR(N'Tryout com o Id informado não foi encontrado.', 16, 1);
        RETURN;
    END

    UPDATE dbo.Tryouts
    SET Status = @NovoStatus
    WHERE Id = @TryoutId;

    SELECT Id, NomeCandidato, Email, Status
    FROM dbo.Tryouts
    WHERE Id = @TryoutId;
END
GO

-- =====================================================================
-- sp_RelatorioVendasProdutos
-- Relatório de vendas agregado por produto, considerando apenas
-- pedidos que não estejam cancelados. Aceita filtro opcional de
-- período (DataInicio/DataFim); se omitidos, considera todo o histórico.
-- Uso:
--   EXEC dbo.sp_RelatorioVendasProdutos;
--   EXEC dbo.sp_RelatorioVendasProdutos @DataInicio = '2026-01-01', @DataFim = '2026-12-31';
-- =====================================================================
IF OBJECT_ID(N'dbo.sp_RelatorioVendasProdutos', N'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_RelatorioVendasProdutos;
GO
CREATE PROCEDURE dbo.sp_RelatorioVendasProdutos
    @DataInicio DATETIME2 = NULL,
    @DataFim    DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.Id                                AS ProdutoId,
        p.Nome                              AS Produto,
        p.Categoria,
        SUM(ip.Quantidade)                  AS QuantidadeVendida,
        SUM(ip.Quantidade * ip.PrecoUnitario) AS ValorTotalVendido,
        COUNT(DISTINCT ip.PedidoId)         AS QuantidadePedidos
    FROM dbo.Produtos p
    INNER JOIN dbo.ItensPedido ip ON ip.ProdutoId = p.Id
    INNER JOIN dbo.Pedidos pe ON pe.Id = ip.PedidoId
    WHERE pe.Status <> N'Cancelado'
      AND (@DataInicio IS NULL OR pe.DataPedido >= @DataInicio)
      AND (@DataFim    IS NULL OR pe.DataPedido <= @DataFim)
    GROUP BY p.Id, p.Nome, p.Categoria
    ORDER BY ValorTotalVendido DESC;
END
GO

-- =====================================================================
-- sp_ResponderSugestao
-- Registra a resposta de um administrador a uma sugestão e marca o
-- status como "Respondida".
-- Uso:
--   EXEC dbo.sp_ResponderSugestao @SugestaoId = 1, @Resposta = N'Obrigado pela sugestão!';
-- =====================================================================
IF OBJECT_ID(N'dbo.sp_ResponderSugestao', N'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ResponderSugestao;
GO
CREATE PROCEDURE dbo.sp_ResponderSugestao
    @SugestaoId INT,
    @Resposta   NVARCHAR(1000)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Sugestoes WHERE Id = @SugestaoId)
    BEGIN
        RAISERROR(N'Sugestão com o Id informado não foi encontrada.', 16, 1);
        RETURN;
    END

    UPDATE dbo.Sugestoes
    SET RespostaAdmin = @Resposta,
        Status = N'Respondida'
    WHERE Id = @SugestaoId;

    SELECT Id, NomeAutor, Categoria, Status, RespostaAdmin
    FROM dbo.Sugestoes
    WHERE Id = @SugestaoId;
END
GO

PRINT 'Script 04_stored_procedures.sql concluído: procedures sp_AprovarTryout, sp_RelatorioVendasProdutos e sp_ResponderSugestao criadas.';
GO
