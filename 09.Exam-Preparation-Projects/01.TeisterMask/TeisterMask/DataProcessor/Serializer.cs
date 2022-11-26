namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Projects");
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportProjectDto), xmlRoot);

            using StringWriter sW = new StringWriter(sb);

            ExportProjectDto[] projects = context
                .Projects
                .ToArray()
                .Where(p => p.Tasks.Any())
                .Select(p => new ExportProjectDto()
                {
                    Name = p.Name,
                    TasksCount = p.Tasks.Count,
                    HasEndDate = p.DueDate.HasValue? "Yes" : "No",
                    Tasks = p.Tasks
                           .Select(t => new ExportProjectTaskDto()
                           {
                               TaskName = t.Name,
                               LabelString = t.LabelType.ToString()
                           })
                           .OrderBy(t => t.TaskName)
                           .ToArray()
                })
                .OrderByDescending(p => p.TasksCount)
                .ThenBy(p => p.Name)
                .ToArray();

            xmlSerializer.Serialize(sW, projects, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var mostBussiesEmployees = context
                .Employees
                .ToArray()
                .Where(e => e.EmployeesTasks
                             .Any(et => et.Task.OpenDate >= date))
                .Select(e => new
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                            .Where(et => et.Task.OpenDate >= date)
                            .Select(et => et.Task)
                            .OrderByDescending(t => t.DueDate)
                            .ThenBy(t => t.Name)
                            .Select(t => new
                            {
                                TaskName = t.Name,
                                OpenDate = t.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                                DueDate = t.DueDate.ToString("d",
                                           CultureInfo.InvariantCulture),
                                LabelType = t.LabelType.ToString(),
                                ExecutionType = t.ExecutionType.ToString()
                            })
                            .ToArray()
                })
                .OrderByDescending(e => e.Tasks.Length)
                .ThenBy(e => e.Username)
                .Take(10)
                .ToArray();

            string json = JsonConvert.SerializeObject(mostBussiesEmployees,
                Formatting.Indented);

            return json;
        }
    }
}