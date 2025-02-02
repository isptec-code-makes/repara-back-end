using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Repara.DTO;
using Repara.DTO.Cliente;
using Repara.Model;

namespace Repara.Helpers.Mappers
{
    public class ClienteProfile: Profile
    {
        private const string DateOnlyFormat = "dd-MM-yyyy";
        private const string DateTimeFormat = "dd-MM-yyyy HH:mm:ss";

        public ClienteProfile()
        {

            CreateMap(typeof(PagedList<>), typeof(PagedList<>))
            .ConvertUsing(typeof(PagedListConverter<,>));

            CreateMap<Cliente, ClienteDTO>()
                .ForMember(c => c.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn.ToString(DateTimeFormat)))
                .ForMember(c => c.UpdatedOn, opt => opt.MapFrom(src => src.UpdatedOn.ToString(DateTimeFormat)));


            CreateMap<ClienteCreateDTO, Cliente>();
               

            CreateMap<ClienteUpdateDTO, Cliente>()
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
