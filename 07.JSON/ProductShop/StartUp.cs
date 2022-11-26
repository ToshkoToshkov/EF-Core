using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs;
using ProductShop.DTOs.Input;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProductShop
{
    public class StartUp
    {
        private static IMapper mapper;

        public static void Main(string[] args)
        {
            ProductShopContext context = new ProductShopContext();

            string usersjsonassstring = File.ReadAllText(@"c:\users\toshk\desktop\задачи c#\entityframework\json-exercise\productshop\datasets\users.json");
            string productsJsonAssString = File.ReadAllText(@"c:\users\toshk\desktop\задачи c#\entityframework\json-exercise\productshop\datasets\products.json");
            string categoriesJsonAssString = File.ReadAllText(@"c:\users\toshk\desktop\задачи c#\entityframework\json-exercise\productshop\datasets\categories.json");
            string categoryProductJsonAssString = File.ReadAllText(@"c:\users\toshk\desktop\задачи c#\entityframework\json-exercise\productshop\datasets\categories-products.json");

            //Console.WriteLine(ImportUsers(context, usersjsonassstring));
            //Console.WriteLine(ImportProducts(context, productsJsonAssString));
            //Console.WriteLine(ImportCategories(context, categoriesJsonAssString));
            //Console.WriteLine(ImportCategoryProducts(context, categoryProductJsonAssString));

            var result = GetSoldProducts(context);
            Console.WriteLine(result);

        }

        //Import users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
                             //WHITHOUT DTO'S

            //IEnumerable<User> users = JsonConvert.DeserializeObject<IEnumerable<User>>(inputJson);

            //context.Users.AddRange(users);
            //context.SaveChanges();



                               //WHIT DTO'S 

            IEnumerable<UserInputDto> users = JsonConvert.DeserializeObject<IEnumerable<UserInputDto>>(inputJson);


            //WHIT AUTOMAPPING

            InitializeMapper();
            var mappedUsers = mapper.Map<IEnumerable<User>>(users);



                         //WHIT STATIC MAPPING USING THE MAPPER-CLASS

            //IEnumerable<User> mappedUsers = users
            //    .Select(x => x.MapToDomainUser())
            //    .ToList();

            context.Users.AddRange(mappedUsers);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }





        //Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
                                      //WHITHOUT DTO'S

            //IEnumerable<Product> products = JsonConvert.DeserializeObject<IEnumerable<Product>>(inputJson);
            //context.Products.AddRange(products);
            //context.SaveChanges();

                                      //WHIT DTO'S

            IEnumerable<ProductsInputDto> products = JsonConvert.DeserializeObject<IEnumerable<ProductsInputDto>>(inputJson);

                                       //WHIT AUTOMAPPER

            InitializeMapper();
            var mappedProducts = mapper.Map<IEnumerable<Product>>(products);

                                       //WHIT STATIC MAPPING USING THE MAPPER-CLASS

           // IEnumerable<Product> mappedProducts = products
           //   .Select(x => x.MapToDomainProduct())
           //   .ToList();

            context.Products.AddRange(mappedProducts);
            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }




        //Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
                                  //WHITHOUT DTO'S
            //IEnumerable<Category> categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(inputJson).Where(x => !string.IsNullOrEmpty(x.Name)); ;
            //context.Categories.AddRange(categories);
            //context.SaveChanges();


            //WHIT DTO'S
            IEnumerable<CategoryInputDto> categories = JsonConvert.DeserializeObject<IEnumerable<CategoryInputDto>>(inputJson)
                .Where(x => !string.IsNullOrEmpty(x.Name));

            //with automapper
            InitializeMapper();
            var mappedCategories = mapper.Map<IEnumerable<Category>>(categories);


            //WITH STATIC MAPPING USING THE MAPPING CLASS
            //IEnumerable<Category> mappedCategories = categories
            //    .Select(x => x.MapToDomainCategory())
            //    .ToList();

            context.Categories.AddRange(mappedCategories);
            context.SaveChanges();

            return $"Successfully imported {mappedCategories.Count()}";
        }




        //Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            //WHITHOUT DTO
            //IEnumerable<CategoryProduct> categoryProduct = JsonConvert.DeserializeObject<IEnumerable<CategoryProduct>>(inputJson);
            //context.CategoryProducts.AddRange(categoryProduct);
            //context.SaveChanges();

            //WHIT DTO
            IEnumerable<CategoryProductInputDto> categoryProduct = JsonConvert.DeserializeObject<IEnumerable<CategoryProductInputDto>>(inputJson);

            //WHIT AUTOMAPPER
            InitializeMapper();
            var mappedCategoryProducts = mapper.Map<IEnumerable<CategoryProduct>>(categoryProduct);

            //WHIT STATIC MAPPING USING THE MAPPING-CLASS
            //IEnumerable<CategoryProduct> mappedCategoryProduct = categoryProduct
            //    .Select(x => x.MapToDomainCategoryProduct())
            //    .ToList();



            context.CategoryProducts.AddRange(mappedCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {mappedCategoryProducts.Count()}";
        }



        //Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    Name = p.Name,
                    Price = p.Price,
                    Seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                })
                .ToList();

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver
            };

            string productsAsJson = JsonConvert.SerializeObject(products, jsonSettings);
            
            return productsAsJson;
        }



        //Export Successfully Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var query = context
                .Users
                .Include(x => x.ProductsSold)
                .Where(x => x.ProductsSold.Any(y => y.Buyer != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold.Select(y => new
                    {
                        Name = y.Name,
                        Price = y.Price,
                        BuyerFirstName = y.Buyer.FirstName,
                        BuyerLastName = y.Buyer.LastName
                    })
                    .ToList()

                })
                .ToList();

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver
            };

            string result = JsonConvert.SerializeObject(query, jsonSettings);

            return result;
        }



        //Export Categories by products count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var query = context
                .Categories
                .OrderByDescending(c => c.CategoryProducts.Count)
                .Select(x => new
                {
                    Category = x.Name,
                    ProductsCount = x.CategoryProducts.Count,
                    AveragePrice = $"{x.CategoryProducts.Average(cp =>                        cp.Product.Price):f2}",
                    TotalRevenue = $"{x.CategoryProducts.Sum(cp => cp.Product.Price):f2}"
                })
                .ToList();

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver
            };

            string result = JsonConvert.SerializeObject(query, jsonSettings);

            return result;
        }


        //Initialize Mapper method
        private static void InitializeMapper()
        {
            var mapperconfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            mapper = new Mapper(mapperconfiguration);
        }
    }

    //Class for static mapping for users
    public static class UserMappings
    {
        public static User MapToDomainUser(this UserInputDto userDto)
        {
            return new User
            {
                Age = userDto.Age,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName
            };
        }

        public static Product MapToDomainProduct(this ProductsInputDto productDto)
        {
            return new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                SellerId = productDto.SellerId,
                BuyerId = productDto.BuyerId
            };
        }

        public static Category MapToDomainCategory(this CategoryInputDto categoryDto)
        {
            return new Category
            {
                Name = categoryDto.Name
            };
        }

        public static CategoryProduct MapToDomainCategoryProduct(this CategoryProductInputDto dto)
        {
            return new CategoryProduct
            {
                CategoryId = dto.CategoryId,
                ProductId = dto.ProductId
            };
        }

    }
}