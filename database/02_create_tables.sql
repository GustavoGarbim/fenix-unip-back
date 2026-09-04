/* =====================================================================
   Fenix UNIP - Atlética
   Script 02 - Criação das tabelas, constraints e índices
   Alvo: SQL Server 2019+ (Express/Developer/Standard) via SSMS
   =====================================================================
   Execução: rodar após 01_create_database.sql, com o contexto de banco
   já apontando para FenixUnipDB (o USE abaixo garante isso).

   Convenções:
     - Nomes de tabelas no plural, PascalCase (padrão EF Core).
     - PKs: PK_<Tabela>
     - FKs: FK_<TabelaOrigem>_<TabelaDestino>[_<Coluna>]
     - UNIQUE: UQ_<Tabela>_<Coluna>
     - CHECK: CK_<Tabela>_<Regra>
     - Índices: IX_<Tabela>_<Coluna>

   Regras de ON DELETE adotadas (evitando múltiplos caminhos de cascata
   conflitantes no SQL Server):
     - Noticias.AdministradorId -> Administradores.Id: SET NULL
       (coluna nullable; ao remover o admin, a notícia permanece órfã).
     - Eventos.ModalidadeId -> Modalidades.Id: CASCADE
       (eventos são propriedade da modalidade; ao remover a modalidade,
        sua agenda de eventos é removida junto).
     - Tryouts.ModalidadeId -> Modalidades.Id: NO ACTION
       (histórico de inscrições de candidatos é preservado mesmo que a
        modalidade seja removida; requer tratamento manual/admin).
     - Pedidos.UsuarioId -> Usuarios.Id: NO ACTION
       (preserva histórico financeiro; usuários devem ser inativados via
        coluna Ativo, não excluídos, quando possuem pedidos).
     - ItensPedido.PedidoId -> Pedidos.Id: CASCADE
       (itens de pedido são sempre filhos do pedido e não existem sem ele).
     - ItensPedido.ProdutoId -> Produtos.Id: NO ACTION
       (preserva histórico de vendas mesmo que o produto seja excluído;
        prefira inativar produtos via coluna Ativo).
     - CarteirinhasDigitais.UsuarioId -> Usuarios.Id: CASCADE
       (registro 1:1 dependente do usuário; some dever ser removido junto).
   ===================================================================== */

USE FenixUnipDB;
GO

/* ---------------------------------------------------------------------
   Reexecução segura: se as tabelas já existirem (de uma execução
   anterior deste script), removê-las respeitando a ordem inversa de
   dependência (filhas antes das tabelas referenciadas), para que os
   DROP TABLE não falhem por violação de FOREIGN KEY.
   --------------------------------------------------------------------- */
IF OBJECT_ID(N'dbo.ItensPedido', N'U')          IS NOT NULL DROP TABLE dbo.ItensPedido;
IF OBJECT_ID(N'dbo.CarteirinhasDigitais', N'U') IS NOT NULL DROP TABLE dbo.CarteirinhasDigitais;
IF OBJECT_ID(N'dbo.Pedidos', N'U')              IS NOT NULL DROP TABLE dbo.Pedidos;
IF OBJECT_ID(N'dbo.Tryouts', N'U')              IS NOT NULL DROP TABLE dbo.Tryouts;
IF OBJECT_ID(N'dbo.Eventos', N'U')              IS NOT NULL DROP TABLE dbo.Eventos;
IF OBJECT_ID(N'dbo.Noticias', N'U')             IS NOT NULL DROP TABLE dbo.Noticias;
IF OBJECT_ID(N'dbo.Produtos', N'U')             IS NOT NULL DROP TABLE dbo.Produtos;
IF OBJECT_ID(N'dbo.Modalidades', N'U')          IS NOT NULL DROP TABLE dbo.Modalidades;
IF OBJECT_ID(N'dbo.Administradores', N'U')      IS NOT NULL DROP TABLE dbo.Administradores;
IF OBJECT_ID(N'dbo.Sugestoes', N'U')            IS NOT NULL DROP TABLE dbo.Sugestoes;
IF OBJECT_ID(N'dbo.Usuarios', N'U')             IS NOT NULL DROP TABLE dbo.Usuarios;
GO

/* ---------------------------------------------------------------------
   Ordem de criação: tabelas sem FK primeiro, depois as dependentes.
   --------------------------------------------------------------------- */

-- =====================================================================
-- Usuarios
-- =====================================================================
CREATE TABLE dbo.Usuarios
(
    Id              INT IDENTITY(1,1)   NOT NULL,
    Nome            NVARCHAR(150)       NOT NULL,
    Email           NVARCHAR(150)       NOT NULL,
    SenhaHash       NVARCHAR(256)       NOT NULL,
    RA              NVARCHAR(20)        NULL,
    Curso           NVARCHAR(100)       NULL,
    Telefone        NVARCHAR(20)        NULL,
    DataNascimento  DATE                NULL,
    DataCadastro    DATETIME2           NOT NULL CONSTRAINT DF_Usuarios_DataCadastro DEFAULT (GETDATE()),
    Ativo           BIT                 NOT NULL CONSTRAINT DF_Usuarios_Ativo DEFAULT (1),
    CONSTRAINT PK_Usuarios PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_Usuarios_Email UNIQUE (Email),
    CONSTRAINT UQ_Usuarios_RA UNIQUE (RA)
);
GO
CREATE NONCLUSTERED INDEX IX_Usuarios_Ativo ON dbo.Usuarios (Ativo);
GO

-- =====================================================================
-- Administradores
-- =====================================================================
CREATE TABLE dbo.Administradores
(
    Id              INT IDENTITY(1,1)   NOT NULL,
    Nome            NVARCHAR(150)       NOT NULL,
    Email           NVARCHAR(150)       NOT NULL,
    SenhaHash       NVARCHAR(256)       NOT NULL,
    Role            NVARCHAR(50)        NOT NULL CONSTRAINT DF_Administradores_Role DEFAULT ('Admin'),
    DataCriacao     DATETIME2           NOT NULL CONSTRAINT DF_Administradores_DataCriacao DEFAULT (GETDATE()),
    CONSTRAINT PK_Administradores PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_Administradores_Email UNIQUE (Email)
);
GO

-- =====================================================================
-- Modalidades
-- =====================================================================
CREATE TABLE dbo.Modalidades
(
    Id          INT IDENTITY(1,1)   NOT NULL,
    Nome        NVARCHAR(100)       NOT NULL,
    Descricao   NVARCHAR(500)       NULL,
    Categoria   NVARCHAR(50)        NULL,   -- ex: Esporte, EGames
    Tecnico     NVARCHAR(100)       NULL,
    ImagemUrl   NVARCHAR(500)       NULL,
    Ativo       BIT                 NOT NULL CONSTRAINT DF_Modalidades_Ativo DEFAULT (1),
    CONSTRAINT PK_Modalidades PRIMARY KEY CLUSTERED (Id)
);
GO
CREATE NONCLUSTERED INDEX IX_Modalidades_Categoria ON dbo.Modalidades (Categoria);
GO

-- =====================================================================
-- Noticias
-- =====================================================================
CREATE TABLE dbo.Noticias
(
    Id              INT IDENTITY(1,1)   NOT NULL,
    Titulo          NVARCHAR(200)       NULL,
    Resumo          NVARCHAR(500)       NULL,
    Conteudo        NVARCHAR(MAX)       NULL,
    ImagemUrl       NVARCHAR(500)       NULL,
    Categoria       NVARCHAR(50)        NULL,
    Destaque        BIT                 NOT NULL CONSTRAINT DF_Noticias_Destaque DEFAULT (0),
    Curtidas        INT                 NOT NULL CONSTRAINT DF_Noticias_Curtidas DEFAULT (0),
    DataPublicacao  DATETIME2           NOT NULL CONSTRAINT DF_Noticias_DataPublicacao DEFAULT (GETDATE()),
    AdministradorId INT                 NULL,
    CONSTRAINT PK_Noticias PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_Noticias_Administradores FOREIGN KEY (AdministradorId)
        REFERENCES dbo.Administradores (Id) ON DELETE SET NULL,
    CONSTRAINT CK_Noticias_Curtidas CHECK (Curtidas >= 0)
);
GO
CREATE NONCLUSTERED INDEX IX_Noticias_DataPublicacao ON dbo.Noticias (DataPublicacao DESC);
CREATE NONCLUSTERED INDEX IX_Noticias_Categoria ON dbo.Noticias (Categoria);
CREATE NONCLUSTERED INDEX IX_Noticias_AdministradorId ON dbo.Noticias (AdministradorId);
GO

-- =====================================================================
-- Eventos
-- =====================================================================
CREATE TABLE dbo.Eventos
(
    Id              INT IDENTITY(1,1)   NOT NULL,
    ModalidadeId    INT                 NOT NULL,
    Titulo          NVARCHAR(150)       NOT NULL,
    Descricao       NVARCHAR(500)       NULL,
    DataHora        DATETIME2           NOT NULL,
    Local           NVARCHAR(150)       NULL,
    TipoEvento      NVARCHAR(50)        NOT NULL,  -- Jogo, Treino, Campeonato
    CONSTRAINT PK_Eventos PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_Eventos_Modalidades FOREIGN KEY (ModalidadeId)
        REFERENCES dbo.Modalidades (Id) ON DELETE CASCADE,
    CONSTRAINT CK_Eventos_TipoEvento CHECK (TipoEvento IN (N'Jogo', N'Treino', N'Campeonato'))
);
GO
CREATE NONCLUSTERED INDEX IX_Eventos_DataHora ON dbo.Eventos (DataHora);
CREATE NONCLUSTERED INDEX IX_Eventos_ModalidadeId ON dbo.Eventos (ModalidadeId);
GO

-- =====================================================================
-- Tryouts
-- =====================================================================
CREATE TABLE dbo.Tryouts
(
    Id              INT IDENTITY(1,1)   NOT NULL,
    ModalidadeId    INT                 NOT NULL,
    NomeCandidato   NVARCHAR(150)       NOT NULL,
    Email           NVARCHAR(150)       NOT NULL,
    Telefone        NVARCHAR(20)        NULL,
    RA              NVARCHAR(20)        NULL,
    Curso           NVARCHAR(100)       NULL,
    Mensagem        NVARCHAR(1000)      NULL,
    DataInscricao   DATETIME2           NOT NULL CONSTRAINT DF_Tryouts_DataInscricao DEFAULT (GETDATE()),
    Status          NVARCHAR(30)        NOT NULL CONSTRAINT DF_Tryouts_Status DEFAULT (N'Pendente'),
    CONSTRAINT PK_Tryouts PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_Tryouts_Modalidades FOREIGN KEY (ModalidadeId)
        REFERENCES dbo.Modalidades (Id) ON DELETE NO ACTION,
    CONSTRAINT CK_Tryouts_Status CHECK (Status IN (N'Pendente', N'Aprovado', N'Reprovado'))
);
GO
CREATE NONCLUSTERED INDEX IX_Tryouts_Status ON dbo.Tryouts (Status);
CREATE NONCLUSTERED INDEX IX_Tryouts_ModalidadeId ON dbo.Tryouts (ModalidadeId);
GO

-- =====================================================================
-- Produtos
-- =====================================================================
CREATE TABLE dbo.Produtos
(
    Id          INT IDENTITY(1,1)   NOT NULL,
    Nome        NVARCHAR(150)       NOT NULL,
    Descricao   NVARCHAR(500)       NULL,
    Preco       DECIMAL(10,2)       NOT NULL,
    ImagemUrl   NVARCHAR(500)       NULL,
    Estoque     INT                 NOT NULL CONSTRAINT DF_Produtos_Estoque DEFAULT (0),
    Categoria   NVARCHAR(50)        NULL,
    Ativo       BIT                 NOT NULL CONSTRAINT DF_Produtos_Ativo DEFAULT (1),
    CONSTRAINT PK_Produtos PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT CK_Produtos_Preco CHECK (Preco >= 0),
    CONSTRAINT CK_Produtos_Estoque CHECK (Estoque >= 0)
);
GO
CREATE NONCLUSTERED INDEX IX_Produtos_Categoria ON dbo.Produtos (Categoria);
CREATE NONCLUSTERED INDEX IX_Produtos_Ativo ON dbo.Produtos (Ativo);
GO

-- =====================================================================
-- Pedidos
-- =====================================================================
CREATE TABLE dbo.Pedidos
(
    Id          INT IDENTITY(1,1)   NOT NULL,
    UsuarioId   INT                 NOT NULL,
    DataPedido  DATETIME2           NOT NULL CONSTRAINT DF_Pedidos_DataPedido DEFAULT (GETDATE()),
    Status      NVARCHAR(30)        NOT NULL CONSTRAINT DF_Pedidos_Status DEFAULT (N'Pendente'),
    ValorTotal  DECIMAL(10,2)       NOT NULL CONSTRAINT DF_Pedidos_ValorTotal DEFAULT (0),
    CONSTRAINT PK_Pedidos PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_Pedidos_Usuarios FOREIGN KEY (UsuarioId)
        REFERENCES dbo.Usuarios (Id) ON DELETE NO ACTION,
    CONSTRAINT CK_Pedidos_Status CHECK (Status IN (N'Pendente', N'Pago', N'Enviado', N'Cancelado')),
    CONSTRAINT CK_Pedidos_ValorTotal CHECK (ValorTotal >= 0)
);
GO
CREATE NONCLUSTERED INDEX IX_Pedidos_UsuarioId ON dbo.Pedidos (UsuarioId);
CREATE NONCLUSTERED INDEX IX_Pedidos_Status ON dbo.Pedidos (Status);
CREATE NONCLUSTERED INDEX IX_Pedidos_DataPedido ON dbo.Pedidos (DataPedido DESC);
GO

-- =====================================================================
-- ItensPedido
-- =====================================================================
CREATE TABLE dbo.ItensPedido
(
    Id              INT IDENTITY(1,1)   NOT NULL,
    PedidoId        INT                 NOT NULL,
    ProdutoId       INT                 NOT NULL,
    Quantidade      INT                 NOT NULL,
    PrecoUnitario   DECIMAL(10,2)       NOT NULL,
    CONSTRAINT PK_ItensPedido PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_ItensPedido_Pedidos FOREIGN KEY (PedidoId)
        REFERENCES dbo.Pedidos (Id) ON DELETE CASCADE,
    CONSTRAINT FK_ItensPedido_Produtos FOREIGN KEY (ProdutoId)
        REFERENCES dbo.Produtos (Id) ON DELETE NO ACTION,
    CONSTRAINT CK_ItensPedido_Quantidade CHECK (Quantidade > 0),
    CONSTRAINT CK_ItensPedido_PrecoUnitario CHECK (PrecoUnitario >= 0)
);
GO
CREATE NONCLUSTERED INDEX IX_ItensPedido_PedidoId ON dbo.ItensPedido (PedidoId);
CREATE NONCLUSTERED INDEX IX_ItensPedido_ProdutoId ON dbo.ItensPedido (ProdutoId);
GO

-- =====================================================================
-- CarteirinhasDigitais
-- =====================================================================
CREATE TABLE dbo.CarteirinhasDigitais
(
    Id                  INT IDENTITY(1,1)   NOT NULL,
    UsuarioId           INT                 NOT NULL,
    NumeroCarteirinha   NVARCHAR(30)        NOT NULL,
    DataEmissao         DATETIME2           NOT NULL CONSTRAINT DF_CarteirinhasDigitais_DataEmissao DEFAULT (GETDATE()),
    DataValidade        DATE                NOT NULL,
    Status              NVARCHAR(30)        NOT NULL CONSTRAINT DF_CarteirinhasDigitais_Status DEFAULT (N'Ativa'),
    CONSTRAINT PK_CarteirinhasDigitais PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_CarteirinhasDigitais_Usuarios FOREIGN KEY (UsuarioId)
        REFERENCES dbo.Usuarios (Id) ON DELETE CASCADE,
    CONSTRAINT UQ_CarteirinhasDigitais_UsuarioId UNIQUE (UsuarioId),
    CONSTRAINT UQ_CarteirinhasDigitais_NumeroCarteirinha UNIQUE (NumeroCarteirinha),
    CONSTRAINT CK_CarteirinhasDigitais_Status CHECK (Status IN (N'Ativa', N'Vencida', N'Suspensa'))
);
GO
CREATE NONCLUSTERED INDEX IX_CarteirinhasDigitais_Status ON dbo.CarteirinhasDigitais (Status);
GO

-- =====================================================================
-- Sugestoes
-- =====================================================================
CREATE TABLE dbo.Sugestoes
(
    Id              INT IDENTITY(1,1)   NOT NULL,
    NomeAutor       NVARCHAR(150)       NULL,   -- pode ser anônimo
    Email           NVARCHAR(150)       NULL,
    Categoria       NVARCHAR(50)        NULL,
    Mensagem        NVARCHAR(1000)      NOT NULL,
    DataEnvio       DATETIME2           NOT NULL CONSTRAINT DF_Sugestoes_DataEnvio DEFAULT (GETDATE()),
    Status          NVARCHAR(30)        NOT NULL CONSTRAINT DF_Sugestoes_Status DEFAULT (N'Recebida'),
    RespostaAdmin   NVARCHAR(1000)      NULL,
    CONSTRAINT PK_Sugestoes PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT CK_Sugestoes_Status CHECK (Status IN (N'Recebida', N'EmAnalise', N'Respondida'))
);
GO
CREATE NONCLUSTERED INDEX IX_Sugestoes_Status ON dbo.Sugestoes (Status);
CREATE NONCLUSTERED INDEX IX_Sugestoes_DataEnvio ON dbo.Sugestoes (DataEnvio DESC);
GO

PRINT 'Script 02_create_tables.sql concluído: todas as tabelas, constraints e índices foram criados.';
GO
