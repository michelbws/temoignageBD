using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DocDbAzure;

namespace NeuroParser
{
    public class ExempleAzure
    {
        //static string neuroBaseURL = "https://services.radio-canada.ca/neuro/v1";
        //static HttpClient httpClient = new HttpClient();
        static Uri docAccountUri = new Uri("https://temoignagedb.documents.azure.com:443/");
        static DocumentClient docClient = new DocumentClient(docAccountUri, "2WxpSk5O18zS0dkvcBZrG6OFncEjWffMmnQhr84J2rfvZADVnNHEI1fX1XOGbnp4oCQEHG1zHHuULFMe9Ui1ZQ==");
        static string dbId = "Temoin";
        static string collId = "TemoinColl";
        static Uri dbUri = new Uri($"dbs/{dbId}", UriKind.Relative);
        static Uri collUri = new Uri($"dbs/{dbId}/colls/{collId}", UriKind.Relative);

        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Process(int.Parse(args[0])).Wait();
            }
            else
            {
                Process().Wait();
            }
        }

        public static async Task Process(int startAt = -1)
        {
            var db = await docClient.CreateDatabaseIfNotExistsAsync(new Database {Id = dbId});

            await docClient.CreateDocumentCollectionIfNotExistsAsync(dbUri,
                new DocumentCollection
                {
                    Id = collId,
                    PartitionKey = new PartitionKeyDefinition
                    {
                        Paths = {"/temoinID"},
                    },
                });
        }


        public async static Task UpsertDocument(TemoingnageJsn temoignage)
        {
            var response = await docClient.UpsertDocumentAsync(collUri, temoignage, disableAutomaticIdGeneration: true);
        }


        //public static async Task<string> GetPage(int pageNumber)
        //{
        //    return await httpClient.GetStringAsync($"{neuroBaseURL}/future/lineups?pageNumber={pageNumber}");
        //}

        //public static async Task<string> GetLineup(int lineupId, int pageNumber)
        //{
        //    return await httpClient.GetStringAsync($"{neuroBaseURL}/future/lineups/{lineupId}?pageNumber={pageNumber}");
        //}
    }
}
