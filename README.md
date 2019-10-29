# elasticsearch-dotnet-demo
Demo for using .NET Core to perform a basic search query with Elasticsearch

This project requires a running Elasticsearch instance.
You can create a Docker container running Elasticsearch using the following:

docker run -d --name es-1 -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" elasticsearch:7.3.1

