// See https://aka.ms/new-console-template for more information

using activationist.app;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddTransient<IDependencyOne, DependencyOne>()
    .AddTransient<IDependencyTwo, DependencyTwo>();
var host = builder.Build();

var activator = new AugmentedActivator<Target>(host.Services);
activator.AddDependency("Don Pedro");
var target = activator.Create();

target?.Invoke();