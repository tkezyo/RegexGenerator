using DynamicData;
using OllamaSharp;
using OllamaSharp.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RegexGenerator.Services;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text;
using System.Text.RegularExpressions;
using Ty.ViewModels;

namespace RegexGenerator.ViewModels
{
    public class GeneratorViewModel : ViewModelBase
    {
        private readonly OllamaService _ollamaService;

        public GeneratorViewModel(OllamaService ollamaService)
        {
            this._ollamaService = ollamaService;
            GenerateCommand = ReactiveCommand.CreateFromTask(Generate);
            ValidateCommand = ReactiveCommand.Create(Validate);
            Text = """
                Hello World
                Hello Regex
                """;
            Prompt = """
                Hello
                """;
            this.WhenAnyValue(c => c.Prompt).Subscribe(c => Errors?.Clear());
            this.WhenAnyValue(c => c.Text).Subscribe(c => Errors?.Clear());
        }
        public override async Task Activate()
        {
         await   GetModels();
        }
        [Reactive]
        public string? Text { get; set; }
        [Reactive]
        public string? Prompt { get; set; }
        [Reactive]
        public string? RegexText { get; set; }

        [Reactive]
        public string? Result { get; set; }

        public ReactiveCommand<Unit, Unit> GenerateCommand { get; }
        public async Task Generate()
        {
            if (string.IsNullOrWhiteSpace(SelectedModel) ||string.IsNullOrWhiteSpace(Text) || Text == "请输入文本")
            {
                Prompt = "请输入文本";
                return;
            }
            RegexText = await _ollamaService.Generate(Text, Prompt + "\r\n" + string.Join("\r\n", Errors), SelectedModel);
        }

        public List<string> Errors { get; set; } = [];

        public ObservableCollection<string> Results { get; set; } = [];

        public ReactiveCommand<Unit, bool> ValidateCommand { get; }
        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(RegexText))
            {
                Result = "请输入文本";
                return false;
            }
            try
            {
                var regex = new Regex(RegexText);
                var match = regex.Match(Text);
                if (match.Success)
                {
                    Result = "匹配成功";
                }
                else
                {
                    Result = "匹配失败";
                    Errors.Add($" - {RegexText} 不符合规则");
                }
                Results.Clear();
                foreach (Match m in regex.Matches(Text))
                {
                    foreach (var item in m.Groups)
                    {
                        Results.Add(item.ToString());
                    }
                }
                return match.Success;
            }
            catch (ArgumentException)
            {
                Result = "无效的正则表达式";
            }
            return false;
        }

        [Reactive]
        public string? SelectedModel { get; set; }
        public ObservableCollection<string> Models { get; set; } = [];

        public async Task GetModels()
        {
            Models.Clear();
            var models = await _ollamaService.GetModels();
            foreach (var item in models)
            {
                Models.Add(item);
            }
        }


    }
}
