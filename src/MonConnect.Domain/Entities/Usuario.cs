
using MonConnect.Domain.Constants; 
public class Usuario
{
    public Guid Id {get; set;}
    public string Email {get; set;} = string.Empty;
    public string PasswordHash {get; set;} =string.Empty;
    public string Rol {get; set;} = Roles.Cajero;
    public bool IsActivo {get; set;} = true;
}