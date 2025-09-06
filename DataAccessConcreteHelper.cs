namespace Generator
{
    internal static class DataAccessConcreteHelper
    {

        internal static void DataAccessConcreteEkle(string dataAccessKlasoru, string item, out string yeniDosyaYolu, out string metin)
        {
            #region DataAccessConcrete
            var dataAccessConcrete = dataAccessKlasoru + "\\Concrete\\EntityFramework";
            Directory.CreateDirectory(dataAccessConcrete);
            yeniDosyaYolu = Path.Combine(dataAccessConcrete, "EF" + item + "DAL.cs");
            metin = $"using CorexPack.DataAccess.EntityFramework;\r\nusing Entities.Models;\r\nusing DataAccess.Abstract;\r\nusing Entities.Models;\r\n\r\nnamespace DataAccess.Concrete" + "{" + "\r\n    public class EF" + item + "DAL : EFEntityRepositoryBaseAsync<" + item + ", LoosipDbContext>, I" + item + "DAL\r\n    {\r\n    }\r\n}\r\n";
            File.WriteAllText(yeniDosyaYolu, metin);
            #endregion
            Console.WriteLine(item + "DAL oluşturuldu.");
        }
    }
}
