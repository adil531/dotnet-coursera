using UserApi.Dtos;
using UserApi.Models;

namespace UserApi.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public IEnumerable<User> GetAll() => _users;

    public User? GetById(Guid id) => _users.FirstOrDefault(u => u.Id == id);

    public User Create(UserCreateDto dto)
    {
        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Age = dto.Age
        };

        _users.Add(user);
        return user;
    }

    public bool Update(Guid id, UserUpdateDto dto)
    {
        var existing = GetById(id);
        if (existing is null)
        {
            return false;
        }

        existing.FirstName = dto.FirstName;
        existing.LastName = dto.LastName;
        existing.Email = dto.Email;
        existing.Age = dto.Age;
        return true;
    }

    public bool Delete(Guid id)
    {
        var existing = GetById(id);
        if (existing is null)
        {
            return false;
        }

        _users.Remove(existing);
        return true;
    }
}

