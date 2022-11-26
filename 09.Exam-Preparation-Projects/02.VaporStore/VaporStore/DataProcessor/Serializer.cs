namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
			List<GamesExportDto> games = new List<GamesExportDto>();

			var gamesToProcess = context
				.Games
				.AsQueryable()
				.Where(g => genreNames.Contains(g.Genre.Name))
				.Where(g => g.Purchases.Any())
				.Include(g => g.GameTags)
				.ThenInclude(gt => gt.Tag)
				.Include(g => g.Purchases)
				.Include(g => g.Genre)
				.ToList();

			foreach (string genre in genreNames)
			{
				var genreGames = gamesToProcess
					.Where(g => g.Genre.Name == genre)
					.ToList();

				if (genreGames.Count == 0)
				{
					continue;
				}

				var result = new GamesExportDto()
				{
					Id = genreGames.First().Id,
					Genre = genreGames.First().Name,
					Games = genreGames
							.Select(g => new GameExDto()
							{
								Id = g.Id,
								Developer = g.Developer.Name,
								Title = g.Name,
								Tags = string.Join(", ", g.GameTags.Select(t =>										t.Tag.Name)),
								Players = g.Purchases.Count
							})
							.OrderByDescending(g => g.Players)
							.ThenBy(g => g.Id)
							.ToArray()

				};

				result.TotalPlayers = result.Games.Sum(g => g.Players);

				games.Add(result);
			}

			games = games
				.OrderByDescending(g => g.TotalPlayers)
				.ThenBy(g => g.Id)
				.ToList();

			//DefaultContractResolver contractResolver = new DefaultContractResolver
			//{
			//	NamingStrategy = new CamelCaseNamingStrategy()
			//};

			//var jsonSettings = new JsonSerializerSettings
			//{
			//	Formatting = Formatting.Indented,
			//	ContractResolver = contractResolver
			//};

			//string gamesAsJson = JsonConvert.SerializeObject(games, jsonSettings);

			return JsonConvert.SerializeObject(games, Formatting.Indented) + " ";

		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
			List<UserEXportDto> users = new List<UserEXportDto>();

			PurchaseType purchaseType = (PurchaseType)Enum.Parse(typeof(PurchaseType), storeType);

			var usersToProcess = context
				.Purchases
				.AsQueryable()
				.Where(p => p.Type == purchaseType)
				.Include(p => p.Game.Genre)
				.Include(p => p.Card.User)
				.ToList()
				.GroupBy(p => p.Card.User.Username)
				.ToList();

			foreach (var user in usersToProcess)
			{
				var result = new UserEXportDto()
				{
					username = user.Key,
					Purchases = user
								.OrderBy(p => p.Date)
								.Select(p => new UserPurchase()
								{
									Card = p.Card.Number,
									Cvc = p.Card.Cvc,
									Date = p.Date.ToString("yyyy-MM-dd HH:mm"),
									Game = new UserPurchaseGame()
									{
										Genre = p.Game.Genre.Name,
										Price = p.Game.Price,
										title = p.Game.Name
									}
								})
								.ToArray()
				};

				result.TotalSpent = result
					.Purchases
					.Select(p => p.Game.Price)
					.Sum();
				users.Add(result);
			}

			users = users
				.OrderByDescending(u => u.TotalSpent)
				.ThenBy(u => u.username)
				.ToList();


			XmlRootAttribute root = new XmlRootAttribute("Users");

			XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
			namespaces.Add(string.Empty, string.Empty);

			XmlSerializer serializer = new XmlSerializer(typeof(UserEXportDto[]), root);

			StringBuilder sb = new StringBuilder();

			using (StringWriter sw = new StringWriter(sb))
			{
				serializer.Serialize(sw, users.ToArray(), namespaces);
			}

			return sb.ToString();

		}
	}
}