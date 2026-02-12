namespace AgenceLocationVoiture.Models
{
    public class Agence : Utilisateur
    {
        public string NomAgence { get; set; } = string.Empty;
        public string NumeroSiret { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SiteWeb { get; set; } = string.Empty;
        public TimeSpan HeureOuverture { get; set; }
        public TimeSpan HeureFermeture { get; set; }
        public bool EstVerifiee { get; set; } = false;
        public string? LogoUrl { get; set; }

        public ICollection<Voiture> Voitures { get; set; } = new List<Voiture>();
        public ICollection<OffreLoc> OffresLocation { get; set; } = new List<OffreLoc>();
        public ICollection<Avis> AvisRecus { get; set; } = new List<Avis>();
    }
}