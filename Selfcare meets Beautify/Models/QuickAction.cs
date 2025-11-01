namespace Selfcare_meets_Beautify.Models
{
    public class QuickAction
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Badge { get; set; }
        public string BadgeColor { get; set; } = "primary";
    }
}
