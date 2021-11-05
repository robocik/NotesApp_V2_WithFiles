using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Newtonsoft.Json;
using NoteBookApp.Shared;
using NoteBookApp.Shared.Exceptions;

namespace NoteBookApp.Client.Services
{
    public abstract class DataServiceBase
    {
        private readonly HttpClient _httpClient;

        protected DataServiceBase(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected JsonSerializerOptions CreateOptions()
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            return options;
        }

        protected string GetUrl(string urlPart, object? param = null)
        {
            var query = param?.ToQueryString();
            var url = urlPart;
            if (!string.IsNullOrEmpty(query))
            {
                url += "?" + query;
            }
            return url;
        }

        protected async Task<T> Execute<T>(Func<HttpClient, Task<T>> method)
        {
            T? res = default;
            try
            {
                res = await method(_httpClient).ConfigureAwait(false);
                if (res is HttpResponseMessage response)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        await ConvertToException(response).ConfigureAwait(false);
                    }
                }
            }
            catch (AccessTokenNotAvailableException e)
            {
                e.Redirect();
            }
            catch (HttpRequestException e)
            {

                convertToException(e.StatusCode, e.Message);
            }

            return res!;
        }
        

        protected async Task ConvertToException(HttpResponseMessage response)
        {
            var message = await response.Content.ReadAsStringAsync();
            var errorDetails = JsonConvert.DeserializeObject<ErrorDetails>(message);
            convertToException(response.StatusCode, errorDetails?.Message, errorDetails?.ServiceError);
        }

        private void convertToException(HttpStatusCode? statusCode, string? message, ServiceError? error = ServiceError.Unknown)
        {
            error ??= ServiceError.Unknown;
            message ??= "Unknown exception occured";
            switch (error)
            {
                case ServiceError.ObjectNotFoundException:
                    throw new ObjectNotFoundException(message);
                case ServiceError.ConstraintException:
                    throw new ConstraintException(message);
                case ServiceError.UniqueException:
                    throw new UniqueException(message);
                case ServiceError.UnauthorizedAccessException:
                    throw new UnauthorizedAccessException(message);
                case ServiceError.ArgumentNullException:
                    throw new ArgumentNullException(message);
                case ServiceError.InvalidOperationException:
                    throw new InvalidOperationException(message);
                case ServiceError.ArgumentOutOfRangeException:
                    throw new ArgumentOutOfRangeException(message);
                
            }
            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    throw new ObjectNotFoundException(message);
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException(message);
            }
            throw new Exception(message);
        }
    }
}