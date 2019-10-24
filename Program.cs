using System;

using Nest;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace Elasticsearch1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            Console.Write("Provide host URL (ex: http://localhost:9200): ");
            var host = Console.ReadLine();
            host = host + (host.EndsWith("/", StringComparison.InvariantCulture) ? "" : "/");
            
            Console.Write("Provide index name: ");
            var indexName = Console.ReadLine();

            var es = new ElasticClient(
                new ConnectionSettings(new Uri(host))
                .DefaultIndex(indexName)
            );


            Console.WriteLine($"Creating index {indexName}...");
            new HttpClient().PutAsync(host + indexName, new JsonContent(new { }));

            Console.WriteLine("Adding document...");
            IndexResponse contentResponse;
            contentResponse = es.IndexDocument(new MyDocument()
            {
                Title = "This is a test document",
                Notes = "Hello World!"
            });
            Console.WriteLine(contentResponse);

            Console.WriteLine("Adding document...");
            contentResponse = es.IndexDocument(new MyDocument()
            {
                Title = "This is a test document",
                Notes = "Hello Office!"
            });
            Console.WriteLine(contentResponse);

            Console.Write("Provide search query: ");
            var contentQuery = Console.ReadLine();
            Console.WriteLine("Searching...");
            var results = es.Search<MyDocument>(
                search => search.Query(
                    query => query.Bool(
                        match => match.Must(
                            mustHave => mustHave.QueryString(
                                queryString => queryString.Query(contentQuery)
                            )
                        )
                    )
                )
            )
            .Documents
            .ToList();
            Console.WriteLine($"Found: {results.Count}");
            foreach(var doc in results)
            {
                Console.WriteLine(new JsonContent(doc).ReadAsStringAsync().Result);
            }
            

            Console.WriteLine($"Deleting index {indexName}...");
            new HttpClient().DeleteAsync(host + indexName);

            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
