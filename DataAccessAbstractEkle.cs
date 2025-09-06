namespace Generator
{
    internal static class DataAccessAbstractHelper
    {

        internal static void DataAccessAbstractEkle(string dataAccessKlasoru, string item, out string yeniDosyaYolu, out string metin)
        {
            #region DataAccessAbstract
            var dataAccessAbstract = dataAccessKlasoru + "\\Abstract";
            Directory.CreateDirectory(dataAccessAbstract);
            yeniDosyaYolu = Path.Combine(dataAccessAbstract, "I" + item + "DAL.cs");
            metin = $"using Core.DataAccess;\r\nusing Entities.Models;\r\n\r\nnamespace DataAccess.Abstract\n\r" + "{" + "\r\n    public interface I" + item + "DAL : IEntityRepositoryAsync<" + item + ">\r\n    {\r\n    }\r\n}";
            File.WriteAllText(yeniDosyaYolu, metin);
            #endregion
            Console.WriteLine("I" + item + "DAL oluşturuldu.");
        }
    }
}
