namespace AgenceLocationVoiture.Models
{
    public class OffreLoc
    {
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal PrixParJour { get; set; }
        public decimal? PrixParSemaine { get; set; }
        public decimal? CautionMontant { get; set; }
        public int KilometrageInclus { get; set; } 
        public decimal? FraisKmSupplementaire { get; set; }
        public int AgeMinimumConducteur { get; set; } = 21;
        public int AnciennetePermisMinimum { get; set; } = 1;
        public bool AssuranceIncluse { get; set; } = true;
        public bool LivraisonPossible { get; set; } = false;
        public decimal? FraisLivraison { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public bool EstActive { get; set; } = true;
        public DateTime DateCreation { get; set; } = DateTime.Now;

        public int VoitureId { get; set; }
        public Voiture? Voiture { get; set; }
        
        public string AgenceId { get; set; } = string.Empty;
        public Agence? Agence { get; set; }

        public ICollection<DemandeLoc> Demandes { get; set; } = new List<DemandeLoc>();
    }
}