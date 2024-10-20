using OllamaSharp;
using OllamaSharp.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace RegexGenerator.Services
{
    public class OllamaService
    {
        public OllamaApiClient Client { get; }
        public OllamaService()
        {
            var uri = new Uri("http://localhost:11434");
            Client = new OllamaApiClient(uri);
            //Client.SelectedModel = "phi3:latest";
            Client.SelectedModel = "gemma2";
        }

        public async Task<string> Generate(string Text, string Prompt)
        {
            var template = $$"""
找出满足要求的正则表达式
- 匹配字符串: {{Text}}
- 要求: {{Prompt}}


- 你需要说的只有：regex:{结果正则表达式}
- 你不需要解释任何事情
- 你不需要说任何与正则表达式以外的任何字
- 全局匹配
""";

            StringBuilder stringBuilder = new();
            // 假设 OllamaApiClient 支持设置温度参数
            var generateRequest = new GenerateRequest
            {
                Prompt = template,
                Model = "gemma2",
                Stream = true,
                Context = [],
                Options = new RequestOptions
                {
                    Temperature = 0.15f
                }

            };
            //await foreach (var stream in _ollamaService.Client.Generate(prompt))
            //    stringBuilder.Append(stream?.Response);

            await foreach (var stream in Client.Generate(generateRequest))
                stringBuilder.Append(stream?.Response);


            return stringBuilder.ToString().Replace("regex:", "").Replace("regex\n", "").Trim().Trim('`').Trim('{').Trim('}');
        }

    }
}
