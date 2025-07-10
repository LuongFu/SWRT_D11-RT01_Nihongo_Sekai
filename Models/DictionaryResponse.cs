namespace JapaneseLearningPlatform.Models
{
    public class JLPTResponse
    {
        public int Total { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public List<DictionaryWord> Words { get; set; }
    }

}
