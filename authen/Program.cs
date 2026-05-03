using authen.common.BaseClass;
using authen.system.web.MenuAndRole;
using authen.extensions;

var builder = WebApplication.CreateBuilder(args);

// ── Module controller registry ───────────────────────────────────────────────
ListController.list = [..SystemListController.list_controller];
foreach (var controller in SystemListController.list_controller)
{
    ListController.list_public.AddRange(controller.list_controller_action_public);
    ListController.list_not_public.AddRange(controller.list_controller_action_publicNonLogin);
}
builder.Services.AddSingleton(ListController.list_public);

// ── Service registrations ────────────────────────────────────────────────────
builder.Services
    // add System service likes: controller
    .addSystemServices(builder.Configuration)
    // config JWT service
    .addJWTService(builder.Configuration)
    // Config CORS with Angular
    .addCorsService()
    // Config MongoDB
    .addMongoDBService(builder.Configuration)
    .addCacheService(builder.Configuration);

// ── Middleware pipeline ──────────────────────────────────────────────────────
var app = builder.Build();

app.UseCors("AllowAngular");
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}.ctr/{action=Index}/{id?}");
app.MapFallbackToFile("index.html");

app.Run();
