namespace authen.extensions
{
    public static class addSwaggerUIExtension
    {
        public static WebApplication addUISwagger(this WebApplication app, IServiceCollection services)
        {

            //services.AddEndpointsApiExplorer();
            //services.AddSwaggerGen();


            ////Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger(option =>
            //    {
            //        // documentName = v1
            //        option.RouteTemplate = "{documentName}/swagger.json";
            //    });
            //    app.UseSwaggerUI(option =>
            //    {
            //        option.SwaggerEndpoint("_api/v1/swagger.json", "AspNetAngularSamePort API");
            //        option.RoutePrefix = "swagger";
            //    });
            //}
            return app;
        }
    }
}
