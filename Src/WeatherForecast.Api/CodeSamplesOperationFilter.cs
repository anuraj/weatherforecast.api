using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace WeatherForecast.Api
{
public class CodeSamplesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var codeSamples = new OpenApiArray();
        if (!operation.Extensions.ContainsKey("x-codeSamples"))
        {
            operation.Extensions["x-codeSamples"] = codeSamples;
        }
        else
        {
            codeSamples = (OpenApiArray)operation.Extensions["x-codeSamples"];
        }

        if (operation.OperationId == "GetWeatherForecast")
        {
            codeSamples.Add(new OpenApiObject
            {
                ["lang"] = new OpenApiString("C#"),
                ["source"] = new OpenApiString("using System.Net.Http;\n\nvar client = " +
                "new HttpClient();\nvar response = await client.GetAsync(\"https://localhost:5001/weatherforecast\");\n" +
                "var content = await response.Content.ReadAsStringAsync();\nConsole.WriteLine(content);")
            });

            codeSamples.Add(new OpenApiObject
            {
                ["lang"] = new OpenApiString("Python"),
                ["source"] = new OpenApiString("import requests\n\nresponse = " +
                "requests.get('https://localhost:5001/weatherforecast')\nprint(response.text)")
            });
        }
    }
}
}
