namespace AwesomeBank.API.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountViewModel>()
                .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.Transactions));

            CreateMap<Transaction, TransactionViewModel>();
            CreateMap<InterestRule, InterestRuleViewModel>();
        }
    }
}