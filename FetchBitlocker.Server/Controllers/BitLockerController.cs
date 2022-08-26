using System.Text.Json;
using FetchBitlocker.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FetchBitlocker.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class BitLockerController : ControllerBase
{
    [HttpPost("BitLocker")]
    public ActionResult<string> BitLocker([FromBody] DataModel dataModel)
    {
        if (dataModel.State == BitLockerState.Unknown) return NoContent();
        
        var dataList = new List<DataModel>();
        lock (dataList)
        {
            if (System.IO.File.Exists("bitlocker.json"))
            {
                var json = System.IO.File.ReadAllText("bitlocker.json");
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
            System.IO.File.WriteAllText("bitlocker.json", dataListJson);

            Console.WriteLine($"{dataModel.HostName} added!");
            return Ok($"{dataModel.HostName} added!");
        }
    }
}
