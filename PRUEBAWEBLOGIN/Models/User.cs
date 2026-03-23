using System.ComponentModel.DataAnnotations;

namespace PRUEBAWEBLOGIN.Models
{
    //  usuario con información de perfil y control de seguridad
    public class User
    {
        [Key]
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "Operador";
        
        // Información personal
        public string FirstName { get; set; } = string.Empty;
        public string LastName1 { get; set; } = string.Empty;
        public string LastName2 { get; set; } = string.Empty;
        
        // Datos de identidad
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
        
        // Datos demográficos
        public string Nationality { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        
        // Contacto
        public string MainEmail { get; set; } = string.Empty;
        public string? SecondaryEmail { get; set; }
        
        public string MobilePhone { get; set; } = string.Empty;
        public string? SecondaryPhoneType { get; set; } = "Tipo";
        public string? SecondaryPhone { get; set; }
        
        // Contratación
        public string ContractType { get; set; } = string.Empty;
        public string ContractDate { get; set; } = string.Empty;
        
        // Institucional
        public string Institution { get; set; } = string.Empty;
        public string Status { get; set; } = "Activo";

        // Control de seguridad: intentos fallidos y bloqueo de cuenta
        public int AccessFailedCount { get; set; } = 0;
        public DateTime? LockoutEnd { get; set; }

        // Nombre completo formateado
        public string FullName => $"{LastName1} {LastName2}, {FirstName}";
    }
}