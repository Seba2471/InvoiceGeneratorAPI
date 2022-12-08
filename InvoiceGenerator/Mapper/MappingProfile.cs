using AutoMapper;
using InvoiceGenerator.Entities;
using InvoiceGenerator.Models;
using InvoiceGenerator.Responses;

namespace InvoiceGenerator.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Invoice, InvoiceDto>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.InvoiceNumber, act => act.MapFrom(src => src.InvoiceNumber))
                .ForMember(dest => dest.IssueDate, act => act.MapFrom(src => src.IssueDate))
                .ForMember(dest => dest.SellerFullName, act => act.MapFrom(src => src.Seller.FullName))
                .ForMember(dest => dest.BuyerFullName, act => act.MapFrom(src => src.Buyer.FullName))
                .ForMember(dest => dest.Currency, act => act.MapFrom(src => src.Currency))
                .AfterMap((src, dest, context) =>
                {
                    var itemsAmount = src.InvoiceItems.Select(item => item.Quantity * item.Cost);
                    dest.Amount = itemsAmount.Sum();
                });

            CreateMap<Pagination<Invoice>, Pagination<InvoiceDto>>()
                .ConstructUsing((x, m) =>
                {
                    var items = m.Mapper.Map<ICollection<InvoiceDto>>(x.Items);
                    return new Pagination<InvoiceDto>(items, x.TotalPages, x.ItemsFrom, x.ItemsTo);
                }
                );
        }
    }
}
