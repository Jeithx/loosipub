using Nest;

public class ElasticsearchClient<T> where T : class
{
    private readonly ElasticClient _client;
    private readonly string _indexName;

    public ElasticsearchClient(string uri, string indexName, string username, string password)
    {
        var settings = new ConnectionSettings(new Uri(uri))
            .DefaultIndex(indexName)
            .DisableDirectStreaming()
            .BasicAuthentication(username, password) // Kullanıcı adı ve şifre ile oturum açma
            .ServerCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) =>
                true); // SSL doğrulamasını devre dışı bırak
        _client = new ElasticClient(settings);
        _indexName = indexName;
    }

    public ElasticClient Client => _client;

    public async Task<bool> InsertDocumentAsync(T document)
    {
        var response = await _client.IndexDocumentAsync(document);
        return response.IsValid;
    }

    public async Task<T> GetDocumentAsync(string id)
    {
        var response = await _client.GetAsync<T>(id);
        return response.Source;
    }

    public async Task<bool> UpdateDocumentAsync(string id, T document)
    {
        var response = await _client.UpdateAsync<T>(id, u => u.Doc(document));
        return response.IsValid;
    }

    public async Task<bool> DeleteDocumentAsync(string id)
    {
        var response = await _client.DeleteAsync<T>(id);
        return response.IsValid;
    }

    public async Task<List<T>> SearchAsync(string searchTerm, string fieldName)
    {
        var searchResponse = await _client.SearchAsync<T>(s => s
            .Query(q => q
                .Match(m => m
                        .Field(fieldName) // Aranacak alan
                        .Query(searchTerm) // Arama terimi
                        .Fuzziness(Fuzziness.Auto) // Esnek eşleşme sağlar
                        .PrefixLength(3) // İlk 3 karakterin tam eşleşmesini zorunlu kılar
                )
            )
        );

        if (!searchResponse.IsValid)
        {
            throw new Exception($"Search request failed: {searchResponse.ServerError.Error.Reason}");
        }

        return searchResponse.Documents.ToList();
    }

    public async Task<List<T>> SearchAsyncV2(
        string searchTerm,
        string fieldName,
        string collapseField,
        string filterString, // Örnek: "Ia=1,status=1,brand.Ia=1"
        int pageNumber = 1,
        int pageSize = 10)
    {
        int from = (pageNumber - 1) * pageSize;

        var searchResponse = await _client.SearchAsync<T>(s => s
            .From(from)
            .Size(pageSize)
            .Query(q => q
                .Bool(b => b
                    .Must(mustQueries =>
                    {
                        // Arama terimi için Match sorgusu
                        mustQueries.Match(m => m
                            .Field(fieldName)
                            .Query(searchTerm)
                            .Fuzziness(Fuzziness.Auto)
                            .PrefixLength(3)
                        );

                        // Filtre string'ini işle
                        if (!string.IsNullOrWhiteSpace(filterString))
                        {
                            // Filtreleri ayır (örneğin, "Ia=1,status=1,brand.Ia=1" -> ["Ia=1", "status=1", "brand.Ia=1"])
                            var filters = filterString.Split(',', StringSplitOptions.RemoveEmptyEntries);

                            foreach (var filter in filters)
                            {
                                // Filtreyi alan ve değer olarak ayır (örneğin, "Ia=1" -> ["Ia", "1"])
                                var parts = filter.Split('=', StringSplitOptions.RemoveEmptyEntries);
                                if (parts.Length != 2)
                                    throw new ArgumentException($"Invalid filter format: {filter}");

                                string field = parts[0].Trim();
                                string value = parts[1].Trim();

                                // Değeri uygun türe dönüştür (örneğin, "1" -> int, "true" -> bool)
                                object typedValue = value switch
                                {
                                    var v when int.TryParse(v, out var intVal) => intVal,
                                    var v when bool.TryParse(v, out var boolVal) => boolVal,
                                    _ => value // String olarak kalır
                                };

                                // Nested veya normal alan kontrolü
                                if (field.Contains("."))
                                {
                                    var fieldParts = field.Split('.');
                                    string nestedPath = fieldParts[0]; // Örneğin, "brand"
                                    string nestedField = field; // Örneğin, "brand.Ia"

                                    mustQueries.Nested(n => n
                                        .Path(nestedPath)
                                        .Query(nq => nq
                                            .Term(t => t
                                                .Field(nestedField)
                                                .Value(typedValue)
                                            )
                                        )
                                    );
                                }
                                else
                                {
                                    // Normal alanlar için Term sorgusu
                                    mustQueries.Term(t => t
                                        .Field(field)
                                        .Value(typedValue)
                                    );
                                }
                            }
                        }

                        return mustQueries;
                    })
                )
            )
            .Collapse(c => c
                .Field(collapseField)
                .InnerHits(ih => ih
                    .Name("latest")
                    .Size(1)
                )
            )
        );

        if (!searchResponse.IsValid)
        {
            throw new Exception($"Search request failed: {searchResponse.ServerError?.Error?.Reason}");
        }

        return searchResponse.Documents.ToList();
    }

}