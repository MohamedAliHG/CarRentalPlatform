using AgenceLocationVoiture.Models;
using System.ComponentModel.DataAnnotations;

namespace AgenceLocationVoiture.ViewModels
{
    public class RechercheViewModel
    {
        public string? Marque { get; set; }
        public string? Modele { get; set; }
        public CategorieVoiture? Categorie { get; set; }
        public TypeCarburant? Carburant { get; set; }
        public TypeTransmission? Transmission { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateDebut { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateFin { get; set; }

        public string? Ville { get; set; }

        [Range(0, 10000, ErrorMessage = "Le prix doit être entre 0 et 10000")]
        public decimal? PrixMax { get; set; }

        [Range(2000, 2030, ErrorMessage = "L'année doit être entre 2000 et 2030")]
        public int? AnneeMin { get; set; }

        public int? NombrePlacesMin { get; set; }
        public bool? AssuranceIncluse { get; set; }
        public bool? LivraisonPossible { get; set; }

        // Pagination
        public int Page { get; set; } = 1;
        public int TaillePage { get; set; } = 12;

        // Résultats
        public List<OffreLocViewModel> Resultats { get; set; } = new List<OffreLocViewModel>();
        public int TotalResultats { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalResultats / TaillePage);
    }
}