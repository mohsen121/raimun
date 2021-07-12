using AutoMapper;
using Raimun.Core.Common.Mappings;
using Raimun.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raimun.Core.Common.Models.UserModels
{
    public class UserSignInResponseModel : IMapFrom<User>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string JWT { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserSignInResponseModel>()
                .ReverseMap()
                ;
        }
    }
}
