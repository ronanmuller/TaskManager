
Perguntas ao PO 

- N�o foi requisitado nenhum tipo de log. Adicionei o Ilogger por ser uma boa pratica. 

- Banco de dados - Nao foi especificado com diagramas de relacionamentos, nem tamanho dos campos, tipos etc. Estou assumindo os tamanhos, tipos e nomeclatura.
Alem disso para efeito de teste tecnico eu usei o entity mas normalmente em sistemas com fluxo alto se leva em considera��o ORMs mais leves como o dapper com performance melhor.

- N�o foi informado se a mesma tarefa poderia ser atribuida a mais de um projeto. Devido as regras de update de tarefas eu assumi que a tarefa � de um projeto apenas.

- Cada tarefa deve ter uma prioridade atribu�da (baixa, m�dia, alta). N�o foi especificado o que seria feito com esse campo. Coloquei na tabela como um enumerador.
A tarefa inicial inicia como pendente. Ela vai ser atualizada. N�o � possivel criar uma tarefa ja concluida por exemplo
 
- N�o � permitido alterar a prioridade de uma tarefa depois que ela foi criada. 
Vou assumir que esta na tabela de tarefas, � um campo imutavel e � setado no payload de criacao de tarefas. Vou tirar o campo do payload de atualizacao
 
 - N�o foi especificado o escopo do projeto com rela��o a latencia, throughput de dados, uso de dados. 
 Vou usar o padr�o CQRS assumindo uma latencia alta com contextos de leitura e escrita separados e partindo do presuposto que estamos criando um sistema que ira crescer (baixo acoplamento)
 
 Nos requisitos para o item:
 "Um projeto n�o pode ser removido se ainda houver tarefas pendentes associadas a ele". 
 A remo��o � fisica ou logica? Assumindo que estamos falando em manter historicos de projetos vou deletar logicamente. Alem disso creio que um projeto alem de excluido poderia ser finalizado tbem. Pode ser incluido nos requisitos
 
 Remo��o de Tarefas - remover uma tarefa de um projeto
 Ja as tarefas assumo que so mudaria o status para Completed ou Canceled. O cancelado poderia ser equivalente ao deletado? 

 - Nos casos das listagens n�o foi requisitado a necessidade do uso de cache (redis por exemplo). Dependendo do tamanho do trafego novamente � uma boa pratica;
 
 - N�o foi citado tambem a necessidade de paginacao porem levei em consideracao que isso � uma pratica comum para listas.
 
- N�o foi feito um refinamento de como seria o tratamento de excessao e as mensagens de resposta (resources) (mensagens) para o projeto. Vou criar o mais simples. Um middleware que orquestra todas as excess�es.
  
 - A API deve fornecer endpoints para gerar relat�rios de desempenho, como o n�mero m�dio de tarefas conclu�das por usu�rio nos �ltimos 30 dias.
 Eu refinaria melhor esse requisito. Porem  levei em considera��o que o gerente pode informar a data que ele quiser. (exemplo pedir um relatorio na sexta feira da semana ou os ultimos 5 dias)
 O numero medio seria a diaria, media semanal? Pode ser passado como parametro essa informa��o. 
 
 
 - Para ese requisito: Coment�rios nas Tarefas creio que uma melhor definicao com o arquiteto seria interessante. 
 Em vez de colocar os comentario na tabela de atualizac�es, criar um registro separada vinculado a task. 
 Porque podemos ter comentarios diversos e a tabela de atualizacoes ia crescer exponencialmente. Dificultando ate para carregar as informacoes.


 Para gerar o banco via migrations: (N�o foi definido se os dados secretos viram de uma secret na nuvem ou de variaveis de ambiente.) Coloquei direto no appsettings so para gerar o migration.
 Na hora da migra��o setar a conection string apropriada
 Add-Migration Initial -Context ReadContext
 PM> Update-Database -Context ReadContext
 

