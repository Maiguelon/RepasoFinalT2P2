using System.Security.Cryptography.X509Certificates;
using TiendaManga.Models; 
public interface IUserRepository
{
    Usuario GetUser(string username,string password);
}
