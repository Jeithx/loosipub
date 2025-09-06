using ClosedXML.Excel;

namespace Hafiz.Core.Utilities.Excel
{
    public class CreateExcel
    {
        public byte[] CreatorExcelFile(List<Dictionary<string, string>> contents)
        {
            byte[] excelFile;
            using (var workbook = new XLWorkbook())
            {
                string[] headerTitle = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V" };
                var workSheet = workbook.Worksheets.Add("Sayfa 1");

                int x = 1;
                foreach (var item in contents.FirstOrDefault())
                {
                    workSheet.Cell(headerTitle[x - 1] + 1).Value = item.Key;
                    x++;
                }

                int i = 2, y = 1;
                foreach (var item in contents)
                {
                    foreach (var property in item)
                    {
                        workSheet.Cell(i, y).SetValue(property.Value);
                        y++;
                    }
                    workSheet.RecalculateAllFormulas();
                    i++; y = 1;
                };

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    excelFile = memoryStream.ToArray();
                }
            }
            return excelFile;

            //Test Edebileceğiniz ve Data gönderirken izlemeniz gereken yol için aşağıya örnek bırakıyorum.
            //var tests = new List<Dictionary<string, string>>();
            //for (int i = 1; i < 11; i++)
            //{
            //    var test = new Dictionary<string, string>();
            //    test.Add("Ad Soyad", i + ".   Ali ");
            //    test.Add("Telefon", i + ". 539123102");
            //    test.Add("Email", i + ". asdsa@hotmail.com");
            //    tests.Add(test);
            //}

            //Mail Gönderirken Attachment için Örnek Bırakıyorum. 
            //new Attachment(new MemoryStream(excelFile), "ornekIsim" + ".xlsx")
        }
    }
}
