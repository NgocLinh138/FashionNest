using AutoMapper;
using FashionNest.Application.Features.Categories.Responses;
using FashionNest.Application.Features.Payments.Responses;
using FashionNest.Application.Features.Products.Responses;
using FashionNest.Application.Features.Roles.Responses;
using FashionNest.Application.Features.Users.Responses;
using FashionNest.Domain.Enums;

namespace FashionNest.Application.Mapper
{
    public class ServiceProfile : Profile 
    {
        public ServiceProfile()
        {
            #region Role
            CreateMap<RoleId, Guid>().ConvertUsing(roleId => roleId.Value);
            CreateMap<Role, RoleResponse>().ReverseMap();

            #endregion

            #region Product
            CreateMap<ProductId, Guid>().ConvertUsing(productId => productId.Value);

            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.IsRental, opt => opt.MapFrom(src => src.IsRental))
                .ReverseMap();
            #endregion

            #region Category
            CreateMap<CategoryId, Guid>().ConvertUsing(categoryId => categoryId.Value);

            CreateMap<Category, CategoryResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description)).ReverseMap();
            #endregion

            #region Payment
            CreateMap<PaymentId, Guid>().ConvertUsing(paymentId => paymentId.Value);
            CreateMap<UserId, Guid>().ConvertUsing(userId => userId.Value);
            CreateMap<PaymentMethod, string>().ConvertUsing(method => method.ToString());
            CreateMap<PaymentStatus, string>().ConvertUsing(status => status.ToString());

            CreateMap<Payment, PaymentResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Value))
                .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method.HasValue ? src.Method.Value.ToString() : string.Empty)) 
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.HasValue ? src.Status.Value.ToString() : string.Empty))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? string.Empty))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate.ToString("o")))
                .ReverseMap();
            #endregion

            #region User
            CreateMap<UserId, Guid>().ConvertUsing(userId => userId.Value);

            CreateMap<Role, UserRoleResponse>().ReverseMap();

            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ReverseMap();
            #endregion
        }
    }
}
