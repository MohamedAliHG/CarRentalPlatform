namespace AgenceLocationVoiture.Models
{
    public class Avis
    {
        public int Id { get; set; }
        public int Note { get; set; } 
        public string Commentaire { get; set; } = string.Empty;
        public DateTime DateAvis { get; set; } = DateTime.Now;
        public bool EstVerifie { get; set; } = false;
        public bool EstVisible { get; set; } = true;

        public int? NoteQualiteVehicule { get; set; }
        public int? NoteServiceClient { get; set; }
        public int? NoteRapportQualitePrix { get; set; }
        public int? NoteProprete { get; set; }
        
        public string? ReponseAgence { get; set; }
        public DateTime? DateReponse { get; set; }

        public string ClientId { get; set; } = string.Empty;
        public Client? Client { get; set; }
        
        public string AgenceId { get; set; } = string.Empty;
        public Agence? Agence { get; set; }
    }
}