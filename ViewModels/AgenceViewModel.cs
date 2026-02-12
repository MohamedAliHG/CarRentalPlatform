using System.ComponentModel.DataAnnotations;

namespace AgenceLocationVoiture.ViewModels
{
    public class AgenceViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Le nom de l'agence est requis")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
        public string NomAgence { get; set; } = string.Empty;


        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le téléphone est requis")]
        [Phone(ErrorMessage = "Format de téléphone invalide")]
        public string Telephone { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'adresse est requise")]
        public string Adresse { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ville est requise")]
        public string Ville { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le code postal est requis")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Le code postal doit contenir 5 chiffres")]
        public string CodePostal { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La description ne peut pas dépasser 500 caractères")]
        public string? Description { get; set; }

        [Url(ErrorMessage = "URL invalide")]
        public string? SiteWeb { get; set; }

       
        public string? LogoUrl { get; set; }
        public bool EstVerifiee { get; set; }
        public bool EstActif { get; set; } = true;
        public double NoteMoyenne { get; set; }
        public int NombreAvis { get; set; }
        public int NombreVoitures { get; set; }
        public int NombreOffres { get; set; }
    }

    public class AgenceRegistrationViewModel
    {
        [Required(ErrorMessage = "Le nom de l'agence est requis")]
        public string NomAgence { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le numéro SIRET est requis")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "Le SIRET doit contenir 14 chiffres")]
        public string NumeroSiret { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est requis")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est requis")]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le téléphone est requis")]
        [Phone(ErrorMessage = "Format de téléphone invalide")]
        public string Telephone { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'adresse est requise")]
        public string Adresse { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ville est requise")]
        public string Ville { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le code postal est requis")]
        public string CodePostal { get; set; } = string.Empty;

        // ✅ Nouvelles propriétés pour le logo
        [Display(Name = "Logo de l'agence")]
        public IFormFile? LogoFile { get; set; }

        [Display(Name = "Ou URL du logo")]
        [Url(ErrorMessage = "Format d'URL invalide")]
        public string? LogoUrl { get; set; }

        [Display(Name = "Description")]
        [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
        public string? Description { get; set; }

        [Display(Name = "Site web")]
        [Url(ErrorMessage = "Format d'URL invalide")]
        public string? SiteWeb { get; set; }


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