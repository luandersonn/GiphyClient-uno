using GiphyClient.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Uno.Extensions.Specialized;

namespace GiphyClient.WebApi
{
	public class GiphyApiClient
	{
		private readonly string _apiKey;
		private static readonly string _baseUrl = "https://api.giphy.com/v1/gifs/";
		// trending url with limit and offset
		private static readonly string _trendingUrl = "trending?api_key={0}&limit={1}&offset={2}";
		// search url with limit and offset
		private static readonly string _searchUrl = "search?api_key={0}&q={1}&limit={2}&offset={3}";
		// httpClient
		private readonly HttpClient _client;

		public GiphyApiClient(string apiKey)
		{
			_apiKey = apiKey;
#if __WASM__
			var innerHandler = new Uno.UI.Wasm.WasmHttpHandler();
#else
			var innerHandler = new HttpClientHandler();
#endif
			_client = new HttpClient(innerHandler);
		}

		public async Task<IEnumerable<Gif>> GetTrendingGifsAsync(int maxItems, int offset)
		{
			var requestUrl = string.Format(new CultureInfo("en-US"), _trendingUrl, _apiKey, maxItems, offset);
			var json = await GetAsync(requestUrl).ConfigureAwait(false);
			return ParseGifs(json);
		}

		public async Task<IEnumerable<Gif>> SearchGifsAsync(string searchTerm, int maxItems, int offset)
		{
			var requestUrl = string.Format(new CultureInfo("en-US"), _searchUrl, _apiKey, searchTerm, maxItems, offset);
			var json = await GetAsync(requestUrl).ConfigureAwait(false);
			return ParseGifs(json);
		}

		private static IEnumerable<Gif> ParseGifs(string json)
		{
			var document = JsonDocument.Parse(json);
			var root = document.RootElement;
			var data = root.GetProperty("data");
			foreach (var item in data.EnumerateArray())
			{
				var gif = new Gif
				{
					Title = item.GetProperty("title").GetString(),
					Username = item.GetProperty("username").GetString(),					
					Id = item.GetProperty("id").GetString(),
					Url = item.GetProperty("url").GetString(),
					OriginalUrl = item.GetProperty("images").GetProperty("original").GetProperty("url").GetString(),
					StaticUrl = item.GetProperty("images").GetProperty("original_still").GetProperty("url").GetString(),
					DownsizedUrl = item.GetProperty("images").GetProperty("downsized").GetProperty("url").GetString(),
					PreviewUrl = item.GetProperty("images").GetProperty("preview_gif").GetProperty("url").GetString()
				};
				yield return gif;
			}
		}

		// Insert CreateRequestMessage method below here
		private HttpRequestMessage CreateRequestMessage(HttpMethod method, string url, Dictionary<string, string> headers = null)
		{
			var uri = new Uri($"{_baseUrl}{url}");
			var httpRequestMessage = new HttpRequestMessage(method, uri);
			if (headers != null && headers.Any())
			{
				foreach (var header in headers)
				{
					httpRequestMessage.Headers.Add(header.Key, header.Value);
				}
			}
			return httpRequestMessage;
		}

		protected async Task<string> GetAsync(string url, Dictionary<string, string> headers = null)
		{
			using (var request = CreateRequestMessage(HttpMethod.Get, url, headers))
			using (var response = await _client.SendAsync(request))
			{
				return await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
			}
		}
	}
}
