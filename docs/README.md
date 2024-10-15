Perguntas para o PO e Considera��es T�cnicas e arquiteturais

1. Logging

    Requisito: N�o foi solicitado nenhum tipo de logging.
    A��o: Adicionei o uso do ILogger para seguir boas pr�ticas e garantir melhor rastreabilidade.

2. Banco de Dados

    Especifica��es ausentes: N�o foram fornecidos diagramas de relacionamento, tamanhos de campos ou tipos de dados.
    Assumido: Configurei os campos e os tipos de dados com base em suposi��es razo�veis.
    ORM: Para efeitos de teste, utilizei o Entity Framework. Contudo, em sistemas de alta demanda, ORMs mais leves, como o Dapper, podem ser considerados devido ao melhor desempenho.

3. Regras para Tarefas

    Atribui��o de Tarefas: N�o foi especificado se uma tarefa pode pertencer a mais de um projeto. Assumi que cada tarefa est� associada a um �nico projeto.
    Prioridade: Cada tarefa deve ter uma prioridade (baixa, m�dia, alta). Adicionei esse campo como um enumerador. N�o � poss�vel criar uma tarefa j� conclu�da, portanto, todas as tarefas iniciam como "Pendente".
    Imutabilidade: A prioridade da tarefa � definida no momento da cria��o e n�o pode ser alterada posteriormente. Removi esse campo do payload de atualiza��o.

4. Escopo de Desempenho

    Lat�ncia e Throughput: O escopo do projeto n�o definiu requisitos claros para lat�ncia, throughput de dados ou volume de uso.
    Decis�o: Implementarei o padr�o CQRS, separando contextos de leitura e escrita, considerando que o sistema poder� crescer e exigir� baixo acoplamento.

5. Exclus�o de Projetos

    Requisito: "Um projeto n�o pode ser removido se houver tarefas pendentes associadas".
    Remo��o F�sica ou L�gica: Assumindo que o hist�rico do projeto deve ser mantido, farei a exclus�o l�gica. Sugiro, al�m disso, permitir que o projeto seja finalizado, al�m de exclu�do, o que poderia ser inclu�do nos requisitos.

6. Remo��o de Tarefas

    Status das Tarefas: Para remover uma tarefa, vou considerar que ela apenas muda de status para "Completed" ou "Canceled". A exclus�o l�gica de uma tarefa cancelada poderia ser equivalente ao "deletado".

7. Listagens e Pagina��o

    Uso de Cache: N�o houve men��o � necessidade de cache (como Redis). Dependendo do tr�fego, pode ser uma boa pr�tica a ser considerada.
    Pagina��o: N�o foi explicitamente requisitada, mas implementei, pois � uma pr�tica comum para otimizar a exibi��o de listas.

8. Tratamento de Exce��es

    Defini��o: N�o houve refinamento sobre o tratamento de exce��es e mensagens de resposta.
    Decis�o: Implementarei um middleware simples para gerenciar as exce��es, com mensagens padronizadas.

9. Relat�rios de Desempenho

    Requisito: "A API deve fornecer endpoints para gerar relat�rios de desempenho, como o n�mero m�dio de tarefas conclu�das por usu�rio nos �ltimos 30 dias".
    D�vidas: Sugiro definir melhor esse requisito, pois o per�odo pode variar (�ltimos 5 dias, semanal, etc.). A possibilidade de parametrizar o intervalo de tempo seria �til.

10. Coment�rios nas Tarefas

    Implementa��o Atual: Os coment�rios est�o na tabela de atualiza��es.
    Sugest�o: Criar uma tabela separada para coment�rios associados �s tarefas. Isso evita o crescimento exponencial na tabela de atualiza��es e facilita a consulta das informa��es.

11. Banco de Dados e Migrations

    Configura��o: Para gerar o banco via migrations, n�o foi definido se os dados secretos vir�o de uma ferramenta de secrets na nuvem ou de vari�veis de ambiente.
 
12. Estrutura do Projeto

    Organiza��o dos M�dulos: A aplica��o foi dividida em m�dulos seguindo a separa��o de responsabilidades (SRP), com camadas para Dom�nio, Aplica��o, Infraestrutura e Interfaces.
    Contextos de Leitura e Escrita: Foram criados dois DbContexts distintos para suportar a separa��o de leitura e escrita no padr�o CQRS.

13. Valida��o de Dados

    Valida��es: As valida��es dos dados de entrada foram implementadas usando FluentValidation para garantir consist�ncia e evitar dados inv�lidos no sistema.
    Mensagens de Erro: As mensagens de erro est�o configuradas para serem apresentadas em portugu�s, conforme prefer�ncia especificada.

14. Mapeamento entre Entidades e DTOs

    AutoMapper: Utilizei o AutoMapper para facilitar o mapeamento entre entidades de dom�nio e DTOs, mantendo o c�digo mais limpo e reduzindo duplica��o.
    Configura��o Centralizada: A configura��o do AutoMapper foi centralizada no Program.cs para garantir f�cil manuten��o e acesso.

15. Configura��o e Vari�veis de Ambiente

    Program.cs e Startup.cs: Separa��o entre Program.cs e Startup.cs foi adotada para garantir clareza na configura��o do projeto.
    Vari�veis de Ambiente: As vari�veis de ambiente s�o usadas para configurar a string de conex�o com o banco de dados e outros par�metros sens�veis. Em um ambiente de produ��o, uma ferramenta de gerenciamento de segredos seria mais adequada para armazenar esses dados.

16. Testes

    Testes Unit�rios: A aplica��o foi preparada para receber testes unit�rios, com o uso de interfaces nos servi�os para facilitar a cria��o de mocks.
    Moq: A biblioteca Moq est� sendo usada para simular depend�ncias e permitir a execu��o de testes isolados.

17. Versionamento da API

    Sem Versionamento Definido: N�o foi especificado um versionamento da API, mas para projetos que crescem rapidamente, isso � considerado uma boa pr�tica para evitar quebras de compatibilidade em futuras altera��es.

18. Seguran�a

    Autentica��o e Autoriza��o: N�o foi especificada nenhuma forma de autentica��o ou autoriza��o para os endpoints. Recomenda-se implementar uma camada de seguran�a com JWT ou OAuth2 para proteger os recursos, especialmente em ambientes de produ��o.

19. Documenta��o de Endpoints

    Swagger: A documenta��o dos endpoints foi gerada automaticamente utilizando o Swagger, facilitando o acesso aos detalhes de cada opera��o e par�metros.