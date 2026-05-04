using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planora.DataAccess.Models;

public class UserDB
{
    [Key]
    [Column("Id")]
    public required Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public bool Tovholder { get; set; }
    public bool Deleted { get; set; }
    public List<TaskDB> Tasks { get; set; }
        
    public UserDB() { }

}