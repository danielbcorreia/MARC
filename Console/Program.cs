using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MARC;

namespace Marc.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = "C:\\Users\\dbmc\\Desktop\\isoteste.iso";

            using (FileMARCReader reader = new FileMARCReader(filename))
            {
                foreach (Record marc in reader)
                {
                    IEnumerable<DataField> covers = marc.Fields.Where(f => f.Tag == "200").Cast<DataField>();

                    foreach (var cover in covers)
                    {
                        string title = cover.GetSubfieldOrEmpty('a');
                        string subtitle = cover.GetSubfieldOrEmpty('e');
                        string author = cover.GetSubfieldOrEmpty('f');
                        string otherAuthors = cover.GetSubfieldOrEmpty('g');

                        Console.WriteLine("Título: {0}", title);
                        Console.WriteLine("Subtítulo: {0}", subtitle);
                        Console.WriteLine("Autor: {0}", author);
                        Console.WriteLine("Outros Autores: {0}", otherAuthors);
                    }

                    Console.WriteLine();
                    //Console.ReadKey(true);
                }
            }
        }
    }

    public static class MarcExtensions
    {
        public static string GetSubfieldOrEmpty(this DataField field, char c)
        {
            var subfield = field.Subfields.FirstOrDefault(f => f.Code == c);
            return subfield != null ? subfield.Data : string.Empty;
        }
    }
}
