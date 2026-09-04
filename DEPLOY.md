# Deploy — Fênix UNIP

Este documento explica como configurar front-end, back-end e banco por variável, para não precisar mexer em código ao subir em outro servidor.

## Front-end (`fenix-unip/`)

Toda chamada à API passa por `src/services/api.js`, que lê a URL base de uma única variável:

```
VITE_API_URL=http://localhost:5004/api
```

- Local: arquivo `.env` (já criado, ignorado no git).
- Produção: crie `.env.production` (ou defina `VITE_API_URL` nas env vars do serviço de hospedagem — Vercel, Netlify, Azure Static Web Apps, etc.) apontando para a URL pública do backend, ex: `VITE_API_URL=https://api.fenixunip.com.br/api`.

**Importante (Vite):** variáveis `VITE_*` são embutidas no bundle **no momento do build**, não em runtime. Ou seja, ao trocar `VITE_API_URL` em produção é preciso rodar `npm run build` de novo (não basta reiniciar o servidor estático). Se quiser trocar a URL sem rebuild, seria necessário servir um `config.js` externo lido em runtime — não implementado agora por não ter sido pedido, mas é possível migrar para isso depois se precisar.

## Back-end (`fenix-unip-back/`)

O ASP.NET Core já mescla `appsettings.json` com variáveis de ambiente automaticamente (variável de ambiente sempre sobrescreve o valor do JSON, sem precisar rebuild — só reiniciar o processo). Os pontos configuráveis:

| O que | Chave no appsettings.json | Variável de ambiente equivalente |
|---|---|---|
| Conexão com o banco | `ConnectionStrings:DefaultConnection` | `ConnectionStrings__DefaultConnection` |
| Chave JWT | `Jwt:Key` | `Jwt__Key` |
| Emissor JWT | `Jwt:Issuer` | `Jwt__Issuer` |
| Audiência JWT | `Jwt:Audience` | `Jwt__Audience` |
| Origens liberadas no CORS (separadas por vírgula) | `Cors:AllowedOrigins` | `Cors__AllowedOrigins` |

Exemplo (Linux/produção):
```bash
export ConnectionStrings__DefaultConnection="Server=meu-servidor-sql;Database=FenixUnipDB;User Id=usuario;Password=senha;TrustServerCertificate=True;"
export Cors__AllowedOrigins="https://fenixunip.com.br,https://www.fenixunip.com.br"
export Jwt__Key="uma-chave-bem-longa-e-secreta-de-producao"
```

No IIS/Windows Server, as mesmas variáveis podem ser definidas em "Configuration Editor" do site ou no `web.config` (`<environmentVariables>`), ou via variáveis de ambiente do serviço/App Pool. Em Docker, via `docker run -e` ou `docker-compose.yml`.

`Cors:AllowedOrigins` aceita múltiplas origens separadas por vírgula — assim é só adicionar o domínio real do front (ex.: `https://fenixunip.com.br`) sem precisar recompilar o backend.

## Banco de dados

Os scripts em `database/` usam nome fixo de banco (`FenixUnipDB`), mas o **endereço/servidor** é definido só pela connection string do backend (`ConnectionStrings__DefaultConnection`), então trocar de ambiente (dev → homologação → produção) é só apontar essa variável para o servidor SQL correto, sem alterar os scripts.

## Resumo do fluxo de deploy

1. Suba o SQL Server e rode os scripts de `database/` (ou `dotnet ef database update`).
2. Publique o backend (`dotnet publish`) e defina as variáveis de ambiente da tabela acima no servidor de destino.
3. Defina `VITE_API_URL` apontando para a URL pública do backend e rode `npm run build` do front; publique o conteúdo de `dist/`.
