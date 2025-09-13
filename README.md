# BlazorApiApp – Yu-Gi-Oh! Cards

## Objectif

Projet d’examen de dispense pour le cours **Programmation orientée objet**.  
L’application a été développée avec **Blazor WebAssembly (C#)** et permet de :

- Afficher des cartes de l’API Yu-Gi-Oh! (`https://db.ygoprodeck.com/api/v7/cardinfo.php`)
- Naviguer entre les cartes (carte principale → liste de carte avec miniature → détails)
- Ajouter ou retirer des cartes en **favoris**
- Sauvegarder les favoris en **LocalStorage** (persistants même après fermeture du navigateur)

---

## Technologies utilisées

- **.NET 8 SDK** (Blazor WebAssembly)
- **C#** pour la logique orientée objet
- **HttpClient** pour la consommation de l’API
- **Blazored.LocalStorage** pour la gestion des favoris
- **Flexbox (CSS)** pour la mise en page responsive
- **xUnit** – Framework de tests unitaires
- **Moq** – Framework de mock pour simuler le LocalStorage
- **Microsoft.NET.Test.Sdk** – Outils nécessaires à l’exécution des tests

---

## Lancement

### Création d’une app Blazor WebAssembly

dotnet new blazorwasm -o BlazorApiApp

### Se placer dans le dossier

cd BlazorApiApp

### Installation du package LocalStorage

dotnet add package Blazored.LocalStorage

### Lancement du serveur local

dotnet run

---

## Fonctionnalités principales

### Page Home

- Liste de **10 carte (avec miniature)** (clic → met à jour la carte principale affiché en grand)
- **Grande carte** affichée au centre
- **Étoile cliquable** pour ajouter/retirer des favoris
- Clic sur une carte → redirection vers la page **Details**

### Page Details

- Affichage d’une carte avec **description**
- Boutons **Previous / Next** pour naviguer
- **Étoile cliquable** (ajout/retrait des favoris)

### Page Favorites

- Affiche les cartes sauvegardées en **favoris**
- Possibilité de **retirer une carte** via le bouton _Remove_

---

## Tests

L’application est accompagnée d’un projet de tests automatisés (`BlazorApiApp.Tests`) basé sur **xUnit** et **Moq**.

Les tests couvrent principalement le `FavoritesService` qui gère les favoris :

- récupération des favoris (liste vide si aucun élément),
- ajout d’une carte aux favoris,
- vérification qu’un doublon n’est pas ajouté,
- suppression d’une carte existante,
- gestion du cas où la carte à supprimer n’existe pas.

### Lancer les tests

Depuis le dossier du projet (`BlazorApiApp`), exécuter :

dotnet test BlazorApiApp.sln

## Auteur

Ghazal Loutfi Adonis
