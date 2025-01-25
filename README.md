# Task Management API

## Descri��o
Este projeto � uma API para gerenciamento de tarefas, que permite criar, atualizar, e gerenciar tarefas associadas a projetos. 
Tamb�m � poss�vel adicionar coment�rios �s tarefas e gerar relat�rios de desempenho para usu�rios com fun��o de gerente. 
O sistema possui regras de neg�cio claras para organiza��o e controle.

## Funcionalidades Principais
- **Tarefas:** CRUD completo para gerenciamento de tarefas.
- **Projetos:** CRUD para projetos.
- **Coment�rios:** Possibilidade de adicionar coment�rios �s tarefas.
- **Hist�rico de tarefas:** Registro autom�tico de altera��es feitas nas tarefas.
- **Relat�rios de desempenho:** C�lculo de m�tricas como o n�mero m�dio de tarefas conclu�das por usu�rio nos �ltimos 30 dias.

---

## Como executar o projeto

### Requisitos
- **Docker**: Certifique-se de que o Docker e o Docker Compose est�o instalados.
- **.NET 8+**: Para compila��o local sem Docker (opcional).
- **SQL Server**: Banco de dados utilizado no projeto.

### Passos para executar com Docker Compose

1. **Clone o reposit�rio:**
   ```bash
   git clone https://github.com/ruandias/challenge.git
   cd challenge
   ```

2. **Suba os cont�ineres:**
   Certifique-se de estar na raiz do projeto, onde o arquivo `docker-compose.yml` est� localizado, e execute:
   ```bash
   docker-compose up -d
   ```

3. **Acesse a API:**
   Ap�s a inicializa��o bem-sucedida, a API estar� dispon�vel em:
   - Swagger UI: `http://localhost:5000/swagger/index.html`

4. **Parar os cont�ineres:**
   Quando n�o estiver usando, pare os cont�ineres com:
   ```bash
   docker-compose down
   ```

---

## Estrutura de Diret�rios

- **Controllers:** Cont�m os endpoints da API.
- **Services:** L�gica de neg�cio, como valida��o de regras e integra��es.
- **Repositories:** Respons�veis pela intera��o com o banco de dados.
- **Entities:** Classes que representam as tabelas do banco de dados.
- **DTOs:** Objetos de transfer�ncia de dados para comunica��o com a API.
- **Migrations:** Configura��es do Entity Framework para cria��o de tabelas e seed de dados.

---

## Seed de Dados

Ao executar as migrations, dados iniciais s�o populados no banco de dados, incluindo:
- **Usu�rios:** Usu�rios com diferentes fun��es ("manager" e "colaborator").

---

## O que perguntar ao PO para refinamento

- **Relat�rios:**
  - H� necessidade de outros relat�rios al�m do desempenho das tarefas conclu�das?
  - Como os relat�rios ser�o apresentados? (PDF, JSON, dashboards?)

- **Autentica��o e Autorizac�o:**
  - Planejamos implementar autentica��o JWT ou outra solu��o? Alguma integra��o com provedores de identidade?

- **L�gica de Neg�cio:**
  - Existe algum prazo espec�fico para a conclus�o de tarefas que precisa ser validado?
  - Quais outros limites devem ser impostos (como o limite de 20 tarefas por projeto)?

- **Escalabilidade:**
  - Existe previs�o de aumento no n�mero de usu�rios ou tarefas que o sistema deve suportar?
  - Planejamos migrar para um ambiente em nuvem no futuro?

---

## Pontos de Melhoria e Vis�o do Projeto

### Padr�es de Projeto e Arquitetura
- **Modulariza��o:** Separar melhor as responsabilidades, talvez implementando uma arquitetura mais segmentada (Clean Architecture).
- **CQRS:** Implementar Command Query Responsibility Segregation para isolar comandos de consultas em opera��es mais complexas.
- **MediatR:** Utilizar o MediatR para gerenciar requisi��es, tornando a comunica��o entre camadas mais desacoplada.

### Integra��o Cont�nua
- Adicionar pipelines de CI/CD para valida��o e publica��o automatizada, utilizando GitHub Actions, Azure DevOps ou GitLab CI/CD.

### Banco de Dados
- Adicionar �ndices nas tabelas mais acessadas (como `Tasks` e `Comments`) para melhorar o desempenho das consultas.
- Revisar as seeds para que sejam ajust�veis via vari�veis de ambiente.

### Cloud
- Considerar hospedar em uma solu��o cloud como Azure ou AWS para escalabilidade.
- Utilizar servi�os gerenciados, como Azure SQL, para simplificar a manuten��o do banco.
- Implementar monitoramento usando ferramentas como Application Insights.

### Documenta��o e Testes
- Melhorar a documenta��o no Swagger, incluindo exemplos mais detalhados.
- Cobrir a API com testes de integra��o.
- Adicionar suporte a testes de carga para verificar a escalabilidade do sistema.

### Frontend
- Planejar um frontend para consumo da API, com dashboards e visualiza��es dos relat�rios (React ou Angular seriam boas op��es).

Com esses ajustes e refinamentos, o projeto ficaria mais robusto, escal�vel e pronto para evoluir conforme as necessidades do neg�cio.

