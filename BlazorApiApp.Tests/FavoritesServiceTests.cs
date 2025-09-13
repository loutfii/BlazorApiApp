using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorApiApp.Models;
using BlazorApiApp.Services;
using Moq;
using Xunit;

namespace BlazorApiApp.Tests
{
    public class FavoritesServiceTests
    {
        // Helper pour créer un service avec un stockage en mémoire simulé
        private static (FavoritesService service, List<Card> store, Mock<ILocalStorageService> mock) CreateServiceWithStore(List<Card>? initial = null)
        {
            var store = initial ?? new List<Card>();

            var mockLocalStorage = new Mock<ILocalStorageService>();

            // Simuler la lecture du LocalStorage
            mockLocalStorage
                .Setup(ls => ls.GetItemAsync<List<Card>>("favorites", It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => store);

            // Simuler l'écriture dans le LocalStorage
            mockLocalStorage
                .Setup(ls => ls.SetItemAsync("favorites", It.IsAny<List<Card>>(), It.IsAny<CancellationToken>()))
                .Callback<string, List<Card>, CancellationToken>((_, list, __) => store = list)
                .Returns(new ValueTask());

            var service = new FavoritesService(mockLocalStorage.Object);
            return (service, store, mockLocalStorage);
        }

        //Test 1 : Vérifie que si aucun favoris n'est enregistré,
        // le service retourne une liste vide (et non pas null).
        [Fact]
        public async Task GetFavorites_ShouldReturnEmptyList_WhenStorageIsNull()
        {
            // Arrange : on configure un LocalStorage qui retourne null
            var mockLocalStorage = new Mock<ILocalStorageService>();
            mockLocalStorage
                .Setup(ls => ls.GetItemAsync<List<Card>>("favorites", It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<Card>?)null);

            var service = new FavoritesService(mockLocalStorage.Object);

            // Act : appel de la méthode
            var result = await service.GetFavorites();

            // Assert : on vérifie que la liste est bien initialisée et vide
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        // Test 2 : Vérifie qu'ajouter une carte qui n'existe pas encore
        // la place correctement dans la liste des favoris.
        [Fact]
        public async Task AddFavorite_ShouldAddCard_WhenNotExisting()
        {
            // Arrange : service avec liste vide
            var (service, store, _) = CreateServiceWithStore();
            var card = new Card { id = 1, name = "Dark Magician" };

            // Act : ajout de la carte
            await service.AddFavorite(card);

            // Assert : une seule carte en favoris, et c'est la bonne
            Assert.Single(store);
            Assert.Equal(1, store.First().id);
        }

        // Test 3 : Vérifie que si une carte est déjà dans les favoris,
        // elle n'est pas ajoutée une seconde fois (pas de doublon).
        [Fact]
        public async Task AddFavorite_ShouldNotDuplicate_WhenCardAlreadyExists()
        {
            // Arrange : on initialise avec une carte déjà en favoris
            var initial = new List<Card> { new Card { id = 1, name = "Dark Magician" } };
            var (service, store, _) = CreateServiceWithStore(initial);
            var duplicate = new Card { id = 1, name = "Dark Magician" };

            // Act : tentative d'ajout de la même carte
            await service.AddFavorite(duplicate);

            // Assert : toujours une seule carte dans la liste
            Assert.Single(store); // toujours 1 carte
            Assert.Equal(1, store.First().id);
        }

        // Test 4 : Vérifie que lorsqu'une carte existe dans les favoris,
        // on peut bien la retirer
        [Fact]
        public async Task RemoveFavorite_ShouldRemove_WhenCardExists()
        {
            // Arrange : on commence avec deux cartes 
            var initial = new List<Card>
            {
                new Card { id = 1, name = "Dark Magician" },
                new Card { id = 2, name = "Blue-Eyes White Dragon" }
            };
            var (service, store, _) = CreateServiceWithStore(initial);

            // Act : on supprime la carte avec id = 1
            await service.RemoveFavorite(1);

             // Assert : il reste une seule carte id = 2
            Assert.Single(store);
            Assert.Equal(2, store.First().id);
        }

        // Test 5 : Vérifie que si on essaie de supprimer une carte
        // qui n'existe pas, la liste reste inchangée.
        [Fact]
        public async Task RemoveFavorite_ShouldDoNothing_WhenCardDoesNotExist()
        {
            // Arrange : on commence avec une seule carte (id = 2)
            var initial = new List<Card> { new Card { id = 2, name = "Blue-Eyes White Dragon" } };
            var (service, store, _) = CreateServiceWithStore(initial);

            // Act : on tente de supprimer une carte qui n'est pas là
            await service.RemoveFavorite(999);

            // Assert : la liste contient toujours la même carte
            Assert.Single(store);
            Assert.Equal(2, store.First().id);
        }
    }
}