using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Empower;
using Empower.Document;
using Empower.Security;

namespace EmpowerClient
{
    internal static class Program
    {
        private static SecurityTokenResponse securityToken = null;

        private static async Task Main()
        {
            Console.WriteLine("Press Any Key");
            Console.ReadKey();

            await GetSecurityToken()
                .ConfigureAwait(false);

            await PostFile()
                .ConfigureAwait(false);

            await Export()
                .ConfigureAwait(false);

            Console.WriteLine("Done");
            Console.ReadKey();
        }



        private static async Task Export()
        {
            using HttpClient client = new HttpClient(new HttpClientHandler { UseProxy = false });

            client.DefaultRequestHeaders.Add(securityToken.Body.CsrfHeader, securityToken.Body.CsrfToken);

            var exportUri = Environment.GetEnvironmentVariable("ExportDocument");

            var uriString = exportUri.Replace("{id}", Guid.NewGuid().ToString());

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
        }

        private static async Task GetSecurityToken()
        {
            using HttpClient client = new HttpClient(new HttpClientHandler { UseProxy = false });

            var user = Environment.GetEnvironmentVariable("UserName");
            var password = Environment.GetEnvironmentVariable("Password");

            var authToken = Encoding
                .ASCII
                .GetBytes($"{user}:{password}");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(authToken));

            var response = await client
                .GetAsync(Environment.GetEnvironmentVariable("GetToken"))
                .ConfigureAwait(false);

            var result = await response
                .Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            securityToken = JsonSerializer.Deserialize<SecurityTokenResponse>(
                result,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true, });
        }

        private static async Task<bool> PostFile()
        {
            using HttpClient client = new HttpClient(new HttpClientHandler { UseProxy = false });

            client.DefaultRequestHeaders.Add(securityToken.Body.CsrfHeader, securityToken.Body.CsrfToken);

            var mpw = await File
                .ReadAllBytesAsync("test.mpw")
                .ConfigureAwait(false);

            var content = new MultipartFormDataContent
            {
                { new ByteArrayContent(mpw), "file", "test.mpw" },
                { new StringContent("51-1115057666-1531728594-0", Encoding.UTF8), "appId" },
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

            var importResponse = JsonSerializer.Deserialize<ImportResponse>(
                response,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true, });

            return importResponse.Header.Status.Code == Status.SUCCESS;
        }
    }
}
