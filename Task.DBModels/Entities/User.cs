using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task.DBModels.Entities
{
    [Table("AspNetUsers")]
    public class User : IdentityUser<int>
    {

    }
}