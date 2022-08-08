using System;
using System.IO;
using System.Text.Json;
using SeevoConfig.Errors;

namespace SeevoConfig.Projects
{
    public static class ProjectService
    {
        public const string DefaultDescription = "Project Seevo";

        public static Project New()
        {
            return new Project
            {
                Created = DateTime.Now,
                Description = DefaultDescription
            };
        }

        public static Project Load(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path)) { throw new ArgumentException("Invalid project file path."); }
                if (!File.Exists(path)) { throw new FileNotFoundException("Project file not found."); }

                var json = File.ReadAllText(path);
                var model = JsonSerializer.Deserialize<Project>(json, GetOptions());
                if (model == null) { throw new Exception("Invalid project file."); }
                model.FilePath = path;
                return model;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

        public static void Save(string path, Project model)
        {
            try
            {
                if (model == null) { throw new ArgumentException("Invalid project model."); }
                if (string.IsNullOrWhiteSpace(path)) { throw new ArgumentException("Invalid project file path."); }

                var json = JsonSerializer.Serialize(model, GetOptions());
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        private static JsonSerializerOptions GetOptions()
        {
            var jso = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            //jso.Converters.Add(new Converter());

            return jso;
        }
    }
}
