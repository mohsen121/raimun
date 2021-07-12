using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Raimun.Core.Common.Exceptions;
using Raimun.Core.Common.Interfaces;
using Raimun.Core.Common.Mappings;
using Raimun.Core.Common.Models.UserModels;
using Raimun.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raimun.Core.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<UserCreateModel>, IMapFrom<User>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateUserCommand, User>()
                .ReverseMap()
                .ForMember(x => x.ConfirmPassword, x => x.Ignore())
                ;
        }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserCreateModel>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            this._mapper = mapper;
        }
        public async Task<UserCreateModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
                return new UserCreateModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName
                };

            throw new BadRequestException(string.Join('\n', result.Errors.Select(x => x.Description).ToArray()));
        }
    }
}
