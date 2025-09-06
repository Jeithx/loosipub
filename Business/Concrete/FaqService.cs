using AutoMapper;
using Business.Abstract;
using Core.Constants;
using Core.Models.Read;
using Core.Models.Write;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Models;
using System.Linq.Expressions;
namespace Business.Concrete;
public class FaqService : IFaqService
{
    private readonly IMapper _mapper;
    readonly ILogger _logger;
    IFaqDAL _faqDal;

    public FaqService(IFaqDAL faqDal, IMapper mapper, ILogger logger)
    {
        _faqDal = faqDal;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IDataResult<FaqRD>> Create(FaqWD faqWD)
    {
        try
        {
            var data = await _faqDal.AddAsync(_mapper.Map<Faq>(faqWD));
            return new SuccessDataResult<FaqRD>(_mapper.Map<FaqRD>(data), Messages.AddingProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Create), new { data = faqWD });
            return new ErrorDataResult<FaqRD>(Messages.AddingProcessFailed);
        }
    }

    public async Task<bool> Delete(long id)
    {
        try
        {
            Faq faq = await _faqDal.GetAsync(x => x.Id == id);
            if (faq != null)
            {
                faq.IsActive = false;
                faq.UpdateDate = DateTime.Now;
                await _faqDal.UpdateAsync(faq);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Delete), new { data = id });
            throw;
        }
    }

    public async Task<IDataResult<List<FaqRD>>> Get(Expression<Func<Entities.Models.Faq, bool>> filter = null, string[]? includes = null, int? pageNumber = 1, int? pageSize = 10)
    {
        try
        {
            var faqs = await _faqDal.GetPageAsync(filter, includes, (int)pageNumber!, (int)pageSize!);
            return new SuccessDataResult<List<FaqRD>>(_mapper.Map<List<FaqRD>>(faqs), Messages.ProcessSuccess, await _faqDal.CountAsync(filter));
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Get), new { data = filter });
            throw new Exception(Messages.GetProcessFailed, ex);
        }
    }

    public async Task<FaqRD?> GetByExpression(Expression<Func<Entities.Models.Faq, bool>> filter = null, string[] includes = null)
    {
        try
        {
            var result = await _faqDal.GetAsync(filter, includes);
            if (result != null)
                return _mapper.Map<FaqRD>(result);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(GetByExpression), new { data = filter });
            throw new Exception(Messages.ProcessFailed, ex);
        }
    }

    public async Task<IDataResult<FaqRD>> Update(FaqWD faqWD)
    {
        try
        {
            await _faqDal.UpdateAsync(_mapper.Map<Faq>(faqWD));
            return new SuccessDataResult<FaqRD>(_mapper.Map<FaqRD>(faqWD), Messages.UpdateProcessSuccessful);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Update), new { data = faqWD });
            return new ErrorDataResult<FaqRD>(Messages.UpdateProcessFailed);
        }
    }

    public async Task<long> Count(Expression<Func<Faq, bool>>? filter = null, string[]? includes = null)
    {
        try
        {
            return await _faqDal.CountAsync(filter, includes);
        }
        catch (Exception ex)
        {
            _logger.LogException(ex, nameof(Count), new { data = "" });
            throw;
        }
    }
}