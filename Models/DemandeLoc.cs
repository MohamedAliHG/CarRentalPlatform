namespace AgenceLocationVoiture.Models
{
    public class DemandeLoc
    {
        public int Id { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string LieuPriseEnCharge { get; set; } = string.Empty;
        public string LieuRetour { get; set; } = string.Empty;
        public bool LivraisonDemandee { get; set; } = false;
        public string? AdresseLivraison { get; set; }
        public StatutDemande Statut { get; set; } = StatutDemande.EnAttente;
        public DateTime DateDemande { get; set; } = DateTime.Now;
        public string? MessageClient { get; set; }
        public string? ReponseAgence { get; set; }
        public DateTime? DateReponse { get; set; }
        public decimal? MontantTotal { get; set; }
        
        public string ClientId { get; set; } = string.Empty;
        public Client? Client { get; set; }
        
        public int OffreLocId { get; set; }
        public OffreLoc? OffreLoc { get; set; }
    }
    
    public enum StatutDemande
    {
        EnAttente,
        Acceptee,
        Refusee,
        Annulee,
        EnCours,
        Terminee
    }
}