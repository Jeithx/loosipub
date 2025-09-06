namespace Generator
{
    internal class ServiceGenerator
    {
        internal static void ServiceCreate(string applicationKlasoru, string item, out string yeniDosyaYolu, out string metin)
        {
            #region BusinessAbstract
            var applicationInterfaces = applicationKlasoru + "\\Concrete";
            Directory.CreateDirectory(applicationInterfaces);
            yeniDosyaYolu = Path.Combine(applicationInterfaces, item + "Service.cs");
            var itemToLower = item.ToLower();

            metin = $"using AutoMapper;"
            + "\r\nusing Core.Constants;"
            + "\r\nusing Core.Helper;"
            + "\r\nusing Core.Models;"
            + "\r\nusing Core.Models.Read;"
            + "\r\nusing Core.Models.Write;"
            + "\r\nusing Core.Utilities.Results;"
            + "\r\nusing System.Linq.Expressions;"
            + "\r\nusing Business.Abstract;"
            + "\r\nusing Entities.Models;"
            + "\r\nusing DataAccess.Abstract;"
            + "\r\nusing System.Linq.Expressions;" +
            "\r\nnamespace Business.Concrete;" +
            "\r\npublic class " + item + "Service : I" + item + "Service\r\n{\r\n    private readonly IMapper _mapper; \r\nreadonly ILogger _logger;\r\n    I" + item + "DAL _" + itemToLower + "Dal;\r\n\r\n    public " + item + "Service(I" + item + "DAL " + itemToLower + "Dal, IMapper mapper, ILogger logger)\r\n    {\r\n        _" + itemToLower + "Dal = " + itemToLower + "Dal;\r\n        _mapper = mapper;\r\n _logger = logger; \r\n    }\r\n\r\n    public async Task<IDataResult<" + item + "RD>> Create(" + item + "WD " + itemToLower + "WD)\r\n    {\r\n        try\r\n        {\r\n            var data = await _" + itemToLower + "Dal.AddAsync(_mapper.Map<" + item + ">(" + itemToLower + "WD));\r\n            return new SuccessDataResult<" + item + "RD>(_mapper.Map<" + item + "RD>(data), Messages.AddingProcessSuccessful);\r\n        }\r\n        catch (Exception ex)\r\n        {\r\n             _logger.LogException(ex, nameof(Create), new { data = " + itemToLower + "WD });\r\n            return new ErrorDataResult<" + item + "RD>(Messages.AddingProcessFailed);\r\n        }\r\n    }\r\n\r\n    public async Task<bool> Delete(long id)\r\n    {\r\n        try\r\n        {\r\n            " + item + " " + itemToLower + " = await _" + itemToLower + "Dal.GetAsync(x => x.Id == id);\r\n            " +
            //+ itemToLower + ".IsActive = false;\r\n            "
            //+ itemToLower + ".UpdatedDate = DateTime.Now;" +
            "\r\n            await _" + itemToLower + "Dal.UpdateAsync(" + itemToLower + ");\r\n\r\n            return true;\r\n        }\r\n        catch (Exception ex)\r\n        {\r\n             _logger.LogException(ex, nameof(Delete), new { data = id });\r\n            throw;\r\n        }\r\n    }\r\n\r\n    public async Task<IDataResult<List<" + item + "RD>>> Get(Expression<Func<Entities.Models." + item + ", bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)\r\n    {\r\n        try\r\n        {\r\n " +
            "var " + itemToLower + "s = await _" + itemToLower + "Dal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);" +
            "\r\n     return new SuccessDataResult<List<" + item + "RD>>(_mapper.Map<List<" + item + "RD>>(" + itemToLower + "s), Messages.ProcessSuccess, await _" + itemToLower + "Dal.CountAsync(filter));        \r\n        }\r\n        catch (Exception ex)\r\n        {\r\n             _logger.LogException(ex, nameof(Get), new { data = filter });\r\n            throw new Exception(Messages.GetProcessFailed, ex);\r\n        }\r\n    }\r\n\r\n    public async Task<" + item + "RD?> GetByExpression(Expression<Func<Entities.Models." + item + ", bool>> filter = null, string[] includes = null)\r\n    {\r\n        try\r\n        {\r\n            var result = await _" + itemToLower + "Dal.GetAsync(filter, includes);\r\n            if (result != null)\r\n                return _mapper.Map<" + item + "RD>(result);\r\n\r\n            return null;\r\n        }\r\n        catch (Exception ex)\r\n        {\r\n             _logger.LogException(ex, nameof(GetByExpression), new { data = filter });\r\n            throw new Exception(Messages.ProcessFailed, ex);\r\n        }\r\n    }\r\n\r\n    public async Task<IDataResult<" + item + "RD>> Update(" + item + "WD " + itemToLower + "WD)\r\n    {\r\n        try\r\n        {\r\n            await _" + itemToLower + "Dal.UpdateAsync(_mapper.Map<" + item + ">(" + itemToLower + "WD));\r\n            return new SuccessDataResult<" + item + "RD>(_mapper.Map<" + item + "RD>(" + itemToLower + "WD), Messages.UpdateProcessSuccessful);\r\n        }\r\n        catch (Exception ex)\r\n        {\r\n             _logger.LogException(ex, nameof(Update), new { data = " + itemToLower + "WD });\r\n            return new ErrorDataResult<" + item + "RD>(Messages.UpdateProcessFailed);\r\n        }\r\n    }\r\n\r\n    public async Task<long> Count(Expression<Func<" + item + ", bool>>? filter = null, string[]? includes = null)\r\n    {\r\n        try\r\n        {\r\n            return await _" + itemToLower + "Dal.CountAsync(filter, includes);\r\n        }\r\n        catch (Exception ex)\r\n        {\r\n            _logger.LogException(ex, nameof(Count), new { data = \"\" });\r\n            throw;\r\n        }\r\n    }\r\n}";

            File.WriteAllText(yeniDosyaYolu, metin);
            #endregion
            Console.WriteLine(item + "Service oluşturuldu.");
        }
    }
}
