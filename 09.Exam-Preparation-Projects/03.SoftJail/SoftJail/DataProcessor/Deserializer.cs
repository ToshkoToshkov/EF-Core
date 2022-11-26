namespace SoftJail.DataProcessor
{

    using Data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using SoftJail.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using System.Linq;
    using System.Globalization;
    using System.Xml.Serialization;
    using System.IO;
    using SoftJail.Data.Models.Enums;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportDepartmentDto[] departments = JsonConvert
                .DeserializeObject<ImportDepartmentDto[]>(jsonString);

            List<Department> dbDepartments = new List<Department>();

            foreach (ImportDepartmentDto department in departments)
            {
                if (!IsValid(department))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Department d = new Department()
                {
                    Name = department.Name
                };

                bool isDepValid = true;

                foreach (ImportDepartmenCellsDto cellsDto in department.Cells)
                {
                    if (!IsValid(cellsDto))
                    {
                        isDepValid = false;
                        break;
                    }

                    d.Cells.Add(new Cell()
                    {
                        CellNumber = cellsDto.CellNumber,
                        HasWindow = cellsDto.HasWindow
                    });
                }

                if (!isDepValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                if (d.Cells.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }


                dbDepartments.Add(d);

                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count()} cells");
            }

            context.Departments.AddRange(dbDepartments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportPrisonerDto[] prisonerDtos = JsonConvert.DeserializeObject<ImportPrisonerDto[]>(jsonString);

            List<Prisoner> prisoners = new List<Prisoner>();

            foreach (ImportPrisonerDto prisonerDto in prisonerDtos)
            {
                if (!IsValid(prisonerDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime incarcerationDate;
                bool isIncarcerationDateValid = DateTime.TryParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out incarcerationDate);

                if (!isIncarcerationDateValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime? releaseDate = null;
                if (!String.IsNullOrEmpty(prisonerDto.ReleaseDate))
                {
                    DateTime releaseDateValue;
                    bool isReleaseDateValid = DateTime.TryParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDateValue);

                    if (!isReleaseDateValid)
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    releaseDate = releaseDateValue;
                }

                Prisoner p = new Prisoner()
                {
                    FullName = prisonerDto.FullName,
                    Nickname = prisonerDto.Nickname,
                    Age = prisonerDto.Age,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = releaseDate,
                    Bail = prisonerDto.Bail,
                    CellId = prisonerDto.CellId
                };

                bool areMailsValid = true;
                foreach (ImportPrisonerMailsDto mailDto in prisonerDto.Mails)
                {
                    if (!IsValid(mailDto))
                    {
                        areMailsValid = false;
                        continue;
                    }

                    p.Mails.Add(new Mail()
                    {
                        Description = mailDto.Description,
                        Sender = mailDto.Sender,
                        Address = mailDto.Address
                    });
                }

                if (!areMailsValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                prisoners.Add(p);
                sb.AppendLine($"Imported {p.FullName} {p.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Officers");
            XmlSerializer serializer = new XmlSerializer(typeof(ImportOfficerDto[]), root);

            using StringReader sr = new StringReader(xmlString);

            ImportOfficerDto[] dtos = (ImportOfficerDto[])serializer.Deserialize(sr);

            ICollection<Officer> officers = new HashSet<Officer>();

            foreach (ImportOfficerDto officerDto in dtos)
            {
                if (!IsValid(officerDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                object positionObj;
                bool isPositionValid = Enum.TryParse(typeof(Position), officerDto.Position, out positionObj);

                if (!isPositionValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                object weaponObj;
                bool isWeaponValid = Enum.TryParse(typeof(Weapon), officerDto.Weapon, out weaponObj);

                if (!isWeaponValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Officer of = new Officer()
                {
                    FullName = officerDto.Name,
                    Salary = decimal.Parse(officerDto.Money),
                    Position = (Position)positionObj,
                    Weapon = (Weapon)weaponObj,
                    DepartmentId = int.Parse(officerDto.DepartmentId)
                };


                foreach (ImportOfficerPrisonerDto prisonerDto in officerDto.Prisoners)
                {
                    of.OfficerPrisoners.Add(new OfficerPrisoner()
                    {
                        Officer = of,
                        PrisonerId = int.Parse(prisonerDto.Id)
                    });
                }

                officers.Add(of);
                sb.AppendLine($"Imported {of.FullName} ({of.OfficerPrisoners.Count} prisoners)");
            }

            context.Officers.AddRange(officers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}