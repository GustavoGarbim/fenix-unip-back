# Banco de dados — Atlética Fênix UNIP

Scripts T-SQL para criação manual do banco `FenixUnipDB` via **SSMS (SQL Server
Management Studio)**. Servem como documentação executável do schema definido
no contrato compartilhado (`CONTRATO_API.md`) e como forma de subir o banco
rapidamente em um ambiente novo sem depender do backend.

## Versão alvo

SQL Server **2019 ou superior** (Express, Developer ou Standard). Os scripts
usam apenas tipos e recursos T-SQL padrão (sem Always Encrypted, sem recursos
exclusivos de edições específicas), então também funcionam em instâncias mais
recentes (2022) sem alterações.

## Ordem de execução no SSMS

Conecte-se à sua instância local (ex: `localhost\SQLEXPRESS`) e execute os
scripts nesta ordem, um de cada vez:

1. **`01_create_database.sql`** — cria o banco `FenixUnipDB` (idempotente,
   usa `IF NOT EXISTS`). Execute com o contexto em `master`.
2. **`02_create_tables.sql`** — cria todas as 11 tabelas do domínio
   (Usuarios, Administradores, Noticias, Modalidades, Eventos, Tryouts,
   Produtos, Pedidos, ItensPedido, CarteirinhasDigitais, Sugestoes), com
   chaves primárias/estrangeiras, constraints `UNIQUE`, `CHECK`, valores
   `DEFAULT` e índices não-clusterizados. Este script derruba e recria as
   tabelas (`DROP TABLE IF EXISTS`) — não execute em um banco com dados
   reais sem antes fazer backup.
3. **`03_seed_data.sql`** — popula o banco com dados de exemplo realistas
   (1 administrador, 2 usuários/alunos, 4 modalidades, eventos futuros,
   5 produtos de loja, 3 notícias, 2 tryouts e 2 sugestões). É idempotente:
   cada `INSERT` é protegido por `IF NOT EXISTS`, então pode ser reexecutado
   sem duplicar registros.
4. **`04_stored_procedures.sql`** *(opcional, mas recomendado)* — cria as
   procedures `sp_AprovarTryout`, `sp_RelatorioVendasProdutos` e
   `sp_ResponderSugestao`, exemplos de operações de negócio comuns.

Basta abrir cada arquivo no SSMS (`Arquivo > Abrir > Arquivo...`) e executar
com `F5`, na ordem acima.

## Credenciais de exemplo (seed)

- **Administrador:** `admin@fenixunip.com.br` — senha `admin123`
- **Usuários:** `joao.silva@aluno.unip.br` e `maria.souza@aluno.unip.br` —
  senha `aluno123`

Os hashes BCrypt inseridos no script de seed são **placeholders ilustrativos**
(comentados no próprio script) — eles têm o formato correto de um hash BCrypt,
mas não foram validados como correspondendo exatamente a essas senhas em
tempo de execução. **Antes de usar em qualquer ambiente real de testes**,
gere hashes reais com `BCrypt.Net-Next` (o mesmo pacote usado pelo backend)
para as senhas desejadas e substitua os valores em `03_seed_data.sql`, por
exemplo:

```csharp
string hash = BCrypt.Net.BCrypt.HashPassword("admin123");
```

## Relação com o backend (.NET 8 + EF Core)

O backend (`fenix-unip-back`) usa **EF Core Code-First**: os `Models/` em C#
mapeiam 1:1 para estas mesmas tabelas, e o schema também pode ser criado ou
atualizado via **EF Core Migrations** (`dotnet ef database update`), sem
necessidade de rodar estes scripts manualmente.

Os dois caminhos devem produzir o **mesmo schema lógico** (mesmos nomes de
tabelas/colunas/tipos, conforme o contrato). Use um ou outro conforme o
cenário:

- **Via SSMS (estes scripts):** setup rápido e manual, útil para quem só quer
  o banco pronto (ex.: para inspecionar dados, rodar queries ad-hoc, ou como
  ambiente de referência), sem precisar compilar/rodar o backend primeiro.
- **Via EF Core Migrations:** fluxo recomendado durante o desenvolvimento do
  backend, pois mantém o histórico de alterações do schema versionado junto
  com o código C#.

**Importante:** não rode os dois caminhos "misturados" contra o mesmo banco
sem cuidado — se o backend já tiver aplicado migrations em `FenixUnipDB`, o
script `02_create_tables.sql` (que faz `DROP TABLE` antes de recriar) vai
apagar os dados existentes. Prefira usar este `database/` scripts em um banco
limpo, ou como referência/documentação, e deixar o EF Core Migrations como
fonte da verdade para o schema em ambientes de desenvolvimento contínuo do
backend.

## Connection string de referência

```
Server=localhost\SQLEXPRESS;Database=FenixUnipDB;Trusted_Connection=True;TrustServerCertificate=True;
```

Ajuste `localhost\SQLEXPRESS` para o nome da sua instância local do SQL
Server (ex.: apenas `localhost`, `.\SQLEXPRESS`, ou o nome de uma instância
nomeada customizada).

## Estrutura dos arquivos

```
database/
├── 01_create_database.sql      -- cria o banco FenixUnipDB
├── 02_create_tables.sql        -- cria tabelas, constraints e índices
├── 03_seed_data.sql            -- dados de exemplo (idempotente)
├── 04_stored_procedures.sql    -- procedures de exemplo
└── README.md                   -- este arquivo
```
