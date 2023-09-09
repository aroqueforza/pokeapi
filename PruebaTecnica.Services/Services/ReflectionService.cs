using PruebaTecnica.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnica.Services.Services
{
    public static class ReflectionService<model> where model : class
    {
        public static DataTable GetObject(List<string> properties,List<model?> objects)
        {
            DataTable dt = new DataTable();
            var props = typeof(model).GetProperties().Where(o => properties.Select(o=>o.ToLower()).Contains(o.Name.ToLower()));
            foreach (var prop in props)
            {
                Console.WriteLine($"{prop.Name}");
                dt.Columns.Add(prop.Name, prop.PropertyType);
            }
            foreach (var r in objects)
            {
                DataRow dr = dt.NewRow();
                foreach (var prop in props)
                {
                    dr[prop.Name] = prop.GetValue(r);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
