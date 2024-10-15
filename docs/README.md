Perguntas para o PO e Considerações Técnicas e arquiteturais

1. Logging

    Requisito: Não foi solicitado nenhum tipo de logging.
    Ação: Adicionei o uso do ILogger para seguir boas práticas e garantir melhor rastreabilidade.

2. Banco de Dados

    Especificações ausentes: Não foram fornecidos diagramas de relacionamento, tamanhos de campos ou tipos de dados.
    Assumido: Configurei os campos e os tipos de dados com base em suposições razoáveis.
    ORM: Para efeitos de teste, utilizei o Entity Framework. Contudo, em sistemas de alta demanda, ORMs mais leves, como o Dapper, podem ser considerados devido ao melhor desempenho.

3. Regras para Tarefas

    Atribuição de Tarefas: Não foi especificado se uma tarefa pode pertencer a mais de um projeto. Assumi que cada tarefa está associada a um único projeto.
    Prioridade: Cada tarefa deve ter uma prioridade (baixa, média, alta). Adicionei esse campo como um enumerador. Não é possível criar uma tarefa já concluída, portanto, todas as tarefas iniciam como "Pendente".
    Imutabilidade: A prioridade da tarefa é definida no momento da criação e não pode ser alterada posteriormente. Removi esse campo do payload de atualização.

4. Escopo de Desempenho

    Latência e Throughput: O escopo do projeto não definiu requisitos claros para latência, throughput de dados ou volume de uso.
    Decisão: Implementarei o padrão CQRS, separando contextos de leitura e escrita, considerando que o sistema poderá crescer e exigirá baixo acoplamento.

5. Exclusão de Projetos

    Requisito: "Um projeto não pode ser removido se houver tarefas pendentes associadas".
    Remoção Física ou Lógica: Assumindo que o histórico do projeto deve ser mantido, farei a exclusão lógica. Sugiro, além disso, permitir que o projeto seja finalizado, além de excluído, o que poderia ser incluído nos requisitos.

6. Remoção de Tarefas

    Status das Tarefas: Para remover uma tarefa, vou considerar que ela apenas muda de status para "Completed" ou "Canceled". A exclusão lógica de uma tarefa cancelada poderia ser equivalente ao "deletado".

7. Listagens e Paginação

    Uso de Cache: Não houve menção à necessidade de cache (como Redis). Dependendo do tráfego, pode ser uma boa prática a ser considerada.
    Paginação: Não foi explicitamente requisitada, mas implementei, pois é uma prática comum para otimizar a exibição de listas.

8. Tratamento de Exceções

    Definição: Não houve refinamento sobre o tratamento de exceções e mensagens de resposta.
    Decisão: Implementarei um middleware simples para gerenciar as exceções, com mensagens padronizadas.

9. Relatórios de Desempenho

    Requisito: "A API deve fornecer endpoints para gerar relatórios de desempenho, como o número médio de tarefas concluídas por usuário nos últimos 30 dias".
    Dúvidas: Sugiro definir melhor esse requisito, pois o período pode variar (últimos 5 dias, semanal, etc.). A possibilidade de parametrizar o intervalo de tempo seria útil.

10. Comentários nas Tarefas

    Implementação Atual: Os comentários estão na tabela de atualizações.
    Sugestão: Criar uma tabela separada para comentários associados às tarefas. Isso evita o crescimento exponencial na tabela de atualizações e facilita a consulta das informações.

11. Banco de Dados e Migrations

    Configuração: Para gerar o banco via migrations, não foi definido se os dados secretos virão de uma ferramenta de secrets na nuvem ou de variáveis de ambiente.
 
12. Estrutura do Projeto

    Organização dos Módulos: A aplicação foi dividida em módulos seguindo a separação de responsabilidades (SRP), com camadas para Domínio, Aplicação, Infraestrutura e Interfaces.
    Contextos de Leitura e Escrita: Foram criados dois DbContexts distintos para suportar a separação de leitura e escrita no padrão CQRS.

13. Validação de Dados

    Validações: As validações dos dados de entrada foram implementadas usando FluentValidation para garantir consistência e evitar dados inválidos no sistema.
    Mensagens de Erro: As mensagens de erro estão configuradas para serem apresentadas em português, conforme preferência especificada.

14. Mapeamento entre Entidades e DTOs

    AutoMapper: Utilizei o AutoMapper para facilitar o mapeamento entre entidades de domínio e DTOs, mantendo o código mais limpo e reduzindo duplicação.
    Configuração Centralizada: A configuração do AutoMapper foi centralizada no Program.cs para garantir fácil manutenção e acesso.

15. Configuração e Variáveis de Ambiente

    Program.cs e Startup.cs: Separação entre Program.cs e Startup.cs foi adotada para garantir clareza na configuração do projeto.
    Variáveis de Ambiente: As variáveis de ambiente são usadas para configurar a string de conexão com o banco de dados e outros parâmetros sensíveis. Em um ambiente de produção, uma ferramenta de gerenciamento de segredos seria mais adequada para armazenar esses dados.

16. Testes

    Testes Unitários: A aplicação foi preparada para receber testes unitários, com o uso de interfaces nos serviços para facilitar a criação de mocks.
    Moq: A biblioteca Moq está sendo usada para simular dependências e permitir a execução de testes isolados.

17. Versionamento da API

    Sem Versionamento Definido: Não foi especificado um versionamento da API, mas para projetos que crescem rapidamente, isso é considerado uma boa prática para evitar quebras de compatibilidade em futuras alterações.

18. Segurança

    Autenticação e Autorização: Não foi especificada nenhuma forma de autenticação ou autorização para os endpoints. Recomenda-se implementar uma camada de segurança com JWT ou OAuth2 para proteger os recursos, especialmente em ambientes de produção.

19. Documentação de Endpoints

    Swagger: A documentação dos endpoints foi gerada automaticamente utilizando o Swagger, facilitando o acesso aos detalhes de cada operação e parâmetros.