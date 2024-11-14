using Todo.API.Dtos;

namespace Todo.API.Helpers.Mappers
{
    public static class UserMapper
    {
        public static UserModel MapCreateUserDtoToUserModel(this CreateUserDto createUserDto,string passwordHash, string salt)
        {
            return new UserModel
            {
                FullName = createUserDto.FullName,
                Email = createUserDto.Email,
                Password = passwordHash,
                Salt = salt,
                Role = createUserDto.Role
            };
        }

        public static UserModel MapUpdateUserDtoToUserModel(this UpdateUserDto updateUserDto)
        {
            return new UserModel
            {
                Id = updateUserDto.Id,
                FullName = updateUserDto.FullName,
                Email = updateUserDto.Email,
                Password = updateUserDto.Password,
                Role = updateUserDto.Role,
            };
        }

        public static UserResponseDto MapUserModelToUserDto(this UserModel user)
        {
            return new()
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                RoleName = user.Role.ToString(),
                CreatedDate = user.CreatedDate,
            };
        }

        public static IEnumerable<UserResponseDto> MapUsersModelToUsersDto(this IEnumerable<UserModel> users)
        {
            IEnumerable<UserResponseDto> usersDto = Enumerable.Empty<UserResponseDto>();

            usersDto = users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role,
                RoleName = u.Role.ToString(),
                CreatedDate = u.CreatedDate,
            }).ToList();

            return usersDto;
        }
    }
}
