using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Task1.Data;
using Task1.Dtos;
using Xunit;

namespace Task1.Tests;

public class CustomersApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public CustomersApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task ResetDbAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
    }

    [Fact]
    public async Task Test_Bad_Email_Returns_400()
    {
        await ResetDbAsync();

        var req = new CreateCustomerRequest
        {
            Name = "Test",
            Email = "example" // invalid
        };

        var res = await _client.PostAsJsonAsync("/api/customers", req);

        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Test_Valid_Email_Returns_201()
    {
        await ResetDbAsync();

        var req = new CreateCustomerRequest
        {
            Name = "Sara",
            Email = "example@example.com"
        };

        var res = await _client.PostAsJsonAsync("/api/customers", req);

        Assert.Equal(HttpStatusCode.Created, res.StatusCode);
    }

    [Fact]
    public async Task Test_Name_Too_Long_Returns_400()
    {
        await ResetDbAsync();

        var req = new CreateCustomerRequest
        {
            Name = new string('A', 51),
            Email = "a@b.com"
        };

        var res = await _client.PostAsJsonAsync("/api/customers", req);

        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task Test_GetById_NotFound_Returns_404()
    {
        await ResetDbAsync();

        var res = await _client.GetAsync("/api/customers/9999");

        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    [Fact]
    public async Task Test_Create_Then_GetAll_Returns_200_And_Not_Empty()
    {
        await ResetDbAsync();

        // 1) Create
        var createReq = new CreateCustomerRequest
        {
            Name = "Yasmeen",
            Email = "yasmeen@site.com"
        };

        var createRes = await _client.PostAsJsonAsync("/api/customers", createReq);
        Assert.Equal(HttpStatusCode.Created, createRes.StatusCode);

        // 2) Get all
        var getRes = await _client.GetAsync("/api/customers");
        Assert.Equal(HttpStatusCode.OK, getRes.StatusCode);

        // 3) Count items in returned JSON array
        var json = await getRes.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
        Assert.True(doc.RootElement.GetArrayLength() > 0);
    }
}
