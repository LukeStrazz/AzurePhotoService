using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using APSApp.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Net;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;

namespace APSApp.Controllers;

public class HomeController : Controller
{
    private readonly IComputerVisionClient _computerVisionClient;
    private static string _subscriptionKey = Environment.GetEnvironmentVariable("VISION_KEY");
    private static string _endpoint = Environment.GetEnvironmentVariable("VISION_ENDPOINT");

    public HomeController(IComputerVisionClient computerVisionClient)
    {
        _computerVisionClient = computerVisionClient;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> AnalyzeImage(ImageUpload model)
    {
        if (ModelState.IsValid)
        {
            ComputerVisionClient client = Authenticate(_endpoint, _subscriptionKey);

            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
        {
            VisualFeatureTypes.Tags
        };

            ImageAnalysis results = await client.AnalyzeImageAsync(model.ImageUrl, visualFeatures: features);

            if (Request.IsAjaxRequest())
            {
                return PartialView("ResultPartial", results);
            }

            return View("Result", results);
        }

        return View("Result", model);
    }
    public ComputerVisionClient Authenticate(string endpoint, string key)
    {
        ComputerVisionClient client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
        { Endpoint = endpoint };
        return client;
    }

}


public static class HttpRequestExtensions
{
    public static bool IsAjaxRequest(this HttpRequest request)
    {
        return request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }
}

