using AutoMapper;
using Task_ERP_Api.DTOs;
using Task_ERP_Api.Models;

namespace Task_ERP_Api.Mapper
{

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Account mappings
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<Account, CreateAccountDto>().ReverseMap();
            CreateMap<Account, UpdateAccountDto>().ReverseMap();
            CreateMap<CreateAccountDto, Account>()
                .ForMember(dest => dest.Branch, opt => opt.Ignore())
                .ForMember(dest => dest.JVDetails, opt => opt.Ignore())
                .ForMember(dest => dest.SubAccounts_Details, opt => opt.Ignore());

            // Branch mappings
            CreateMap<Branch, BranchDto>().ReverseMap();
            CreateMap<Branch, CreateBranchDto>().ReverseMap();
            CreateMap<Branch, UpdateBranchDto>().ReverseMap();
            CreateMap<CreateBranchDto, Branch>()
                .ForMember(dest => dest.Accounts, opt => opt.Ignore())
                .ForMember(dest => dest.Cities, opt => opt.Ignore())
                .ForMember(dest => dest.JVDetails, opt => opt.Ignore())
                .ForMember(dest => dest.JVTypes, opt => opt.Ignore())
                .ForMember(dest => dest.JVs, opt => opt.Ignore())
                .ForMember(dest => dest.SubAccounts, opt => opt.Ignore())
                .ForMember(dest => dest.SubAccountsClients, opt => opt.Ignore())
                .ForMember(dest => dest.SubAccounts_Details, opt => opt.Ignore())
                .ForMember(dest => dest.SubAccounts_Levels, opt => opt.Ignore());

            // City mappings
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<City, CreateCityDto>().ReverseMap();
            CreateMap<City, UpdateCityDto>().ReverseMap();
            CreateMap<CreateCityDto, City>()
                .ForMember(dest => dest.Branch, opt => opt.Ignore())
                .ForMember(dest => dest.SubAccountsClients, opt => opt.Ignore());

            // JV mappings
            CreateMap<JV, JVDto>().ReverseMap();
            CreateMap<JV, CreateJVDto>().ReverseMap();
            CreateMap<JV, UpdateJVDto>().ReverseMap();

            // JVDetail mappings
            CreateMap<JVDetail, JVDetailDto>().ReverseMap();
            CreateMap<CreateJVDetailDto, JVDetail>()
                .ForMember(dest => dest.JVDetailID, opt => opt.Ignore())
                .ForMember(dest => dest.JV, opt => opt.Ignore())
                .ForMember(dest => dest.Account, opt => opt.Ignore())
                .ForMember(dest => dest.SubAccount, opt => opt.Ignore())
                .ForMember(dest => dest.Branch, opt => opt.Ignore());
            CreateMap<JVDetail, CreateJVDetailDto>().ReverseMap();
            CreateMap<JVDetail, UpdateJVDetailDto>().ReverseMap();

            // JVType mappings
            CreateMap<JVType, JVTypeDto>().ReverseMap();
            CreateMap<JVType, CreateJVTypeDto>().ReverseMap();
            CreateMap<JVType, UpdateJVTypeDto>().ReverseMap();
            CreateMap<CreateJVTypeDto, JVType>()
                .ForMember(dest => dest.Branch, opt => opt.Ignore())
                .ForMember(dest => dest.JVs, opt => opt.Ignore());

            // SubAccount mappings
            CreateMap<SubAccount, SubAccountDto>().ReverseMap();
            CreateMap<SubAccount, CreateSubAccountDto>().ReverseMap();
            CreateMap<SubAccount, UpdateSubAccountDto>().ReverseMap();
            CreateMap<CreateSubAccountDto, SubAccount>()
                .ForMember(dest => dest.Branch, opt => opt.Ignore())
                .ForMember(dest => dest.InverseParent, opt => opt.Ignore())
                .ForMember(dest => dest.JVDetails, opt => opt.Ignore())
                .ForMember(dest => dest.Level, opt => opt.Ignore())
                .ForMember(dest => dest.Parent, opt => opt.Ignore())
                .ForMember(dest => dest.SubAccountType, opt => opt.Ignore())
                .ForMember(dest => dest.SubAccountsClients, opt => opt.Ignore())
                .ForMember(dest => dest.SubAccounts_Details, opt => opt.Ignore());

            // SubAccounts_Detail mappings
            CreateMap<SubAccounts_Detail, SubAccounts_DetailDto>().ReverseMap();
            CreateMap<SubAccounts_Detail, CreateSubAccounts_DetailDto>().ReverseMap();
            CreateMap<SubAccounts_Detail, UpdateSubAccounts_DetailDto>().ReverseMap();

            // SubAccounts_Level mappings
            CreateMap<SubAccounts_Level, SubAccounts_LevelDto>().ReverseMap();
            CreateMap<SubAccounts_Level, CreateSubAccounts_LevelDto>().ReverseMap();
            CreateMap<SubAccounts_Level, UpdateSubAccounts_LevelDto>().ReverseMap();

            // SubAccountsClient mappings (already exist)
            CreateMap<SubAccountsClient, SubAccountsClientDto>().ReverseMap();
            CreateMap<SubAccountsClient, CreateSubAccountsClientDto>().ReverseMap();
            CreateMap<SubAccountsClient, UpdateSubAccountsClientDto>().ReverseMap();

            // SubAccountsType mappings
            CreateMap<SubAccountsType, SubAccountsTypeDto>().ReverseMap();
            CreateMap<SubAccountsType, CreateSubAccountsTypeDto>().ReverseMap();
            CreateMap<SubAccountsType, UpdateSubAccountsTypeDto>().ReverseMap();
            CreateMap<CreateSubAccountsTypeDto, SubAccountsType>()
                .ForMember(dest => dest.SubAccounts, opt => opt.Ignore());
        }
    }
} 