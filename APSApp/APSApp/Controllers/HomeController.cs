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
    private static string _subscriptionKey = "319fbc4c03f34fb391db3718e7713ca6";
    private static string _endpoint = "https://apsapp.cognitiveservices.azure.com/";

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
            try
            {
                ComputerVisionClient client = Authenticate(_endpoint, _subscriptionKey);

                List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Tags,
                VisualFeatureTypes.Description
            };

                ImageAnalysis results = await client.AnalyzeImageAsync(model.ImageUrl, visualFeatures: features);

                return View("Result", results);
            }
            catch (ComputerVisionErrorResponseException ex) when (ex.Response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ModelState.AddModelError("", "The image URL is invalid. Please provide a valid URL and try again.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            }
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

