namespace AgenceLocationVoiture.Models
{
    public class Client : Utilisateur
    {
        public DateTime DateNaissance { get; set; }
        public string NumeroPermis { get; set; } = string.Empty;
        public DateTime DateObtentionPermis { get; set; }
        public string? PhotoPermisUrl { get; set; }
        public bool PermisVerifie { get; set; } = false;

        public ICollection<DemandeLoc> DemandesLocation { get; set; } = new List<DemandeLoc>();
        public ICollection<Avis> AvisDonnes { get; set; } = new List<Avis>();
    }
}