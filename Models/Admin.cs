namespace AgenceLocationVoiture.Models
{
    public class Admin : Utilisateur
    {
        public string? Departement { get; set; }
        public DateTime? DateDerniereConnexion { get; set; }
    }
}