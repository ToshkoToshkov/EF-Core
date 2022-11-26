namespace CarDealer
{
    using CarDealer.Data;
    using CarDealer.DTO.ExportDto;
    using CarDealer.DTO.ImportDto;
    using CarDealer.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext context = new CarDealerContext();

            ResetDb(context);

            //SUPPLIERS
            string inputSuppliersXml = File.ReadAllText("Datasets/suppliers.xml");
            string result = ImportSuppliers(context, inputSuppliersXml);

            //PARTS
            string inputPartrsXml = File.ReadAllText("Datasets/parts.xml");
            string result = ImportParts(context, inputPartrsXml);

            //Cars
            string inputCarXml = File.ReadAllText("Datasets/cars.xml");
            string result = ImportCars(context, inputCarXml);

            //Customers
            string inputCustomerXml = File.ReadAllText("Datasets/customers.xml");
            string result = ImportCustomers(context, inputCustomerXml);

            //Sales
            string inputSaleXml = File.ReadAllText("Datasets/sales.xml");
            string result = ImportSales(context, inputSaleXml);

            var result = GetCarsFromMakeBmw(context);
            
            Console.WriteLine(result);
        }

        //Reset Database
        private static void ResetDb(CarDealerContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Console.WriteLine("Reset");
        }

        //Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Suppliers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);

            ImportSupplierDto[] dtos = (ImportSupplierDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Supplier> suppliers = new HashSet<Supplier>();
            foreach (ImportSupplierDto supplierDto in dtos)
            {
                Supplier s = new Supplier()
                {
                    Name = supplierDto.Name,
                    IsImporter = bool.Parse(supplierDto.IsImporter)
                };

                suppliers.Add(s);
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";

        }


        //Import Parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Parts");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPartDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);

            ImportPartDto[] dtos = (ImportPartDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Part> parts = new HashSet<Part>();
            foreach (ImportPartDto partDto in dtos)
            {
                Supplier supplier = context
                    .Suppliers
                    .Find(partDto.SupplierId);

                if (supplier == null)
                {
                    continue;
                }

                Part p = new Part()
                {
                    Name = partDto.Name,
                    Price = decimal.Parse(partDto.Price),
                    Quantity = int.Parse(partDto.Quantity),
                    Supplier = supplier
                };

                parts.Add(p);
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }


        //Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Cars");
            XmlSerializer xmlSerializer = 
                new XmlSerializer(typeof(ImportCarDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);

            ImportCarDto[] dtos = (ImportCarDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Car> cars = new HashSet<Car>();
            
            foreach (ImportCarDto carDto in dtos)
            {
                Car c = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TraveledDistance
                };

                ICollection<PartCar> currentCarParts = new HashSet<PartCar>();

                foreach (int partId in carDto.Parts.Select(p => p.Id).Distinct())
                {
                    Part p = context
                        .Parts
                        .Find(partId);

                    if (p == null)
                    {
                        continue;
                    }

                    PartCar partCar = new PartCar()
                    {
                        Car = c,
                        Part = p
                    };
                    currentCarParts.Add(partCar);
                }

                c.PartCars = currentCarParts;
                cars.Add(c);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";

        }

        //Import Custumers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Customers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCustumerDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);

            ImportCustumerDto[] dtos = (ImportCustumerDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Customer> customers = new HashSet<Customer>();

            foreach (ImportCustumerDto customerDto in dtos)
            {
                Customer cus = new Customer()
                {
                    Name = customerDto.Name,
                    BirthDate = customerDto.BirthDate,
                    IsYoungDriver = bool.Parse(customerDto.IsYoungDriver)
                };

                customers.Add(cus);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        //Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Sales");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSalesDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);

            ImportSalesDto[] dtos = (ImportSalesDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Sale> sales = new HashSet<Sale>();

            foreach (ImportSalesDto salesDto in dtos)
            {
                Car car = context
                    .Cars
                    .Find(salesDto.CarId);

                if (car == null)
                {
                    continue;
                }

                Sale s = new Sale()
                {
                    CarId = salesDto.CarId,
                    CustomerId = salesDto.CustomerId,
                    Discount = salesDto.Discount
                };

                sales.Add(s);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }


        //EXPORT DATA


        //Get Car With Distance
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(sb);

            XmlRootAttribute xmlRoot = new XmlRootAttribute("cars");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarsWithDistanceDto[]), xmlRoot);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            ExportCarsWithDistanceDto[] carsDtos = context
                .Cars
                .Where(c => c.TravelledDistance > 2000000)
                .OrderBy(C => C.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(c => new ExportCarsWithDistanceDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TravelledDistance.ToString()
                })
                .ToArray();

            xmlSerializer.Serialize(stringWriter, carsDtos, namespaces);

            return sb.ToString().TrimEnd();
        }


        //Cars From Make BMW
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter sW = new StringWriter(sb);

            XmlRootAttribute xmlRoot = new XmlRootAttribute("cars");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportCarsWhitMakeBMWdto[]), xmlRoot);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            ExportCarsWhitMakeBMWdto[] dtos = context
                .Cars
                .Where(c => c.Model == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new ExportCarsWhitMakeBMWdto()
                {
                    Id = c.Id.ToString(),
                    Model = c.Model,
                    TraveledDistance = c.TravelledDistance.ToString()
                })
                .ToArray();

            xmlSerializer.Serialize(sW, dtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Sales With Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(sb);

            XmlRootAttribute xmlRoot = new XmlRootAttribute("sales");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportSalesWithDiscountDto[]), xmlRoot);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            ExportSalesWithDiscountDto[] dtos = context
                .Sales
                .Select(s => new ExportSalesWithDiscountDto()
                {
                    Car = new ExportSalesCarDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistane = s.Car.TravelledDistance.ToString()
                    },
                    Discount = s.Discount.ToString("f2"),
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price).ToString("f2"),
                    PriceWhitDiscount = (s.Car.PartCars.Sum(pc => pc.Part.Price) - s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount / 100).ToString("f2")
                })
                .ToArray();

            xmlSerializer.Serialize(stringWriter, dtos, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}