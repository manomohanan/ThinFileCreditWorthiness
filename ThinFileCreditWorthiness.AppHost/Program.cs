using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var metabasePort = builder.AddParameter("metabasePort", false);
var userName = builder.AddParameter("username", true);
var password = builder.AddParameter("password", true);

var metabasePortValue = Convert.ToInt32(metabasePort.Resource.Value);
var dbService = builder.AddPostgres("database", userName, password, 4000).WithPgAdmin().WithPgWeb();
var apiService = builder.AddProject<Projects.ThinFileCreditWorthiness_ApiService>("apiservice")
    .WithReference(dbService);
var metabaseService = builder.AddContainer("metabase", "metabase/metabase").WithHttpEndpoint(metabasePortValue, metabasePortValue);

builder.AddProject<Projects.ThinFileCreditWorthiness_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
