# Deploy — Fênix UNIP

Este documento explica como configurar back-end e banco por variável, para não precisar mexer em código ao subir em outro servidor.

## Front-end

O front-end vive em um repositório separado: [`GustavoGarbim/fenix-unip`](https://github.com/GustavoGarbim/fenix-unip), hospedado no Vercel. Este repositório (`fenix-unip-back`) contém apenas o back-end e os scripts de banco de dados.

Toda chamada à API do front passa por `src/services/api.js`, que lê a URL base de uma única variável:

```
VITE_API_URL=http://localhost:5004/api
```

- Local: arquivo `.env` (ignorado no git) no repositório do front.
- Produção: defina `VITE_API_URL` nas env vars do Vercel apontando para a URL pública deste backend, ex: `VITE_API_URL=https://api.fenixunip.com.br/api`.

**Importante (Vite):** variáveis `VITE_*` são embutidas no bundle **no momento do build**, não em runtime. Ou seja, ao trocar `VITE_API_URL` em produção é preciso rodar o build de novo (o Vercel já faz isso automaticamente a cada deploy).

## Back-end (`fenix-unip-back/`)

O ASP.NET Core já mescla `appsettings.json` com variáveis de ambiente automaticamente (variável de ambiente sempre sobrescreve o valor do JSON, sem precisar rebuild — só reiniciar o processo). Os pontos configuráveis:

| O que | Chave no appsettings.json | Variável de ambiente equivalente |
|---|---|---|
| Conexão com o banco | `ConnectionStrings:DefaultConnection` | `ConnectionStrings__DefaultConnection` |
| Chave JWT | `Jwt:Key` | `Jwt__Key` |
| Emissor JWT | `Jwt:Issuer` | `Jwt__Issuer` |
| Audiência JWT | `Jwt:Audience` | `Jwt__Audience` |
| Origens liberadas no CORS (separadas por vírgula) | `Cors:AllowedOrigins` | `Cors__AllowedOrigins` |

**Banco de dados:** o backend usa o provider MySQL (Pomelo) via EF Core, compatível com TiDB Cloud. Formato da connection string:
```
Server=<host>;Port=<porta>;Database=<banco>;User=<usuario>;Password=<senha>;SslMode=Required;
```

Exemplo (Linux/produção):
```bash
export ConnectionStrings__DefaultConnection="Server=gateway01.sa-east-1.prod.aws.tidbcloud.com;Port=4000;Database=fenix_unip;User=usuario;Password=senha;SslMode=Required;"
export Cors__AllowedOrigins="https://fenixunip.com.br,https://www.fenixunip.com.br"
export Jwt__Key="uma-chave-bem-longa-e-secreta-de-producao"
```

No IIS/Windows Server, as mesmas variáveis podem ser definidas em "Configuration Editor" do site ou no `web.config` (`<environmentVariables>`), ou via variáveis de ambiente do serviço/App Pool. Em Docker, via `docker run -e` ou `docker-compose.yml`.

`Cors:AllowedOrigins` aceita múltiplas origens separadas por vírgula — assim é só adicionar o domínio real do front (ex.: `https://fenixunip.com.br`) sem precisar recompilar o backend.

## Banco de dados

O schema é criado e versionado via **migrations do EF Core** (pasta `Migrations/` do backend) — basta rodar `dotnet ef database update` apontando `ConnectionStrings__DefaultConnection` para o servidor MySQL/TiDB de destino.

Os scripts T-SQL em `database/` foram escritos originalmente para SQL Server e ficaram desatualizados depois da migração para MySQL/TiDB; hoje servem só de referência histórica do schema, não use para provisionar o banco.

## Deploy no Render

O Render não tem runtime nativo para .NET — o serviço precisa ser criado como **Docker** apontando para o `Dockerfile` na raiz deste repositório:

- **Dockerfile Path:** `Dockerfile`
- **Docker Build Context Directory:** `.` (raiz do repositório)

O Render injeta a variável `PORT` em runtime e o container já está preparado para escutar nela (ver `ENTRYPOINT` do `Dockerfile`). Configure as demais variáveis de ambiente (tabela acima) na aba **Environment** do serviço no Render.

## Resumo do fluxo de deploy

1. Crie o banco vazio no MySQL/TiDB e rode `dotnet ef database update` para aplicar as migrations.
2. Crie o serviço Docker no Render apontando para o `Dockerfile` e defina as variáveis de ambiente da tabela acima.
3. No repositório `fenix-unip` (front, Vercel), defina `VITE_API_URL` apontando para a URL pública deste backend (ex.: `https://fenix-unip-back.onrender.com/api`).
