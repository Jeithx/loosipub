namespace Generator
{
    internal class ApiGenerator
    {
        internal static void ApiEkle(string apiKlasoru, string item, out string yeniDosyaYolu, out string metin)
        {
            #region API
            var api = apiKlasoru + "/Controllers/v1";
            //Klasör oluşturulacak
            var itemKlasoru = Path.Combine(api, item + "Controller");
            Directory.CreateDirectory(api);
            Directory.CreateDirectory(itemKlasoru);
            yeniDosyaYolu = Path.Combine(itemKlasoru, "_" + item + "Controller.cs");

            metin = $"using Microsoft.AspNetCore.Mvc;" +
                "\r\nusing Business.Abstract;" +
                "\r\n\r\nnamespace Api.Controllers.v1." + item + "Controller;" +
                "\r\n\r\n[Route(\"v1/api/[controller]\")]" +
                "\r\n[ApiController]" +
                "\r\npublic partial class " + item + "Controller : ControllerBase" +
                "\r\n" +
                "{" +
                "\r\n    private readonly I" + item + "Service _service;" +
                "\r\n    public " + item + "Controller(I" + item + "Service service)" +
                "\r\n    " +
                "{" +
                "\r\n        _service = service;" +
                "}" +
                "\r\n" +
                "}" +
                "\r\n";

            File.WriteAllText(yeniDosyaYolu, metin);
            #endregion

            CreateFuncAdd(itemKlasoru, item);
            DeleteFunc(itemKlasoru, item);
            GetFunc(itemKlasoru, item);
            UpdateFunc(itemKlasoru, item);
            GetByIdFunc(itemKlasoru, item);
            Console.WriteLine(item + "Controller oluşturuldu.");
        }

        internal static void CreateFuncAdd(string itemKlasoru, string item)
        {
            #region API
            //Klasör oluşturulacak
            var yeniDosyaYolu = Path.Combine(itemKlasoru, "Create.cs");

            string metin = $"using Microsoft.AspNetCore.Mvc;" +
                "\r\nusing Core.Models.Write;" +
                "\r\nusing Core.Models;" +
                "\r\nusing Core.Utilities.Results;" +
                "\r\nusing Core.Models.Read;" +
                "\r\nusing Microsoft.AspNetCore.Authorization;" +
                "\r\n\r\nnamespace Api.Controllers.v1." + item + "Controller;" +
                "\r\n\r\npublic partial class " + item + "Controller" +
                "\r\n{\r\n   [Authorize(Roles = \"\")] \r\n [HttpPost]" +
                "\r\n    public async Task<IActionResult> Create(" + item + "WD model)" +
                "\r\n    " +
                "{\r\n      " +
                "       var resultModel = await _service.Create(model);" +
                "\r\n\r\n        if (resultModel.Success)" +
                "\r\n            return Ok(new SuccessDataResult<" + item + "RD>(resultModel.Data, resultModel.Message));" +
                "\r\n        else" +
                "\r\n            return BadRequest(new ErrorDataResult<" + item + "RD>(resultModel.Message));" +
                "\r\n   " +
                "}" +
                "\r\n" +
                "}" +
                "\r\n";

            File.WriteAllText(yeniDosyaYolu, metin);
            #endregion
            Console.WriteLine(item + "Create oluşturuldu.");
        }

        internal static void DeleteFunc(string itemKlasoru, string item)
        {
            #region API
            //Klasör oluşturulacak
            var yeniDosyaYolu = Path.Combine(itemKlasoru, "Delete.cs");

            string metin = $"using Microsoft.AspNetCore.Mvc;" +
                "\r\nusing Core.Constants;" +
                "\r\nusing Core.Models;" +
                "\r\nusing Core.Utilities.Results;" +
                "\r\nusing Microsoft.AspNetCore.Authorization;" +
                "\r\n\r\nnamespace Api.Controllers.v1." + item + "Controller" +
                "\r\n{\r\n    public partial class " + item + "Controller" +
                "\r\n    {\r\n      [Authorize(Roles = \"\")] \r\n  [HttpDelete]" +
                "\r\n        public async Task<IActionResult> Delete(int id)" +
                "\r\n        " +
                "{\r\n            if (id == 0)" +
                "\r\n                return BadRequest(new ErrorDataResult<bool>(false ,Messages.DeleteProcessFailed));" +
                "\r\n\r\n            bool result = await _service.Delete(id);" +
                "\r\n\r\n            if (result)" +
                "\r\n                return Ok(new SuccessDataResult<bool>(true, Messages.DeleteProcessSuccessful));" +
                "\r\n            else" +
                "\r\n                return BadRequest(new ErrorDataResult<bool>(false, Messages.DeleteProcessFailed));" +
                "\r\n       " +
                " }\r\n   " +
                " }\r\n" +
                "}\r\n";

            File.WriteAllText(yeniDosyaYolu, metin);
            #endregion
            Console.WriteLine(item + "Delete oluşturuldu.");
        }

        internal static void GetFunc(string itemKlasoru, string item)
        {
            #region API
            //Klasör oluşturulacak
            var yeniDosyaYolu = Path.Combine(itemKlasoru, "Get.cs");

            string metin = $"using Microsoft.AspNetCore.Mvc;" +
                "\r\nusing Core.Constants;" +
                "\r\nusing Core.Models;" +
                "\r\nusing Core.Models.Read;" +
                "\r\nusing Core.Utilities.Results;" +
                "\r\nusing Microsoft.AspNetCore.Authorization;" +
                "\r\n\r\nnamespace Api.Controllers.v1." + item + "Controller" +
                "\r\n{\r\n    public partial class " + item + "Controller" +
                "\r\n    {\r\n   [Authorize(Roles = \"\")] \r\n     [HttpGet]" +
                "\r\n        public async Task<IActionResult> Get(int? pageNumber = 1, int? pageSize = 10)" +
                "\r\n        {\r\n          " +
                "\r\n\r\n            var result = await _service.Get(null, null, pageNumber, pageSize);" +
                "\r\n\r\n            return Ok(new SuccessDataResult<List<" + item + "RD>>(result.Data, result.Message, result.RecordTotals));" +
                "\r\n    " +
                "    }\r\n " +
                "   }\r\n" +
                "}\r\n";

            File.WriteAllText(yeniDosyaYolu, metin);
            #endregion
            Console.WriteLine(item + "Get oluşturuldu.");
        }

        internal static void UpdateFunc(string itemKlasoru, string item)
        {
            #region API
            //Klasör oluşturulacak
            var yeniDosyaYolu = Path.Combine(itemKlasoru, "Update.cs");

            string metin = $"using Microsoft.AspNetCore.Mvc;" +
                "\r\nusing Core.Models;" +
                "\r\nusing Core.Models.Read;" +
                "\r\nusing Core.Models.Write;" +
                "\r\nusing Core.Utilities.Results;" +
                "\r\nusing Microsoft.AspNetCore.Authorization;" +
                "\r\n\r\nnamespace Api.Controllers.v1." + item + "Controller" +
                "\r\n{\r\n    public partial class " + item + "Controller" +
                "\r\n    {\r\n       [Authorize(Roles = \"\")] \r\n[HttpPut]" +
                "\r\n        public async Task<IActionResult> Update([FromBody] " + item + "WD model)" +
                "\r\n       " +
                " {\r\n           " +
                "\r\n\r\n            var result = await _service.Update(model);" +
                "\r\n\r\n            if (result.Success)" +
                "\r\n                return Ok(new SuccessDataResult<" + item + "RD>(result.Data, result.Message));" +
                "\r\n            else" +
                "\r\n                return BadRequest(new ErrorDataResult<" + item + "RD>(result.Message));" +
                "\r\n      " +
                "  }\r\n" +
                "    }\r\n" +
                "}\r\n";

            File.WriteAllText(yeniDosyaYolu, metin);
            #endregion
            Console.WriteLine(item + "Update oluşturuldu.");
        }

        internal static void GetByIdFunc(string itemKlasoru, string item)
        {
            #region API
            //Klasör oluşturulacak
            var yeniDosyaYolu = Path.Combine(itemKlasoru, "GetById.cs");

            string metin = $"using Microsoft.AspNetCore.Mvc;\r\nusing Core.Constants;\r\nusing Core.Models;\r\nusing Core.Utilities.Results;\r\nusing Microsoft.AspNetCore.Authorization;\r\nusing Core.Models.Read;\r\n\r\nnamespace Api.Controllers.v1." + item + "Controller\r\n{\r\n    public partial class " + item + "Controller\r\n    {\r\n  \r\n [Authorize(Roles = \"\")]\r\n      [HttpGet(\"GetById\")]\r\n        public async Task<IActionResult> GetById(long id)\r\n        {\r\n            if (id == 0)\r\n                return BadRequest(new ErrorDataResult<" + item + "RD>(Messages.NotFound));\r\n\r\n            var result = await _service.GetByExpression(x=>x.Id == id);\r\n\r\n            if (result == null)\r\n                return BadRequest(new ErrorDataResult<" + item + "RD>(Messages.NotFound));\r\n\r\n            return Ok(new SuccessDataResult<" + item + "RD>(result, Messages.GetProcessSuccessful));\r\n        }\r\n    }\r\n}";

            File.WriteAllText(yeniDosyaYolu, metin);
            #endregion
            Console.WriteLine(item + "GetById oluşturuldu.");
        }
    }
}
