using Dapper;
using Microsoft.AspNetCore.HttpOverrides;
using VocTrainer.Components;
using VocTrainer.Services;

namespace VocTrainer;

public class Program
{
    public static void Main(string[] args)
    {
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<VocService>();
        builder.Services.AddSingleton<PinAuthService>();
        builder.Services.AddScoped<PinAuthState>();

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        var app = builder.Build();

        app.UseForwardedHeaders();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}