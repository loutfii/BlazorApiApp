using System.Collections.Generic;

namespace BlazorApiApp.Models
{
    public class Card
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
        public string desc { get; set; } = string.Empty;

        // Une carte peut avoir plusieurs images
        public List<CardImage> card_images { get; set; } = new();
    }
}
