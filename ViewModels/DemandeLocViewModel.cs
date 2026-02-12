using AgenceLocationVoiture.Models;
using System.ComponentModel.DataAnnotations;

namespace AgenceLocationVoiture.ViewModels
{
   
    public class CreateDemandeLocViewModel
    {
        [Required(ErrorMessage = "La date de début est requise")]
        [DataType(DataType.Date)]
        public DateTime DateDebut { get; set; }

        [Required(ErrorMessage = "La date de fin est requise")]
        [DataType(DataType.Date)]
        public DateTime DateFin { get; set; }

        [Required(ErrorMessage = "Le lieu de prise en charge est requis")]
        public string LieuPriseEnCharge { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le lieu de retour est requis")]
        public string LieuRetour { get; set; } = string.Empty;

        public bool LivraisonDemandee { get; set; }
        public string? AdresseLivraison { get; set; }

        [StringLength(500, ErrorMessage = "Le message ne peut pas dépasser 500 caractères")]
        public string? MessageClient { get; set; }

        [Required]
        public int OffreLocId { get; set; }
    }
}