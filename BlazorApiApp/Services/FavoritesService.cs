using System.Collections.Generic;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorApiApp.Models;

namespace BlazorApiApp.Services
{
    public class FavoritesService
    {
        private readonly ILocalStorageService _localStorage;
        private const string Key = "favorites";

        public FavoritesService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<List<Card>> GetFavorites()
        {
            var list = await _localStorage.GetItemAsync<List<Card>>(Key);
            return list ?? new List<Card>();
        }

        public async Task AddFavorite(Card card)
        {
            var favorites = await GetFavorites();
            if (!favorites.Exists(c => c.id == card.id))
            {
                favorites.Add(card);
                await _localStorage.SetItemAsync(Key, favorites);
            }
        }

        public async Task RemoveFavorite(int id)
        {
            var favorites = await GetFavorites();
            favorites.RemoveAll(c => c.id == id);
            await _localStorage.SetItemAsync(Key, favorites);
        }

        public async Task<bool> IsFavorite(int id)
        {
            var favorites = await GetFavorites();
            return favorites.Exists(c => c.id == id);
        }
    }
}
