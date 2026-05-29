using ApiAggregator;

var builder = WebApplication
	.CreateBuilder(args)
	.AddModules()
	.AddServiceDefaults()
	.AddStandardServices();

var app = builder.Build();

app.UseExceptionHandler();
app.MapDefaultEndpoints();

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