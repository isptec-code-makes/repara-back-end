using AutoMapper;
using Repara.DTO;
using Repara.DTO.Montagem;
using Repara.Model;

namespace Repara.Helpers.Mappers
{
    public class MontagemProfile : Profile
    {
        private const string DateOnlyFormat = "dd-MM-yyyy";
        private const string DateTimeFormat = "dd-MM-yyyy HH:mm:ss";

        public MontagemProfile()
        {

            CreateMap(typeof(PagedList<>), typeof(PagedList<>))
            .ConvertUsing(typeof(PagedListConverter<,>));

            CreateMap<Montagem, MontagemDTO>()
                .ForMember(c => c.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn.ToString(DateTimeFormat)))
                .ForMember(c => c.UpdatedOn, opt => opt.MapFrom(src => src.UpdatedOn.ToString(DateTimeFormat)));


            CreateMap<MontagemCreateDTO, Montagem>();
            CreateMap<Montagem, Montagem>();


            CreateMap<MontagemUpdateDTO, Montagem>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) =>
                {

                    if (srcMember == null) return false;


                    if (srcMember.GetType().IsValueType)
                    {
                        var defaultValue = Activator.CreateInstance(srcMember.GetType());
                        return !srcMember.Equals(defaultValue); // Só mapear se não for o valor padrão
                    }
                    return true;
                }));
        }
    }
}
