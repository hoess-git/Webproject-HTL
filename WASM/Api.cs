using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace WASM
{
	public static class Api
	{
		static JsonSerializerOptions Options { get; } = new JsonSerializerOptions()
		{
			Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
			PropertyNameCaseInsensitive = true
		};

		public static async Task<T> Get<T>(string url, HttpClient client, CancellationToken token = default)
		{
			HttpResponseMessage response = await client.GetAsync(url, token);
			response.EnsureSuccessStatusCode();
			var responseBody = await response.Content.ReadAsStringAsync() ?? throw new ArgumentException("Response is null");
			return JsonSerializer.Deserialize<T>(responseBody, Options) ?? throw new ArgumentException($"Json could not deserialize this string: {responseBody}");
		}

		public static async Task<T> Post<T>(string url, object obj, HttpClient client, CancellationToken token = default)
		{
			string body = JsonSerializer.Serialize(obj, Options);
			var request = new HttpRequestMessage(HttpMethod.Post, url)
			{
				Content = new StringContent(body, Encoding.UTF8, "application/json")
			};
			var response = await client.SendAsync(request, token);
			response.EnsureSuccessStatusCode();
			var responseBody = response.Content.ReadAsStringAsync().Result ?? throw new ArgumentException("Reponse of api is null");
			return JsonSerializer.Deserialize<T>(responseBody, Options) ?? throw new ArgumentException($"Json could not deserialize this string: {responseBody}");
		}

		public static async Task<T> Put<T>(string url, object obj, HttpClient client, CancellationToken token = default)
		{
			string body = JsonSerializer.Serialize(obj, Options);
			var request = new HttpRequestMessage(HttpMethod.Put, url)
			{
				Content = new StringContent(body, Encoding.UTF8, "application/json")
			};
			var response = await client.SendAsync(request, token);
			var responseBody = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<T>(responseBody, Options) ?? throw new ArgumentException($"Json could not deserialize this string {responseBody}");
		}
	}
}
