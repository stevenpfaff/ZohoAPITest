using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using ZohoAPI.AppDbContext;
using ZohoAPI.Models;


{
    string filepath = "C:\\Code Projects\\ZohoAPI\\ZohoAPI\\config.json";
    string jsonData = File.ReadAllText(filepath);
    Config configData = JsonConvert.DeserializeObject<Config>(jsonData);
    // Console.WriteLine(configData.client_id);
    var options = new RestClientOptions("https://accounts.zoho.com")
    {
        MaxTimeout = -1,
    };
    var client = new RestClient(options);
    var request = new RestRequest("/oauth/v2/token?refresh_token=" + configData.refresh_token + "&client_id=" + configData.client_id + "&client_secret=" + configData.client_secret + "&grant_type=" + configData.grant_type, Method.Post);
    RestResponse response = await client.ExecuteAsync(request);

    // Check if the request was successful
    if (response.IsSuccessful)
    {
        // Deserialize the JSON response
        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(value: response.Content);

        // Access the access token
        string accessToken = tokenResponse.access_token;

        // Print the access token
        Console.WriteLine("Access Token: " + accessToken);

        var invOptions = new RestClientOptions("https://www.zohoapis.com")
        {
            MaxTimeout = -1,
        };
        var invClient = new RestClient(invOptions);
        var invRequest = new RestRequest("/crm/v6/Invoices/4996294000090071695", Method.Get);
        invRequest.AddHeader("Content-Type", "application/json");
        invRequest.AddHeader("Authorization", "Zoho-oauthtoken " + accessToken);
        RestResponse invResponse = await invClient.ExecuteAsync(invRequest);

        if (invResponse.IsSuccessful)
        {
            // Console.WriteLine("invResponse: " + invResponse.Content);
            var invJsonResp = JsonConvert.DeserializeObject<dynamic>(value: invResponse.Content);

            foreach (var val in invJsonResp)
            {
                // Console.WriteLine(val);
                foreach (var i in val)
                {
                    Console.WriteLine(i[0]);
                    Console.WriteLine(i[0]["Email"]);
                }
            }
        }
        else
        {
            Console.WriteLine("Error: " + invResponse.ErrorMessage);
        }


    }
    else
    {
        Console.WriteLine("Error: " + response.ErrorMessage);
    }
}
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AppSettingsDbContext, AppSettingsDbContext>();

builder.Services.AddDbContext<DbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppSettingsDbContext"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
