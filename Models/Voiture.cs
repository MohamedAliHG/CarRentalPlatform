namespace AgenceLocationVoiture.Models
{
    public class Voiture
    {
        public int Id { get; set; }
        public string Marque { get; set; } = string.Empty;
        public string Modele { get; set; } = string.Empty;
        public int Annee { get; set; }
        public string Immatriculation { get; set; } = string.Empty;
        public CategorieVoiture Categorie { get; set; }
        public TypeCarburant TypeCarburant { get; set; }
        public TypeTransmission Transmission { get; set; }
        public int NombrePlaces { get; set; }
        public int NombrePortes { get; set; }
        public decimal Kilometrage { get; set; }
        public string Couleur { get; set; } = string.Empty;
        public bool EstDisponible { get; set; } = true;
        public string? PhotoPrincipaleUrl { get; set; }
        public List<string> PhotosUrls { get; set; } = new List<string>();

        public string AgenceId { get; set; } = string.Empty;
        public Agence? Agence { get; set; }
        
        public int? FicheTechniqueId { get; set; }
        public FicheTechnique? FicheTechnique { get; set; }

        public ICollection<OffreLoc> OffresLocation { get; set; } = new List<OffreLoc>();
    }
    
    public enum CategorieVoiture
    {
        Citadine,
        Berline,
        SUV,
        Monospace,
        Sportive,
        Utilitaire,
        Luxe
    }
    
    public enum TypeCarburant
    {
        Essence,
        Diesel,
        Hybride,
        Electrique,
        GPL
    }
    
    public enum TypeTransmission
    {
        Manuelle,
        Automatique
    }
}