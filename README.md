
# FinancialsTransferManager
Exemplo de API REST criada com ASP.NET Core + Mongo + ELK + RabbitMQ + Docker

## Arquitetura: 
![Arquitetura da Solução](https://github.com/osoaresvictor/FinancialsTransferManager/blob/master/Architecture%20Model.png?raw=true)

## Para executar:
- Acesse o diretório que contem o arquivo `docker-compoose.yml` (`cd FinancialsTransfersManager`);
- Execute: `docker-compose up`;
- Acesse: http://localhost:8000 e verá toda a documentação Swagger. 

## Observações Gerais:
- A aplicação leva alguns segundos para subir por completo na primmeira execução do docker-compose;
- Acesse http://localhost/api/account para ver a lista de accounts disponíveis;
- Para ver os logs do Kibana, você precisará definir um padrão de índice pela primeira vez (Sugestão: `logstash-*` e `@timestamp` para time filter).
