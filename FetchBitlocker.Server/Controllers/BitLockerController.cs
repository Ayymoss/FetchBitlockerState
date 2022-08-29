using System.Text.Json;
using FetchBitlocker.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FetchBitlocker.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BitLockerController : ControllerBase
{
    private string _apiKey => Environment.OSVersion.Platform == PlatformID.Unix
        ? Environment.GetEnvironmentVariable("FETCH_BL_API_KEY")
        : Environment.GetEnvironmentVariable("FETCH_BL_API_KEY", EnvironmentVariableTarget.User);

    [HttpPost]
    public ActionResult<string> Submit([FromBody] EndPointData endPointData)
    {
        var dataModel = endPointData.DataModel;
        if (endPointData.Key != _apiKey) return Unauthorized("Invalid API Key");
        if (dataModel.State == BitLockerState.Unknown) return BadRequest("State is unknown");

        var dataList = new List<DataModel>();
        lock (dataList)
        {
            if (System.IO.File.Exists("BitLockerExport.json"))
            {
                var json = System.IO.File.ReadAllText("BitLockerExport.json");
                dataList = JsonSerializer.Deserialize<List<DataModel>>(json);
            }

            if (dataList.Find(x => x.HostName == dataModel.HostName) == null)
            {
                dataList.Add(dataModel);
            }
            else
            {
                Console.WriteLine($"{dataModel.HostName} already exists!");
                return Ok($"{dataModel.HostName} already exists!");
            }

            var dataListJson = JsonSerializer.Serialize(dataList, new JsonSerializerOptions {WriteIndented = true});
            System.IO.File.WriteAllText("BitLockerExport.json", dataListJson);

            Console.WriteLine($"{dataModel.HostName} added!");
            return Ok($"{dataModel.HostName} added!");
        }
    }
}
