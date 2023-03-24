using Postgrest.Models;
using Postgrest.Responses;

namespace Maid.Library.Interfaces;

public class MyModelResponse<T> where T : BaseModel, new()
{
    public List<T> Models { get;  set; } = new List<T>();

    public Postgrest.ClientOptions ClientOptons { get;  set; }
    
    public string? Content { get;  set; }

    public HttpResponseMessage ResponseMessage { get;  set; }

    public MyModelResponse()
    {
        
    }
    public MyModelResponse(ModeledResponse<T> modeledResponse)
    {
        Models = modeledResponse.Models;

        ClientOptons = modeledResponse.ClientOptions;

        Content = modeledResponse.Content;

        ResponseMessage = modeledResponse.ResponseMessage;
    }
}
