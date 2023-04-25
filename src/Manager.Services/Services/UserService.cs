using AutoMapper;
using Manager.Core.Exceptions;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Services.DTOS;
using Manager.Services.Interfaces;

namespace Manager.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public virtual async Task<UserDTO> Create(UserDTO userDTO)
        {
            var userExists = await _userRepository.GetByEmail(userDTO.Email);

            if (userExists != null)
            {
                throw new DomainExceptions("The User already exists");
            }

            var user = _mapper.Map<User>(userDTO);
            user.Validate();

            var userCreated = await _userRepository.Create(user);

            return _mapper.Map<UserDTO>(userCreated);
        }

        public virtual async Task<UserDTO> Update(UserDTO userDTO)
        {
            var userExists = await _userRepository.Get(userDTO.Id);

            if (userExists == null)
            {
                throw new DomainExceptions("The User does not exist");
            }

            var user = _mapper.Map<User>(userDTO);
            user.Validate();

            var userUpdated = await _userRepository.Update(user);

            return _mapper.Map<UserDTO>(userUpdated);
        }

        public virtual async Task<UserDTO> Get(long id)
        {
            var userExists = await _userRepository.Get(id);

            if (userExists is null)
            {
                throw new DomainExceptions("User not found");
            }

            return _mapper.Map<UserDTO>(userExists);
        }

        public virtual async Task<List<UserDTO>> Get()
        {
            var allUsers = await _userRepository.Get();

            if (allUsers is null || allUsers.Equals(0))
            {
                throw new DomainExceptions("Users not found");
            }

            return _mapper.Map<List<UserDTO>>(allUsers);
        }

        public virtual async Task<UserDTO> GetByEmail(string email)
        {
            var userExists = await _userRepository.GetByEmail(email);

            if (userExists is null)
            {
                throw new DomainExceptions("User not found");
            }

            return _mapper.Map<UserDTO>(userExists);
        }

        public virtual async Task Remove(long id)
        {
            await _userRepository.Remove(id);
        }

        public virtual async Task<List<UserDTO>> SearchByEmail(string email)
        {
            var userExists = await _userRepository.SearchByEmail(email);

            if (userExists is null || userExists.Equals(0))
            {
                throw new DomainExceptions("Users not found");
            }

            return _mapper.Map<List<UserDTO>>(userExists);
        }

        public virtual async Task<List<UserDTO>> SearchByName(string name)
        {
            var userExists = await _userRepository.SearchByName(name);

            if (userExists is null || userExists.Equals(0))
            {
                throw new DomainExceptions("Users not found");
            }

            return _mapper.Map<List<UserDTO>>(userExists);
        }
    }
}
