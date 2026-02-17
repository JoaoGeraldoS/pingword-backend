# Backend Pingword

### Visão geral
Este backend tem como objetivo registar e interpretar comportamento de estudos do usuário ao longo do tempo.
Seu papel é guardar histórico, detectar padrões de atividade e permitir retomatos naturais, mesmo após longos períodos de inatividade.

 O Backend é ultilizado pelo aplicativo PingWord que envia notificações com palavras para estudo passivo.  

### Conceito Central
O backend funciona como um diário automático de estudo.
Ele registra quando uma notificação é enviada, quando o usuário interage com ela, quando ocorreu a última interação, em que estado de estudo o usuário estar.

### Problemas que o Backend Resolve
- **Dificuldade de Manter Consistência**: O backend ajuda a manter um registro consistente das atividades de estudo do usuário, mesmo que ele tenha períodos de inatividade.
- **Falta de métricas claras**: O backend fornece métricas claras sobre o comportamento de estudo do usuário.
- **Motivação baseada em dados**: O backend pode usar os dados coletados para fornecer feedback personalizado e motivação ao usuário.

### Tecnologias Utilizadas
- **ASP.NET Core 8**: Para criar endpoints que o aplicativo PingWord pode consumir.
- **Entity Framework Core**: Para gerenciamento de banco de dados e mapeamento objeto-relacional.
- **Swagger**: Para documentação e teste dos endpoints da API.
- **Serilog**: Para logging estruturado e monitoramento do backend.
- **Render (Deploy)**: Ultizada para hospedar o backend na nuvem.

### Arquitetura
- **Controllers -> Services -> Repositories**: Seguindo a arquitetura de camadas para separar responsabilidades.
- **DTOs**: Para transferência de dados entre camadas e para o cliente.
- **Queries com AsNoTracking**: Para otimizar consultas de leitura, evitando o rastreamento desnecessário de entidades.

### Funcionalidades Principais
- **Registro de Atividades**: O backend registra cada interação do usuário com as notificações, incluindo o horário e o tipo de interação.
- **Cálculo de Métricas**: O backend calcula métricas como frequência de estudo.
- **Estado atual do usuário**: O backend mantém o estado atual do usuário.
- **Métricas agregadas**: O backend pode fornecer métricas agregadas para o usuário, como tempo total de estudo, dias consecutivos de estudo, etc.

### Estrutura do Projeto
- **/Controllers**: Contém os controladores que expõem os endpoints da API.
- **/Services**: Contém a lógica de negócios e as regras de aplicação.
- **/Repositories**: Contém a lógica de acesso a dados e interações com o banco de dados.
- **/DTOs**: Contém os objetos de transferência de dados usados para comunicação entre camadas e com o cliente.
- **/Models**: Contém as entidades do domínio e os modelos de dados.
- **/Data**: Contém o contexto do banco de dados e as configurações de acesso a dados.
- **/Migrations**: Contém as migrações do Entity Framework para gerenciamento do esquema do banco de dados.
- **/Errors**: Contém classes para tratamento de erros e exceções personalizadas.
- **/Mappers**: Contém classes para mapeamento entre entidades e DTOs, facilitando a conversão de dados entre camadas.
- **Workers**: Contém classes para tarefas em segundo plano, como buscas e atualizações de estados.
- **/Enums**: Contém enumerações usadas em todo o projeto para representar estados, tipos de interação, etc.