namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportGamesDto[] games = JsonConvert.DeserializeObject<ImportGamesDto[]>(jsonString);

            List<Game> dbGames = new List<Game>();
            List<Developer> devs = new List<Developer>();
            List<Genre> genres = new List<Genre>();
            List<Tag> tags = new List<Tag>();

            foreach (ImportGamesDto game in games)
            {
                if (!IsValid(game))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (!DateTime.TryParse(game.ReleaseDate,
                              CultureInfo.InvariantCulture,
                              DateTimeStyles.None,
                              out DateTime date))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }


                Game g = new Game()
                {
                    Name = game.Name,
                    Price = game.Price,
                    ReleaseDate = date
                };

                Developer dev = devs.FirstOrDefault(d => d.Name == game.Developer);

                if (dev == null)
                {
                    dev = new Developer() { Name = game.Developer };
                    devs.Add(dev);
                }

                g.Developer = dev;

                Genre gen = genres.FirstOrDefault(d => d.Name == game.Genre);

                if (gen == null)
                {
                    gen = new Genre() { Name = game.Genre };
                    genres.Add(gen);
                }

                g.Genre = gen;

                foreach (var tag in game.Tags)
                {
                    Tag dbTag = tags.FirstOrDefault(d => d.Name == tag);

                    if (dbTag == null)
                    {
                        dbTag = new Tag() { Name = tag };
                        tags.Add(dbTag);
                    }

                    g.GameTags.Add(new GameTag() { Tag = dbTag });
                }

                dbGames.Add(g);
                sb.AppendLine($"Added {game.Name} ({game.Genre}) with {game.Tags.Length} tags");
            }

            context.Games.AddRange(dbGames);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportUsersDto[] users = JsonConvert.DeserializeObject<ImportUsersDto[]>(jsonString);

            List<User> usersList = new List<User>();

            foreach (ImportUsersDto user in users)
            {
                bool hasInvalidCard = false;

                if (!IsValid(user))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                User dbUser = new User()
                {
                    FullName = user.FullName,
                    Username = user.Username,
                    Email = user.Email,
                    Age = user.Age
                };

                foreach (var card in user.Cards)
                {
                    string[] validTypes = new string[] { "Debit", "Credit" };

                    if (!IsValid(card) || validTypes.Any(t => t == card.Type) == false)
                    {
                        hasInvalidCard = true;
                        break;
                    }

                    Card dbCard = new Card()
                    {
                        Number = card.Number,
                        Cvc = card.Cvc
                    };

                    dbCard.Type = card.Type == "Debit" ? CardType.Debit : CardType.Credit;
                    dbUser.Cards.Add(dbCard);
                }

                if (hasInvalidCard)
                {
                    sb.AppendLine("Invalid Date");
                    continue;
                }

                usersList.Add(dbUser);
                sb.AppendLine($"Imported {user.Username} with {user.Cards.Length} cards");
            }

            context.Users.AddRange(usersList);
            context.SaveChanges();


            return sb.ToString();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Purchases");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPurchaseDto[]), xmlRoot);

            using StringReader sR = new StringReader(xmlString);

            ImportPurchaseDto[] dtos = (ImportPurchaseDto[])xmlSerializer.Deserialize(sR);

            ICollection<Purchase> purchases = new HashSet<Purchase>();

            var games = context.Games.ToList();
            var cards = context.Cards
                .AsQueryable()
                .Include(c => c.User)
                .ToList();

            foreach (ImportPurchaseDto purchaseDto in dtos)
            {
                if (!IsValid(purchaseDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Purchase dbPurchase = new Purchase()
                {
                    ProductKey = purchaseDto.Key
                };

                if (!DateTime
                    .TryParseExact(purchaseDto.Date,
                    "dd/MM/yyyy HH:mm",
                    CultureInfo.InvariantCulture, 
                    DateTimeStyles.None,
                    out DateTime purchaseDate))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                dbPurchase.Date = purchaseDate;

                if (!Enum.TryParse(typeof(PurchaseType), 
                    purchaseDto.Type, 
                    out object purchaseType))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                dbPurchase.Type = (PurchaseType)purchaseType;

                var card = cards
                   .FirstOrDefault(c => c.Number == purchaseDto.Card);

                if (card == null)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                dbPurchase.Card = card;

                var game = games
                    .FirstOrDefault(g => g.Name == purchaseDto.Title);

                if (game == null)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                dbPurchase.Game = game;

                purchases.Add(dbPurchase);

                sb.AppendLine($"Imported {purchaseDto.Title} for {card.User.Username}");
            }

            context.Purchases.AddRange(purchases);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }


    
}
