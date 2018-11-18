namespace TelegrammAspMvcDotNetCoreBot.Models
{
    public class Day
    {
        public int Id { get; set; }
        public Group Group { get; set; }
        public string Date { get; set; }
        public string HomeWorkText { get; set; }
    }
}