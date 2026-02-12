using System.ComponentModel.DataAnnotations;

namespace AgenceLocationVoiture.ViewModels
{
   
    public class ClientRegistrationViewModel
    {
        [Required(ErrorMessage = "Le nom est requis")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est requis")]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le téléphone est requis")]
        public string Telephone { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'adresse est requise")]
        public string Adresse { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ville est requise")]
        public string Ville { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le code postal est requis")]
        public string CodePostal { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date de naissance est requise")]
        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }

        [Required(ErrorMessage = "Le numéro de permis est requis")]
        public string NumeroPermis { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date d'obtention du permis est requise")]
        [DataType(DataType.Date)]
        public DateTime DateObtentionPermis { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmation du mot de passe est requise")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}