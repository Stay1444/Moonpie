using System;
using Moonpie.Entities;
using Moonpie.Protocol.Protocol;
using Serilog;
using Serilog.Core;
using Tomlyn;
using Moonpie = Moonpie.Moonpie;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/moonpie.log", rollingInterval: RollingInterval.Hour)
    .CreateLogger();

if (!File.Exists("config.toml"))
{
    File.WriteAllText("config.toml", Toml.FromModel(new MoonpieConfiguration("127.0.0.1", 25565, 64)));
}

var config = Toml.ToModel<MoonpieConfiguration>(File.ReadAllText("config.toml"));

var moonpie = new global::Moonpie.Moonpie(config);
moonpie.Start();

await Task.Delay(-1);