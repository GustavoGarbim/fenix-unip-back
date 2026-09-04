/* =====================================================================
   Fenix UNIP - Atlética
   Script 03 - Dados de exemplo (seed)
   Alvo: SQL Server 2019+ (Express/Developer/Standard) via SSMS
   =====================================================================
   Execução: rodar após 02_create_tables.sql.
   Este script é idempotente: cada bloco só insere se os dados ainda
   não existirem (checagem por chave natural), podendo ser reexecutado
   com segurança.
   ===================================================================== */

USE FenixUnipDB;
GO

-- =====================================================================
-- Administradores
-- Senha de exemplo: "admin123"
-- Hash BCrypt real (custo 11), gerado com BCrypt.Net-Next para a string
-- "admin123" e verificado com BCrypt.Verify antes de entrar neste script.
-- Ao subir para produção, gere um hash novo e troque este valor.
-- =====================================================================
IF NOT EXISTS (SELECT 1 FROM dbo.Administradores WHERE Email = N'admin@fenixunip.com.br')
BEGIN
    INSERT INTO dbo.Administradores (Nome, Email, SenhaHash, Role)
    VALUES
    (N'Administrador Geral', N'admin@fenixunip.com.br',
     N'$2a$11$buDq0UcffSd19Q9UznJGq.UYl5vU60tzlzstqXMb52G0TIb1UPJtC', -- BCrypt real de "admin123"
     N'Admin');
END
GO

-- =====================================================================
-- Usuarios (alunos de exemplo)
-- Senha de todos os usuários de exemplo: "aluno123"
-- Hash BCrypt real (custo 11), gerado e verificado da mesma forma acima.
-- =====================================================================
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE Email = N'joao.silva@aluno.unip.br')
BEGIN
    INSERT INTO dbo.Usuarios (Nome, Email, SenhaHash, RA, Curso, Telefone, DataNascimento, Ativo)
    VALUES
    (N'João Silva', N'joao.silva@aluno.unip.br',
     N'$2a$11$US50l0NN7sfQ1edrE0Z1ZunvNHYyacjll2b82a2SXsyuhpV66A.hG', -- BCrypt real de "aluno123"
     N'A1234B-0', N'Ciência da Computação', N'(11) 91234-5678', '2002-05-14', 1);
END

IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE Email = N'maria.souza@aluno.unip.br')
BEGIN
    INSERT INTO dbo.Usuarios (Nome, Email, SenhaHash, RA, Curso, Telefone, DataNascimento, Ativo)
    VALUES
    (N'Maria Souza', N'maria.souza@aluno.unip.br',
     N'$2a$11$US50l0NN7sfQ1edrE0Z1ZunvNHYyacjll2b82a2SXsyuhpV66A.hG',
     N'A5678C-1', N'Educação Física', N'(11) 99876-5432', '2003-09-22', 1);
END
GO

-- =====================================================================
-- Modalidades
-- =====================================================================
IF NOT EXISTS (SELECT 1 FROM dbo.Modalidades WHERE Nome = N'Futsal')
BEGIN
    INSERT INTO dbo.Modalidades (Nome, Descricao, Categoria, Tecnico, ImagemUrl, Ativo)
    VALUES (N'Futsal', N'Time de futsal masculino e feminino da Atlética Fênix, disputando campeonatos universitários regionais.', N'Esporte', N'Carlos Mendes', N'/img/modalidades/futsal.jpg', 1);
END

IF NOT EXISTS (SELECT 1 FROM dbo.Modalidades WHERE Nome = N'Vôlei')
BEGIN
    INSERT INTO dbo.Modalidades (Nome, Descricao, Categoria, Tecnico, ImagemUrl, Ativo)
    VALUES (N'Vôlei', N'Equipe de vôlei de quadra, treinos às terças e quintas no ginásio da UNIP.', N'Esporte', N'Fernanda Lima', N'/img/modalidades/volei.jpg', 1);
END

IF NOT EXISTS (SELECT 1 FROM dbo.Modalidades WHERE Nome = N'E-Games')
BEGIN
    INSERT INTO dbo.Modalidades (Nome, Descricao, Categoria, Tecnico, ImagemUrl, Ativo)
    VALUES (N'E-Games', N'Equipe de e-sports competindo em League of Legends e Valorant em torneios universitários.', N'EGames', N'Rafael Torres', N'/img/modalidades/egames.jpg', 1);
END

IF NOT EXISTS (SELECT 1 FROM dbo.Modalidades WHERE Nome = N'Handebol')
BEGIN
    INSERT INTO dbo.Modalidades (Nome, Descricao, Categoria, Tecnico, ImagemUrl, Ativo)
    VALUES (N'Handebol', N'Time de handebol misto, aberto a novos integrantes todo início de semestre.', N'Esporte', N'Patrícia Alves', N'/img/modalidades/handebol.jpg', 1);
END
GO

-- =====================================================================
-- Eventos (futuros, relativos à data de execução do script)
-- =====================================================================
IF NOT EXISTS (SELECT 1 FROM dbo.Eventos WHERE Titulo = N'Jogo amistoso de Futsal x UNIB')
BEGIN
    INSERT INTO dbo.Eventos (ModalidadeId, Titulo, Descricao, DataHora, Local, TipoEvento)
    SELECT m.Id, N'Jogo amistoso de Futsal x UNIB', N'Amistoso preparatório para o campeonato regional.',
           DATEADD(DAY, 10, CAST(GETDATE() AS DATE)), N'Ginásio Poliesportivo UNIP - Campus Norte', N'Jogo'
    FROM dbo.Modalidades m WHERE m.Nome = N'Futsal';
END

IF NOT EXISTS (SELECT 1 FROM dbo.Eventos WHERE Titulo = N'Treino aberto de Vôlei')
BEGIN
    INSERT INTO dbo.Eventos (ModalidadeId, Titulo, Descricao, DataHora, Local, TipoEvento)
    SELECT m.Id, N'Treino aberto de Vôlei', N'Treino aberto para calouros interessados em participar do time.',
           DATEADD(DAY, 5, CAST(GETDATE() AS DATE)), N'Quadra Coberta - Campus Norte', N'Treino'
    FROM dbo.Modalidades m WHERE m.Nome = N'Vôlei';
END

IF NOT EXISTS (SELECT 1 FROM dbo.Eventos WHERE Titulo = N'Campeonato Universitário de E-Games')
BEGIN
    INSERT INTO dbo.Eventos (ModalidadeId, Titulo, Descricao, DataHora, Local, TipoEvento)
    SELECT m.Id, N'Campeonato Universitário de E-Games', N'Etapa classificatória de Valorant contra outras atléticas.',
           DATEADD(DAY, 20, CAST(GETDATE() AS DATE)), N'Laboratório de Games - Campus Norte', N'Campeonato'
    FROM dbo.Modalidades m WHERE m.Nome = N'E-Games';
END
GO

-- =====================================================================
-- Produtos (loja da atlética)
-- =====================================================================
IF NOT EXISTS (SELECT 1 FROM dbo.Produtos WHERE Nome = N'Camisa Oficial Fênix 2026')
BEGIN
    INSERT INTO dbo.Produtos (Nome, Descricao, Preco, ImagemUrl, Estoque, Categoria, Ativo)
    VALUES (N'Camisa Oficial Fênix 2026', N'Camisa oficial da Atlética Fênix, tecido dry-fit, coleção 2026.', 89.90, N'/img/loja/camisa-oficial.jpg', 50, N'Vestuário', 1);
END

IF NOT EXISTS (SELECT 1 FROM dbo.Produtos WHERE Nome = N'Moletom Fênix Preto')
BEGIN
    INSERT INTO dbo.Produtos (Nome, Descricao, Preco, ImagemUrl, Estoque, Categoria, Ativo)
    VALUES (N'Moletom Fênix Preto', N'Moletom canguru com capuz, logo bordado da Atlética Fênix.', 139.90, N'/img/loja/moletom-preto.jpg', 30, N'Vestuário', 1);
END

IF NOT EXISTS (SELECT 1 FROM dbo.Produtos WHERE Nome = N'Boné Fênix Aba Curva')
BEGIN
    INSERT INTO dbo.Produtos (Nome, Descricao, Preco, ImagemUrl, Estoque, Categoria, Ativo)
    VALUES (N'Boné Fênix Aba Curva', N'Boné ajustável com bordado da Atlética Fênix.', 49.90, N'/img/loja/bone.jpg', 40, N'Acessório', 1);
END

IF NOT EXISTS (SELECT 1 FROM dbo.Produtos WHERE Nome = N'Camisa de Jogo Futsal')
BEGIN
    INSERT INTO dbo.Produtos (Nome, Descricao, Preco, ImagemUrl, Estoque, Categoria, Ativo)
    VALUES (N'Camisa de Jogo Futsal', N'Camisa de jogo oficial do time de Futsal, numerada.', 99.90, N'/img/loja/camisa-futsal.jpg', 25, N'Vestuário', 1);
END

IF NOT EXISTS (SELECT 1 FROM dbo.Produtos WHERE Nome = N'Squeeze Fênix 700ml')
BEGIN
    INSERT INTO dbo.Produtos (Nome, Descricao, Preco, ImagemUrl, Estoque, Categoria, Ativo)
    VALUES (N'Squeeze Fênix 700ml', N'Garrafa squeeze de plástico resistente com logo da atlética.', 29.90, N'/img/loja/squeeze.jpg', 60, N'Acessório', 1);
END
GO

-- =====================================================================
-- Noticias
-- =====================================================================
IF NOT EXISTS (SELECT 1 FROM dbo.Noticias WHERE Titulo = N'Fênix conquista vice-campeonato regional de Futsal')
BEGIN
    INSERT INTO dbo.Noticias (Titulo, Resumo, Conteudo, ImagemUrl, Categoria, Destaque, Curtidas, DataPublicacao, AdministradorId)
    SELECT N'Fênix conquista vice-campeonato regional de Futsal',
           N'Time masculino chega à final após campanha invicta na fase de grupos.',
           N'A equipe de futsal da Atlética Fênix encerrou sua participação no campeonato regional universitário como vice-campeã, após uma campanha de destaque na fase de grupos, mantendo-se invicta até a final. O time agradece o apoio da torcida presente no ginásio.',
           N'/img/noticias/futsal-vice.jpg', N'Esportes', 1, 42, DATEADD(DAY, -7, GETDATE()), a.Id
    FROM dbo.Administradores a WHERE a.Email = N'admin@fenixunip.com.br';
END

IF NOT EXISTS (SELECT 1 FROM dbo.Noticias WHERE Titulo = N'Abertas as inscrições para novos integrantes 2026')
BEGIN
    INSERT INTO dbo.Noticias (Titulo, Resumo, Conteudo, ImagemUrl, Categoria, Destaque, Curtidas, DataPublicacao, AdministradorId)
    SELECT N'Abertas as inscrições para novos integrantes 2026',
           N'Calouros e veteranos já podem se inscrever para os testes seletivos de todas as modalidades.',
           N'A Atlética Fênix abriu as inscrições para testes seletivos (tryouts) de todas as suas modalidades esportivas e de e-games para o ano de 2026. Interessados podem se inscrever diretamente pelo portal, informando a modalidade desejada.',
           N'/img/noticias/inscricoes-abertas.jpg', N'Institucional', 1, 18, DATEADD(DAY, -3, GETDATE()), a.Id
    FROM dbo.Administradores a WHERE a.Email = N'admin@fenixunip.com.br';
END

IF NOT EXISTS (SELECT 1 FROM dbo.Noticias WHERE Titulo = N'Nova coleção de produtos da loja já disponível')
BEGIN
    INSERT INTO dbo.Noticias (Titulo, Resumo, Conteudo, ImagemUrl, Categoria, Destaque, Curtidas, DataPublicacao, AdministradorId)
    SELECT N'Nova coleção de produtos da loja já disponível',
           N'Camisas, moletons e bonés com o novo design chegaram à loja oficial.',
           N'A loja oficial da Atlética Fênix renovou seu catálogo com novos produtos, incluindo camisas, moletons e bonés com o design atualizado para 2026. Confira os itens disponíveis na aba Loja do portal.',
           N'/img/noticias/nova-colecao.jpg', N'Loja', 0, 9, DATEADD(DAY, -1, GETDATE()), a.Id
    FROM dbo.Administradores a WHERE a.Email = N'admin@fenixunip.com.br';
END
GO

-- =====================================================================
-- Tryouts (exemplos de inscrições)
-- =====================================================================
IF NOT EXISTS (SELECT 1 FROM dbo.Tryouts WHERE Email = N'lucas.pereira@email.com')
BEGIN
    INSERT INTO dbo.Tryouts (ModalidadeId, NomeCandidato, Email, Telefone, RA, Curso, Mensagem, Status)
    SELECT m.Id, N'Lucas Pereira', N'lucas.pereira@email.com', N'(11) 98888-1122', N'A9012D-2', N'Engenharia de Software',
           N'Jogo futsal há 5 anos e gostaria de participar do time da atlética.', N'Pendente'
    FROM dbo.Modalidades m WHERE m.Nome = N'Futsal';
END

IF NOT EXISTS (SELECT 1 FROM dbo.Tryouts WHERE Email = N'ana.costa@email.com')
BEGIN
    INSERT INTO dbo.Tryouts (ModalidadeId, NomeCandidato, Email, Telefone, RA, Curso, Mensagem, Status)
    SELECT m.Id, N'Ana Costa', N'ana.costa@email.com', N'(11) 97777-3344', N'A3456E-3', N'Administração',
           N'Tenho experiência com Valorant em nível competitivo e quero representar a faculdade.', N'Aprovado'
    FROM dbo.Modalidades m WHERE m.Nome = N'E-Games';
END
GO

-- =====================================================================
-- CarteirinhasDigitais (para os usuários de exemplo)
-- =====================================================================
IF NOT EXISTS (SELECT 1 FROM dbo.CarteirinhasDigitais c
               JOIN dbo.Usuarios u ON u.Id = c.UsuarioId
               WHERE u.Email = N'joao.silva@aluno.unip.br')
BEGIN
    INSERT INTO dbo.CarteirinhasDigitais (UsuarioId, NumeroCarteirinha, DataValidade, Status)
    SELECT u.Id, N'FENIX-2026-000001', DATEADD(YEAR, 1, CAST(GETDATE() AS DATE)), N'Ativa'
    FROM dbo.Usuarios u WHERE u.Email = N'joao.silva@aluno.unip.br';
END

IF NOT EXISTS (SELECT 1 FROM dbo.CarteirinhasDigitais c
               JOIN dbo.Usuarios u ON u.Id = c.UsuarioId
               WHERE u.Email = N'maria.souza@aluno.unip.br')
BEGIN
    INSERT INTO dbo.CarteirinhasDigitais (UsuarioId, NumeroCarteirinha, DataValidade, Status)
    SELECT u.Id, N'FENIX-2026-000002', DATEADD(YEAR, 1, CAST(GETDATE() AS DATE)), N'Ativa'
    FROM dbo.Usuarios u WHERE u.Email = N'maria.souza@aluno.unip.br';
END
GO

-- =====================================================================
-- Sugestoes (exemplos)
-- =====================================================================
IF NOT EXISTS (SELECT 1 FROM dbo.Sugestoes WHERE Mensagem = N'Poderiam organizar mais eventos de integração entre as modalidades?')
BEGIN
    INSERT INTO dbo.Sugestoes (NomeAutor, Email, Categoria, Mensagem, Status, RespostaAdmin)
    VALUES (N'Pedro Henrique', N'pedro.henrique@aluno.unip.br', N'Eventos',
            N'Poderiam organizar mais eventos de integração entre as modalidades?', N'Respondida',
            N'Ótima sugestão, Pedro! Já estamos planejando um evento de integração para o próximo semestre.');
END

IF NOT EXISTS (SELECT 1 FROM dbo.Sugestoes WHERE Mensagem = N'Seria bom ter mais opções de tamanho nas camisas da loja.')
BEGIN
    INSERT INTO dbo.Sugestoes (NomeAutor, Email, Categoria, Mensagem, Status, RespostaAdmin)
    VALUES (NULL, NULL, N'Loja',
            N'Seria bom ter mais opções de tamanho nas camisas da loja.', N'Recebida', NULL);
END
GO

PRINT 'Script 03_seed_data.sql concluído: dados de exemplo inseridos (quando ainda não existentes).';
GO
