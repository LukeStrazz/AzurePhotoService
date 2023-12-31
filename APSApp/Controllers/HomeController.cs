﻿using System.Diagnostics;
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
                List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>()
        {
            VisualFeatureTypes.Tags,
            VisualFeatureTypes.Description,
            VisualFeatureTypes.ImageType,
            VisualFeatureTypes.Faces,
            VisualFeatureTypes.Categories,
            VisualFeatureTypes.Color,
            VisualFeatureTypes.Adult
        };

                ImageAnalysis results = await _computerVisionClient.AnalyzeImageAsync(model.ImageUrl, visualFeatures: features);

                var viewModel = new
                {
                    AnalysisResult = results,
                    ImageUrl = model.ImageUrl
                };

                return View("Result", viewModel);
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

}

public static class HttpRequestExtensions
{
    public static bool IsAjaxRequest(this HttpRequest request)
    {
        return request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }
}

