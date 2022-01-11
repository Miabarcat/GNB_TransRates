using AutoMapper;
using GNB_TransRates.DAL.Models;
using GNB_TransRates.DL.Models;

namespace GNB_TransRates.API.Infrastucture
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Rates, RatesResponseModel>()
                .ForMember(dest => dest.From,
                            opts => opts.MapFrom(src => src.FromCurr))
                .ForMember(dest => dest.To,
                            opts => opts.MapFrom(src => src.ToCurr))
                .ForMember(dest => dest.Rate,
                            opts => opts.MapFrom(src => src.Amount));
            CreateMap<RatesResponseModel, Rates>()
                .ForMember(dest => dest.FromCurr,
                            opts => opts.MapFrom(src => src.From))
                .ForMember(dest => dest.ToCurr,
                            opts => opts.MapFrom(src => src.To))
                .ForMember(dest => dest.Amount,
                            opts => opts.MapFrom(src => src.Rate));

            CreateMap<Transactions, TransactionsResponseModel>();
            CreateMap<TransactionsResponseModel, Transactions>();
        }
    }
}
