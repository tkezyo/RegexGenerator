using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ty;
using Ty.Views.CustomPages;
using Ty.Views;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Ty.ViewModels;
using RegexGenerator.ViewModels;
using RegexGenerator.Views;
using System.Drawing;
using RegexGenerator.Services;

namespace RegexGenerator
{
    public class RegexGeneratorModule : Ty.ModuleBase
    {
        public override Task ConfigureServices(IHostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<App>();
            builder.Services.AddHostedService<WpfHostedService<App, MainWindow>>();
            builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            builder.Services.AddSingleton<OllamaService>();
            builder.Services.AddTransientView<GeneratorViewModel, GeneratorView>();


            builder.Services.Configure<PageOptions>(options =>
            {
                options.FirstLoadPage = typeof(LayoutViewModel);
                options.Title = "配置编辑器";
            });
            builder.Services.Configure<MenuOptions>(options =>
            {
                options.Menus.Add(new MenuInfo { DisplayName = "ttt", GroupName = "ttt", Name = "Menu.345", Color = Color.Black, ViewModel = typeof(GeneratorViewModel) });
                options.Menus.Add(new MenuInfo { DisplayName = "123", GroupName = "123", Name = "Menu.123", Color = Color.Black });
                options.Menus.Add(new MenuInfo { DisplayName = "345", GroupName = "345", Name = "Menu.123.345" });

            });
            builder.Services.Configure<CustomPageOption>(options =>
            {
                options.RootPath = "D:\\CustomPage";
                options.Name = "name";
            });



            return Task.CompletedTask;
        }
        public override void DependsOn()
        {
            AddDepend<TyWPFBaseModule>();
        }
    }
}
