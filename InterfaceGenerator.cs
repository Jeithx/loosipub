namespace Generator
{
    internal class InterfaceGenerator
    {
        internal static void InterfaceCreate(string applicationKlasoru, string item, out string yeniDosyaYolu, out string metin)
        {
            #region BusinessAbstract
            var applicationInterfaces = applicationKlasoru + "\\Abstract";
            Directory.CreateDirectory(applicationInterfaces);
            yeniDosyaYolu = Path.Combine(applicationInterfaces, "I" + item + "Service.cs");

            metin = $"using Core.Models;\r\nusing Core.Models.Read;\r\nusing Core.Models.Write;\r\nusing Core.Utilities.Results;\r\nusing System.Linq.Expressions;\r\nusing Entities.Models;\r\n\r\nnamespace Business.Abstract;\r\n\r\npublic interface I" + item + "Service\r\n{\r\n    Task<IDataResult<" + item + "RD>> Create(" + item + "WD model);\r\n    Task<IDataResult<" + item + "RD>> Update(" + item + "WD model);\r\n    Task<bool> Delete(long id);\r\n    Task<IDataResult<List<" + item + "RD>>> Get(Expression<Func<Entities.Models." + item + ", bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10);\r\n Task<" + item + "RD> GetByExpression(Expression<Func<Entities.Models." + item + ", bool>> filter = null, string[]? includes = null);\r\n    Task<long> Count(Expression<Func<Entities.Models." + item + ", bool>>? filter = null, string[]? includes = null);\r\n}";
            File.WriteAllText(yeniDosyaYolu, metin);
            #endregion
            Console.WriteLine("I" + item + "Service oluşturuldu.");
        }
    }
}
