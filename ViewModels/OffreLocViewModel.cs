using System.ComponentModel.DataAnnotations;

namespace AgenceLocationVoiture.ViewModels
{
    public class OffreLocViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est requis")]
        [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
        public string Titre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La description est requise")]
        [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prix par jour est requis")]
        [Range(1, 10000, ErrorMessage = "Le prix doit être entre 1 et 10000")]
        public decimal PrixParJour { get; set; }

       

        [Range(0, 10000, ErrorMessage = "La caution doit être entre 0 et 10000")]
        public decimal? CautionMontant { get; set; }

        [Required(ErrorMessage = "Le kilométrage inclus est requis")]
        [Range(0, 1000, ErrorMessage = "Le kilométrage doit être entre 0 et 1000")]
        public int KilometrageInclus { get; set; }

        [Range(0, 10, ErrorMessage = "Les frais doivent être entre 0 et 10")]
        public decimal? FraisKmSupplementaire { get; set; }

        [Required(ErrorMessage = "L'âge minimum est requis")]
        [Range(18, 99, ErrorMessage = "L'âge doit être entre 18 et 99")]
        public int AgeMinimumConducteur { get; set; } = 21;

        [Required(ErrorMessage = "L'ancienneté du permis est requise")]
        [Range(0, 50, ErrorMessage = "L'ancienneté doit être entre 0 et 50")]
        public int AnciennetePermisMinimum { get; set; } = 1;

        public bool AssuranceIncluse { get; set; } = true;
        public bool LivraisonPossible { get; set; } = false;
        public decimal? FraisLivraison { get; set; }

        [Required(ErrorMessage = "La date de début est requise")]
        [DataType(DataType.Date)]
        public DateTime DateDebut { get; set; }

        [Required(ErrorMessage = "La date de fin est requise")]
        [DataType(DataType.Date)]
        public DateTime DateFin { get; set; }

        public bool EstActive { get; set; } = true;
        public DateTime DateCreation { get; set; }

        [Required(ErrorMessage = "La voiture est requise")]
        public int VoitureId { get; set; }
        public VoitureViewModel? Voiture { get; set; }

        public string AgenceId { get; set; } = string.Empty;
        public string? AgenceNom { get; set; }

        public int NombreDemandes { get; set; }
    }
}