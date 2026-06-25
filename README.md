# Revita - Back-End (API)

Este é o projeto Back-End do **Sistema Revita**, uma API RESTful desenvolvida em **.NET 10 (C#)**.
A API é responsável por fornecer os dados e aplicar as regras de negócio do sistema de fidelidade e acompanhamento de compras focado em parcerias entre lojas de suplementos/produtos naturais e parceiros.

## 🚀 Tecnologias e Arquitetura

- **Framework:** .NET 10 (C#)
- **Banco de Dados:** PostgreSQL
- **Autenticação e Autorização:** JWT (JSON Web Tokens) com *Role-Based Access Control* (RBAC).
- **ORM (Sugerido):** Entity Framework Core

## 🔐 Controle de Acesso e Segurança (RBAC)

A API protege seus endpoints com base no perfil do usuário autenticado:

1. **Administrador:** Acesso total (configuração de regras de pontuação, resgate de bônus, relatórios, gestão de usuários).
2. **Funcionário da Loja (Operacional):** Lançamento de compras, cadastro de clientes e parceiros.
3. **Parceiro:** Acesso ao dashboard de saldo, extrato de pontos e clientes vinculados, edição do próprio perfil.
4. **Cliente (Consumidor Final):** Acesso restrito ao próprio extrato de compras e visualização do parceiro vinculado.

## ⚙️ Regras de Negócio e Requisitos Técnicos

- **Motor de Cálculo de Pontos:** A API deve parametrizar e calcular os pontos (ex: a cada R$ 1.000 em compras = 100 pontos; a cada 100 pontos = R$ 50 para resgate). Somente parceiros ganham bônus.
- **Integridade de Dados:** O banco PostgreSQL possui integridade referencial estrita para não permitir a exclusão de clientes ou parceiros com histórico de compras.
- **Auditoria / Ledger:** Todas as movimentações de pontos (geração e resgate) devem ser registradas de forma imutável (rastreabilidade total).

## 🛠️ Como Executar o Projeto Localmente

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/) (Local ou Docker)

### Instalação e Execução

1. Navegue até a pasta do back-end:

   ```bash
   cd projects/back-end
   ```

2. Restaure as dependências:

   ```bash
   dotnet restore
   ```

3. Configure a *Connection String* do banco de dados no arquivo `appsettings.Development.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=RevitaDB;Username=postgres;Password=sua_senha"
   }
   ```

4. Aplique as migrações no banco de dados (se utilizar EF Core):

   ```bash
   dotnet ef database update
   ```

5. Execute a API:

   ```bash
   dotnet run
   ```

6. Acesse o Swagger para documentação e teste dos endpoints. A URL será informada no console após a execução (geralmente `https://localhost:<porta>/swagger`).

## 📦 Estrutura de Domínios Prevista

A arquitetura da API suportará os seguintes domínios de negócio:

- **Auth/Users:** Autenticação JWT, controle de perfis de acesso.
- **StoreOperations:** Lançamento de vendas, associação de cupons fiscais.
- **Loyalty/Points:** Motor de cálculo e registro de auditoria (ledger) de pontuação.
- **Profiles:** Dashboards específicos para Parceiros e histórico para Clientes.

---
*Para mais detalhes sobre as regras de negócio completas, consulte o documento geral de requisitos na raiz do projeto.*
