using System;
using Nest;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace ESDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            #region Get Url

            Console.Write("Provide host URL (ex: http://localhost:9200): ");
            var host = Console.ReadLine();
            host = host + (host.EndsWith("/", StringComparison.InvariantCulture) ? "" : "/");

            #endregion

            #region Create Index

            Console.Write("Provide index name: ");
            var indexName = Console
                .ReadLine()
                .ToLower(); // Index name needs to be lowercase
            
            // Example using REST to create index
            Console.WriteLine($"Creating index {indexName}...");
            new HttpClient().PutAsync(host + indexName, new JsonContent(new { }));

            #endregion

            #region Setup Elasticsearch Client
            
            var es = new ElasticClient(
                new ConnectionSettings(new Uri(host))
                .DefaultIndex(indexName)
            );

            #endregion

            #region Add Documents

            IndexResponse contentResponse;

            Console.WriteLine("Adding document...");
            contentResponse = es.IndexDocument(             // Add a document to the index
                new MyDocument()                            // using a MyDocument object as the document
                {
                    Title = "This is a test document",
                    Notes = "Hello World!"
                });
            Console.WriteLine(contentResponse);

            Console.WriteLine("Adding document...");
            contentResponse = es.IndexDocument(             // Add a document to the index
                new MyDocument()                            // using a MyDocument object as the document
                {
                    Title = "This is a test document",
                    Notes = "Hello Office!"
                });
            Console.WriteLine(contentResponse);

            #endregion

            #region Search
            
            Console.Write("Provide search query: ");
            var contentQuery = Console.ReadLine();
            Console.WriteLine("Searching...");
            var results = es.Search<MyDocument>(                    // Searching for type MyDocument,
                    search => search.Query(                         // Create a query
                        query => query.Bool(                        // where a match
                            match => match.Must(                    // must have
                                mustHave => mustHave.QueryString(   // this string
                                    queryString => queryString.Query(contentQuery)
                                )
                            )
                        )
                    )
                )
                .Documents // There is metadata available but we want the documents found
                .ToList();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"Found: {results.Count}");
            Console.WriteLine("==============================");

            foreach(var doc in results)
            {
                Console.WriteLine(doc);
            }

            Console.WriteLine(Environment.NewLine);

            #endregion

            #region Delete Index

            // Example using REST to delete index
            Console.WriteLine($"Deleting index {indexName}...");
            new HttpClient().DeleteAsync(host + indexName);

            #endregion

            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
