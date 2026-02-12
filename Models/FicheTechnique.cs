namespace AgenceLocationVoiture.Models
{

    public class FicheTechnique
    {
        public int Id { get; set; }
        public string MoteurType { get; set; } = string.Empty;
        public int Puissance { get; set; } 
        public decimal Consommation { get; set; } 
        public decimal EmissionCO2 { get; set; } 
        public int CapaciteReservoir { get; set; } 
        public int VolumeCodeffre { get; set; }
        public decimal Longueur { get; set; } 
        public decimal Largeur { get; set; }
        public decimal Hauteur { get; set; }
        public decimal PoidsVide { get; set; } 
        public string? Equipements { get; set; } 
        public string? OptionsSecurite { get; set; }

        public Voiture? Voiture { get; set; }
    }
}