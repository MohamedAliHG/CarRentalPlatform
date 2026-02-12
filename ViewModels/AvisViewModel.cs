using System.ComponentModel.DataAnnotations;

namespace AgenceLocationVoiture.ViewModels
{
      public class CreateAvisViewModel
    {
        [Required(ErrorMessage = "La note est requise")]
        [Range(1, 5, ErrorMessage = "La note doit être entre 1 et 5")]
        public int Note { get; set; }

        [Required(ErrorMessage = "Le commentaire est requis")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Le commentaire doit contenir entre 10 et 1000 caractères")]
        public string Commentaire { get; set; } = string.Empty;

        [Required]
        public string AgenceId { get; set; } = string.Empty;
    }
}