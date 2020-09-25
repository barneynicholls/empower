using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Empower.Applications;
using Empower.Document;
using Empower.Security;

namespace EmpowerClient
{
    internal static class Program
    {
        private static async Task Main()
        {
            Console.WriteLine("Press Any Key");
            Console.ReadKey();

            var securityToken = await GetSecurityToken()
                .ConfigureAwait(false);

            CreateAppResponse app = await CreateApp(securityToken)
                .ConfigureAwait(false);

            var import = await PostFile(securityToken, app.Body.App.AppId)
                .ConfigureAwait(false);

            await Export(securityToken, import.Body.Document.DocId)
                .ConfigureAwait(false);

            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static void AddHeaders(HttpClient httpClient, SecurityToken securityToken)
        {
            httpClient.DefaultRequestHeaders.Add(securityToken.Body.CsrfHeader, securityToken.Body.CsrfToken);
            AddAuthorizationHeaders(httpClient);
        }

        private static void AddAuthorizationHeaders(HttpClient httpClient)
        {
            var user = Environment.GetEnvironmentVariable("UserName");
            var password = Environment.GetEnvironmentVariable("Password");

            var authToken = Encoding
                .ASCII
                .GetBytes($"{user}:{password}");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(authToken));
        }

        private static async Task<CreateAppResponse> CreateApp(SecurityToken securityToken)
        {
            using HttpClient client = new HttpClient(new HttpClientHandler { UseProxy = false });

            AddHeaders(client, securityToken);

            var mpw = await File
                .ReadAllBytesAsync("test.mpw")
                .ConfigureAwait(false);

            var content = new MultipartFormDataContent
            {
                { new StringContent("1.1.0.7948", Encoding.UTF8), "editorVersion" },
                { new ByteArrayContent(mpw), "file", "test.mpw" },
                { new StringContent("PdfPreview.pub", Encoding.UTF8), "previewPubFile" },
                { new StringContent("role1", Encoding.UTF8), "roleName" },
                { new StringContent("role2", Encoding.UTF8), "roleName" }
            };

            var responseMessage = await client
                .PostAsync(Environment.GetEnvironmentVariable("CreateApp"), content)
                .ConfigureAwait(false);

            var response = await responseMessage
                .Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            return JsonSerializer.Deserialize<CreateAppResponse>(
                response,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true, });
        }

        private static async Task<string> Export(SecurityToken securityToken, string documentId)
        {
            using HttpClient client = new HttpClient(new HttpClientHandler { UseProxy = false });

            AddHeaders(client, securityToken);

            var exportUri = Environment.GetEnvironmentVariable("ExportDocument");

            var uriString = exportUri.Replace("{id}", documentId);

            var response = await client
                .PostAsync(uriString, new StringContent(""))
                .ConfigureAwait(false);

            var bytes = await response
                .Content
                .ReadAsByteArrayAsync()
                .ConfigureAwait(false);

            var filename = response.Content.Headers.ContentDisposition.FileName;

            await File
                .WriteAllBytesAsync(filename, bytes)
                .ConfigureAwait(false);

            return filename;
        }

        private static async Task<SecurityToken> GetSecurityToken()
        {
            using HttpClient client = new HttpClient(new HttpClientHandler { UseProxy = false });

            AddAuthorizationHeaders(client);

            var response = await client
                .GetAsync(Environment.GetEnvironmentVariable("GetToken"))
                .ConfigureAwait(false);

            var result = await response
                .Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            return JsonSerializer.Deserialize<SecurityToken>(
                result,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true, });
        }

        private static async Task<ImportResponse> PostFile(SecurityToken securityToken, string appId)
        {
            using HttpClient client = new HttpClient(new HttpClientHandler { UseProxy = false });

            AddHeaders(client, securityToken);

            var mpw = await File
                .ReadAllBytesAsync("test.mpw")
                .ConfigureAwait(false);

            var content = new MultipartFormDataContent
            {
                { new ByteArrayContent(mpw), "file", "test.mpw" },
                { new StringContent(appId, Encoding.UTF8), "appId" },
                { new StringContent("customer1", Encoding.UTF8), "busDocId" },
                { new StringContent("owner1", Encoding.UTF8), "ownerId" }
            };

            var tags = new List<string>
            {
                "tag1",
                "tag2"
            };

            foreach (var tag in tags)
                content.Add(new StringContent(tag, Encoding.UTF8), "docTag");

            var responseMessage = await client
                .PostAsync(Environment.GetEnvironmentVariable("ImportDocument"), content)
                .ConfigureAwait(false);

            var response = await responseMessage
                .Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            return JsonSerializer.Deserialize<ImportResponse>(
                response,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true, });
        }
    }
}
