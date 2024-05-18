namespace NutrionE.Models.DTOs.ChatGptResponsesDTO
{
    public class Choice
    {
        public string Text { get; set; }

        public int Index { get; set; }

        public string Logprobs { get; set; }

        public string FinishReason { get; set; }
    }
}
