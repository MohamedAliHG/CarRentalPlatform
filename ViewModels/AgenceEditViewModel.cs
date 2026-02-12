using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AgenceLocationVoiture.ViewModels
{
    public class AgenceEditViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom de l'agence est requis")]
        [Display(Name = "Nom de l'agence")]
        public string NomAgence { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "L'adresse est requise")]
        [Display(Name = "Adresse")]
        public string Adresse { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ville est requise")]
        [Display(Name = "Ville")]
        public string Ville { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le code postal est requis")]
        [Display(Name = "Code postal")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Le code postal doit contenir 5 chiffres")]
        public string CodePostal { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le téléphone est requis")]
        [Phone(ErrorMessage = "Format de téléphone invalide")]
        [Display(Name = "Téléphone")]
        public string Telephone { get; set; } = string.Empty;

        [Url(ErrorMessage = "Format d'URL invalide")]
        [Display(Name = "Site web")]
        public string? SiteWeb { get; set; }

        [Display(Name = "Logo actuel")]
        public string? LogoUrl { get; set; }

        [Display(Name = "Nouveau logo")]
        public IFormFile? LogoFile { get; set; }
    }
}