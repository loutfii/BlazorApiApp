using System.Collections.Generic;

namespace BlazorApiApp.Models
{
    public class ApiResponse
    {
        // L’API renvoie un objet avec une liste de cartes
        public List<Card> data { get; set; } = new();
    }
}
