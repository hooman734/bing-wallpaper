using HtmlAgilityPack;
using System.Text.RegularExpressions;

// Base URL of the Bing Website
string remoteUri = @"https://www.bing.com/hp/api/v1/imagegallery";

// Try to Scrape the Bing Website
HtmlWeb web = new HtmlWeb();
HtmlDocument doc = web.Load(remoteUri);

// Targeting the image container HTML Tag
var imageUri = doc.DocumentNode.SelectSingleNode("//div[@class='hero']/a[@class='img']").InnerHtml;

// Extracting the relative URL from the Tag
Regex rx = new Regex(@"src=""([^""]*)""", RegexOptions.Compiled | RegexOptions.IgnoreCase);
var match = rx.Match(imageUri);

// Making up the absolute URL of the image
string src = $@"https://www.bing.com{match.Groups[1].Value}";

// Reading the image and turning that into a Byte-Array
using var client = new HttpClient();
var buffer = await client.GetByteArrayAsync(src);
Console.WriteLine("Download done!");

// Checking whether the destination folder is available
if (!Directory.Exists("images")) Directory.CreateDirectory("images");

// Saving the image file into the destination with date specified name
await using var writer = new BinaryWriter(File.OpenWrite(@$"images/image-{DateOnly.FromDateTime(DateTime.Now).ToString("MM-dd-yyyy")}.jpeg"));
writer.Write(buffer);

// Log to Console
Console.WriteLine("Image file is saved successfully in $HOME/images directory.");
Console.WriteLine(@$"Source file is: {src}");