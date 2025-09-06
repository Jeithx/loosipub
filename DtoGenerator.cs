namespace Generator
{
    internal class DtoGenerator()
    {
        internal static void DtoEkle(string domainKlasoru, string item, out string yeniDosyaYolu, out string metin)
        {
            var modelKlasoru = Path.Combine(domainKlasoru, "Models");
            var modelRD = Path.Combine(modelKlasoru, "Read");
            var modelWD = Path.Combine(modelKlasoru, "Write");

            string anadizin = Directory.GetCurrentDirectory();
            for (int i = 0; i < 4; i++)
            {
                anadizin = Directory.GetParent(anadizin).FullName;
            }

            var modelFilePath = Path.Combine(anadizin + "\\Entities\\Models", $"{item}.cs");

            if (!File.Exists(modelFilePath))
            {
                throw new FileNotFoundException($"Sınıf dosyası bulunamadı: {modelFilePath}");
            }

            // Dosya içeriğini okuyoruz
            var fileContent = File.ReadAllText(modelFilePath);

            // Sınıf adını ve özelliklerini çıkarıyoruz
            var className = Helpers.ExtractClassName(fileContent);
            var properties = Helpers.ExtractProperties(fileContent);

            #region RDCreate
            Directory.CreateDirectory(modelRD);
            yeniDosyaYolu = Path.Combine(modelRD, className + "RD.cs");

            // RD sınıfı için içerik oluşturuluyor
            metin = Helpers.GenerateContent(className, properties, "RD", "_BaseRD", "Core.Models.Read");
            File.WriteAllText(yeniDosyaYolu, metin);
            #endregion

            #region WDCreate
            Directory.CreateDirectory(modelWD);
            yeniDosyaYolu = Path.Combine(modelWD, className + "WD.cs");

            // WD sınıfı için içerik oluşturuluyor
            metin = Helpers.GenerateContent(className, properties, "WD", "_BaseWD", "Core.Models.Write");
            File.WriteAllText(yeniDosyaYolu, metin);
            #endregion

            Console.WriteLine($"{className}RD ve {className}WD oluşturuldu.");
        }
    }
}
