using System.Text;
using LandingZones.Tools.BicepDocs.Destination.Confluence.Abstractions;
using LandingZones.Tools.BicepDocs.Destination.Confluence.Dto;
using Newtonsoft.Json;

namespace LandingZones.Tools.BicepDocs.Destination.Confluence.Services;

public class ConfluenceService : IConfluenceService
{
    private readonly HttpClient _httpClient;

    public ConfluenceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private static string GetUriWithQueryString(string requestUri,
        Dictionary<string, string> queryStringParams)
    {
        var startingQuestionMarkAdded = false;
        var sb = new StringBuilder();
        sb.Append(requestUri);
        foreach (var parameter in queryStringParams)
        {
            sb.Append(startingQuestionMarkAdded ? '&' : '?');
            sb.Append(parameter.Key);
            sb.Append('=');
            sb.Append(parameter.Value);
            startingQuestionMarkAdded = true;
        }

        return sb.ToString();
    }

    private HttpRequestMessage CreateMessage(HttpMethod method, string endpoint)
    {
        var uriBuilder = "";
        var m = new HttpRequestMessage(method, endpoint)
        {
        };

        return m;
    }

    public async Task<PageDto?> GetPageById(int pageId)
    {
        var message = CreateMessage(HttpMethod.Get, Endpoints.GetPageById(pageId));
        var response = await _httpClient.SendAsync(message);

        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(content))
        {
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = JsonConvert.DeserializeObject<ConfluenceError>(content);
            throw new Exception(errorBody.Message);
        }


        return JsonConvert.DeserializeObject<PageDto>(content);
    }

    // public Task SomeOne()
    // {
    //     Uri Url = new Uri(string.Format("{0}/wiki/rest/api/content/{1}/child/attachment", DomaineUrl, pageId));
    //     //string urlPath = ConfigurationSettings.AppSettings.Get("JIRAURLForCreateIssue").ToString();
    //     //Uri webService = new Uri(string.Format("{0}/{1}/attachments", urlPath, IssueKey));
    //     HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, Url);
    //     requestMessage.Headers.ExpectContinue = false;
    //
    //     MultipartFormDataContent multiPartContent = new MultipartFormDataContent("JustBoundary");
    //     ByteArrayContent byteArrayContent = new ByteArrayContent(fileContents);
    //     byteArrayContent.Headers.Add("Content-Type", "image/png");
    //     byteArrayContent.Headers.Add("X-Atlassian-Token", "no-check");
    //     multiPartContent.Add(byteArrayContent, "file", fi.Name);
    //     requestMessage.Content = multiPartContent;
    //
    //     HttpClient httpClient = new HttpClient();
    //     httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
    //         ASCIIEncoding.ASCII.GetBytes(
    //             string.Format("{0}:{1}", UserName, Password))));
    //     httpClient.DefaultRequestHeaders.Add("X-Atlassian-Token", "no-check");
    //     try
    //     {
    //         Task<HttpResponseMessage> httpRequest = httpClient.SendAsync(requestMessage,
    //             HttpCompletionOption.ResponseContentRead, CancellationToken.None);
    //         HttpResponseMessage httpResponse = httpRequest.Result;
    //         HttpStatusCode statusCode = httpResponse.StatusCode;
    //         HttpContent responseContent = httpResponse.Content;
    //
    //         if (responseContent != null)
    //         {
    //             Task<String> stringContentsTask = responseContent.ReadAsStringAsync();
    //             String stringContents = stringContentsTask.Result;
    //         }
    //
    //
    //         return new OkObjectResult(responseMessage);
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.Message);
    //         return new OkObjectResult(responseMessage);
    //     }
    // }
}