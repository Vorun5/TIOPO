using System;
using System.IO;
using links;

Console.Write("Enter link: ");
var link = Console.ReadLine();

// const string link = "http://links.qatl.ru/";
// const string link = "https://www.yola-mkt.ru/";

var lc = new LinkChecker(link);
var startDate = DateTime.Now;
await lc.CheckAllDomainLinks();
var finalDate = DateTime.Now; 


const string validLinksPath = "valid_travelline.txt";
const string invalidLinksPath = "invalid_travelline.txt";


await using (var writer = new StreamWriter(validLinksPath, false))
{
    foreach (var l in lc.ValidLinks)
    {
        await writer.WriteLineAsync($"{l.Key}\t{l.Value}");
    }
    
    await writer.WriteLineAsync($"Count: {lc.ValidLinks.Count} | Start: {startDate.Hour}:{startDate.Minute}:{startDate.Second} | Final: {finalDate.Hour}:{finalDate.Minute}:{finalDate.Second}");
}

await using (var writer = new StreamWriter(invalidLinksPath, false))
{
    foreach (var l in lc.InvalidLinks)
    {
        await writer.WriteLineAsync($"{l.Key}\t{l.Value}");
    }
    await writer.WriteLineAsync($"Count: {lc.InvalidLinks.Count} | Start: {startDate.Hour}:{startDate.Minute}:{startDate.Second} | Final: {finalDate.Hour}:{finalDate.Minute}:{finalDate.Second}");
}

Console.WriteLine("END");
Console.ReadLine();