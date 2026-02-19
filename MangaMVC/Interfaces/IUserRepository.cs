using System.Security.Cryptography.X509Certificates;
using Manga.Models; 
public interface IUserRepository
{
    Usuario GetUser(string username,string password);
}
