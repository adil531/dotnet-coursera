using UserApi.Dtos;
using UserApi.Models;

namespace UserApi.Repositories;

public interface IUserRepository
{
    IEnumerable<User> GetAll();
    User? GetById(Guid id);
    User Create(UserCreateDto dto);
    bool Update(Guid id, UserUpdateDto dto);
    bool Delete(Guid id);
}

