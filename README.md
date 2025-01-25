# Task Management API

## Descrição
Este projeto é uma API para gerenciamento de tarefas, que permite criar, atualizar, e gerenciar tarefas associadas a projetos. 
Também é possível adicionar comentários às tarefas e gerar relatórios de desempenho para usuários com função de gerente. 
O sistema possui regras de negócio claras para organização e controle.

## Funcionalidades Principais
- **Tarefas:** CRUD completo para gerenciamento de tarefas.
- **Projetos:** CRUD para projetos.
- **Comentários:** Possibilidade de adicionar comentários às tarefas.
- **Histórico de tarefas:** Registro automático de alterações feitas nas tarefas.
- **Relatórios de desempenho:** Cálculo de métricas como o número médio de tarefas concluídas por usuário nos últimos 30 dias.

---

## Como executar o projeto

### Requisitos
- **Docker**: Certifique-se de que o Docker e o Docker Compose estão instalados.
- **.NET 8+**: Para compilação local sem Docker (opcional).
- **SQL Server**: Banco de dados utilizado no projeto.

### Passos para executar com Docker Compose

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/ruandias/challenge.git
   cd challenge
   ```

2. **Suba os contêineres:**
   Certifique-se de estar na raiz do projeto, onde o arquivo `docker-compose.yml` está localizado, e execute:
   ```bash
   docker-compose up -d
   ```

3. **Acesse a API:**
   Após a inicialização bem-sucedida, a API estará disponível em:
   - Swagger UI: `http://localhost:5000/swagger/index.html`

4. **Parar os contêineres:**
   Quando não estiver usando, pare os contêineres com:
   ```bash
   docker-compose down
   ```

---

## Estrutura de Diretórios

- **Controllers:** Contém os endpoints da API.
- **Services:** Lógica de negócio, como validação de regras e integrações.
- **Repositories:** Responsáveis pela interação com o banco de dados.
- **Entities:** Classes que representam as tabelas do banco de dados.
- **DTOs:** Objetos de transferência de dados para comunicação com a API.
- **Migrations:** Configurações do Entity Framework para criação de tabelas e seed de dados.

---

## Seed de Dados

Ao executar as migrations, dados iniciais são populados no banco de dados, incluindo:
- **Usuários:** Usuários com diferentes funções ("manager" e "colaborator").

---

## O que perguntar ao PO para refinamento

- **Relatórios:**
  - Há necessidade de outros relatórios além do desempenho das tarefas concluídas?
  - Como os relatórios serão apresentados? (PDF, JSON, dashboards?)

- **Autenticação e Autorizacão:**
  - Planejamos implementar autenticação JWT ou outra solução? Alguma integração com provedores de identidade?

- **Lógica de Negócio:**
  - Existe algum prazo específico para a conclusão de tarefas que precisa ser validado?
  - Quais outros limites devem ser impostos (como o limite de 20 tarefas por projeto)?

- **Escalabilidade:**
  - Existe previsão de aumento no número de usuários ou tarefas que o sistema deve suportar?
  - Planejamos migrar para um ambiente em nuvem no futuro?

---

## Pontos de Melhoria e Visão do Projeto

### Padrões de Projeto e Arquitetura
- **Modularização:** Separar melhor as responsabilidades, talvez implementando uma arquitetura mais segmentada (Clean Architecture).
- **CQRS:** Implementar Command Query Responsibility Segregation para isolar comandos de consultas em operações mais complexas.
- **MediatR:** Utilizar o MediatR para gerenciar requisições, tornando a comunicação entre camadas mais desacoplada.

### Integração Contínua
- Adicionar pipelines de CI/CD para validação e publicação automatizada, utilizando GitHub Actions, Azure DevOps ou GitLab CI/CD.

### Banco de Dados
- Adicionar índices nas tabelas mais acessadas (como `Tasks` e `Comments`) para melhorar o desempenho das consultas.
- Revisar as seeds para que sejam ajustáveis via variáveis de ambiente.

### Cloud
- Considerar hospedar em uma solução cloud como Azure ou AWS para escalabilidade.
- Utilizar serviços gerenciados, como Azure SQL, para simplificar a manutenção do banco.
- Implementar monitoramento usando ferramentas como Application Insights.

### Documentação e Testes
- Melhorar a documentação no Swagger, incluindo exemplos mais detalhados.
- Cobrir a API com testes de integração.
- Adicionar suporte a testes de carga para verificar a escalabilidade do sistema.

### Frontend
- Planejar um frontend para consumo da API, com dashboards e visualizações dos relatórios (React ou Angular seriam boas opções).

Com esses ajustes e refinamentos, o projeto ficaria mais robusto, escalável e pronto para evoluir conforme as necessidades do negócio.

