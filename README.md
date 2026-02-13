# 🛡️ InsuranceSystem - Vehicle Insurance API

> **.NET Clean Architecture | DDD | CQRS | Docker | React**

Este projeto é uma solução robusta para o cálculo de seguros de veículos. A aplicação segue rigorosamente os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**.

---

## 🚀 Tecnologias Utilizadas

| Categoria | Tecnologias |
| :--- | :--- |
| **Back-end** | .NET 8, ASP.NET Core Web API, Entity Framework Core (Code-First), MediatR (CQRS), Refit, FluentValidation |
| **Front-end** | React (Vite), TypeScript, Bootstrap 5, Axios |
| **Banco de Dados** | SQL Server 2022 |
| **Container / Infra** | Docker, Docker Compose, Nginx (Frontend Server) |
| **Testes** | xUnit, FluentAssertions |

---

## 🚀 Visão Geral do Projeto

O sistema é composto por:
1.  **API RESTful (.NET 8):**
    *   **Calcular Seguros:** Processa variáveis e persiste os dados.
    *   **Relatórios:** Fornece métricas estatísticas.
    *   **CQRS & Mediator:** Separação clara de leitura e escrita.
2.  **Frontend (React + Bootstrap):**
    *   Interface moderna e responsiva para visualização do relatório de médias.
3.  **Mock Service (JSON Server):**
    *   Simula a API externa de dados de segurados.
4.  **Banco de Dados (SQL Server):**
    *   Persistência relacional robusta.

---

## 🏗️ Estrutura da Solução

```text
InsuranceSystem
├── src
│   ├── InsuranceSystem.API              # Backend API
│   ├── InsuranceSystem.Web              # Frontend (React App)
│   ├── InsuranceSystem.Domain           # Regras de Negócio
│   ├── InsuranceSystem.Application      # Casos de Uso
│   └── InsuranceSystem.Infrastructure   # Persistência
├── mock-data
│   └── db.json                          # Dados Mockados (Segurados)
├── docker-compose.yml
└── Dockerfile
```

---

## 🚀 Como Executar (Ambiente Completo)

A maneira mais fácil de executar todo o ecossistema (Banco, API, Frontend e Mock) é via Docker.

### 1. Pré-requisitos
* Docker Desktop instalado.

### 2. Execução
Na raiz do projeto, execute:

```bash
docker-compose up --build
```

Aguarde alguns instantes para que o SQL Server inicialize completamente.

### 3. Acessar Aplicação

| Serviço | URL | Descrição |
| :--- | :--- | :--- |
| **Frontend (Relatório)** | **http://localhost** | Painel visual com as médias dos seguros. |
| **API (Swagger)** | http://localhost:5000/swagger | Documentação e teste dos endpoints. |
| **Mock API** | http://localhost:3000 | API simulada de segurados. |

---

## 🖥️ Execução do Front-end

O Front-end foi desenvolvido em **React (Vite) + Bootstrap** e está localizado em `src/InsuranceSystem.Web`.

### Opção A: Via Docker (Recomendado)
Ao executar o `docker-compose up --build` na raiz do projeto, o front-end é automaticamente compilado e servido via **Nginx** na porta **80**.
*   **Acesso:** [http://localhost](http://localhost)

### Opção B: Desenvolvimento Local (Node.js)
Se desejar rodar o front-end fora do Docker para desenvolvimento:
1.  Certifique-se de ter o **Node.js 18+** instalado.
2.  Navegue até a pasta:
    ```bash
    cd src/InsuranceSystem.Web
    ```
3.  Instale as dependências:
    ```bash
    npm install
    ```
4.  Inicie o servidor de desenvolvimento:
    ```bash
    npm run dev
    ```
5.  **Acesso:** O terminal informará a URL (geralmente `http://localhost:5173`).
    *   *Nota: Certifique-se de que a API esteja rodando em http://localhost:5000 para que o front-end consiga consumir os dados.*

---

## 🧪 Testando o Fluxo

1.  Abra o **Swagger** (`http://localhost:5000/swagger`).
2.  Use o endpoint `POST /api/insurance` para criar alguns seguros.
    *   *Dica: Use CPFs que existam no `mock-data/db.json` (ex: `12345678900`) para obter nomes reais, ou qualquer outro para usar o fallback.*
    *   Exemplo de JSON:
        ```json
        {
          "insuredCpf": "12345678900",
          "vehicleModel": "Honda Civic",
          "vehicleValue": 50000
        }
        ```
3.  Após criar alguns registros, abra o **Frontend** (`http://localhost`).
4.  O painel exibirá automaticamente a quantidade de seguros, média de valor dos veículos e média dos prêmios comerciais.

---

## ⚙️ Desenvolvimento Local (Sem Docker)

Caso queira rodar os projetos individualmente:

1.  **Banco de Dados:** Certifique-se de ter uma instância SQL Server (`.\SQL2019` configurada no appsettings ou altere para a sua).
2.  **API:** `dotnet run --project src/InsuranceSystem.API`
3.  **Frontend:**
    ```bash
    cd src/InsuranceSystem.Web
    npm install
    npm run dev
    ```

---

## ⚠️ Observações Importantes: Portas e Conectividade

Para garantir que o Front-end consiga se comunicar com o Back-end, as portas devem estar alinhadas:

*   **Execução via Docker:** Tudo é configurado automaticamente. O Front-end (porta 80) acessa a API através do mapeamento interno para a porta 5000.
*   **Execução Local (Visual Studio/CLI):**
    *   A API está configurada no arquivo `launchSettings.json` para rodar na porta **5000**.
    *   O Front-end (React) está configurado em `App.tsx` para buscar os dados em `http://localhost:5000`.
    *   **Importante:** Se você alterar a porta da API no `launchSettings.json`, deverá refletir essa mudança na variável `apiUrl` dentro do arquivo `src/InsuranceSystem.Web/src/App.tsx`.

---

## 🧪 Testes Automatizados

Para executar os testes de unidade que validam a lógica matemática do exame:

```bash
dotnet test
```