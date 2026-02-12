using AgenceLocationVoiture.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AgenceLocationVoiture.ViewModels
{
    public class VoitureViewModel
    {
        // Informations de base de la voiture
        public int? Id { get; set; }

        [Required(ErrorMessage = "La marque est obligatoire")]
        public string Marque { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le modèle est obligatoire")]
        public string Modele { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'année est obligatoire")]
        [Range(1900, 2100, ErrorMessage = "Année invalide")]
        public int Annee { get; set; }

        [Required(ErrorMessage = "L'immatriculation est obligatoire")]
        public string Immatriculation { get; set; } = string.Empty;

        [Required(ErrorMessage = "La catégorie est obligatoire")]
        public CategorieVoiture Categorie { get; set; }

        [Required(ErrorMessage = "Le type de carburant est obligatoire")]
        public TypeCarburant TypeCarburant { get; set; }

        [Required(ErrorMessage = "Le type de transmission est obligatoire")]
        public TypeTransmission Transmission { get; set; }

        [Required(ErrorMessage = "Le nombre de places est obligatoire")]
        [Range(2, 9, ErrorMessage = "Le nombre de places doit être entre 2 et 9")]
        public int NombrePlaces { get; set; }

        [Required(ErrorMessage = "Le nombre de portes est obligatoire")]
        [Range(2, 5, ErrorMessage = "Le nombre de portes doit être entre 2 et 5")]
        public int NombrePortes { get; set; }

        [Required(ErrorMessage = "Le kilométrage est obligatoire")]
        [Range(0, double.MaxValue, ErrorMessage = "Le kilométrage doit être positif")]
        public decimal Kilometrage { get; set; }

        [Required(ErrorMessage = "La couleur est obligatoire")]
        public string Couleur { get; set; } = string.Empty;

        public bool EstDisponible { get; set; } = true;

        // Photos
        public IFormFile? PhotoPrincipaleFile { get; set; }
        public string? PhotoPrincipaleUrl { get; set; }
        public List<IFormFile>? PhotosFiles { get; set; }
        public List<string>? PhotosUrls { get; set; }

        // ✅ Nouvelles propriétés pour la fiche technique
        public bool AjouterFicheTechnique { get; set; } = false;

        [Display(Name = "Type de moteur")]
        public string? MoteurType { get; set; }

        [Display(Name = "Puissance (chevaux)")]
        [Range(0, 1000, ErrorMessage = "La puissance doit être entre 0 et 1000 ch")]
        public int? Puissance { get; set; }

        [Display(Name = "Consommation (L/100km)")]
        [Range(0, 50, ErrorMessage = "La consommation doit être entre 0 et 50 L/100km")]
        public decimal? Consommation { get; set; }

        [Display(Name = "Émission CO2 (g/km)")]
        [Range(0, 500, ErrorMessage = "L'émission de CO2 doit être entre 0 et 500 g/km")]
        public decimal? EmissionCO2 { get; set; }

        [Display(Name = "Capacité du réservoir (litres)")]
        [Range(0, 200, ErrorMessage = "La capacité du réservoir doit être entre 0 et 200 litres")]
        public int? CapaciteReservoir { get; set; }

        [Display(Name = "Volume du coffre (litres)")]
        [Range(0, 2000, ErrorMessage = "Le volume du coffre doit être entre 0 et 2000 litres")]
        public int? VolumeCodeffre { get; set; }

        [Display(Name = "Longueur (mètres)")]
        [Range(0, 10, ErrorMessage = "La longueur doit être entre 0 et 10 mètres")]
        public decimal? Longueur { get; set; }

        [Display(Name = "Largeur (mètres)")]
        [Range(0, 5, ErrorMessage = "La largeur doit être entre 0 et 5 mètres")]
        public decimal? Largeur { get; set; }

        [Display(Name = "Hauteur (mètres)")]
        [Range(0, 5, ErrorMessage = "La hauteur doit être entre 0 et 5 mètres")]
        public decimal? Hauteur { get; set; }

        [Display(Name = "Poids à vide (kg)")]
        [Range(0, 10000, ErrorMessage = "Le poids doit être entre 0 et 10000 kg")]
        public decimal? PoidsVide { get; set; }

        [Display(Name = "Équipements")]
        [StringLength(1000, ErrorMessage = "Les équipements ne peuvent pas dépasser 1000 caractères")]
        public string? Equipements { get; set; }

        [Display(Name = "Options de sécurité")]
        [StringLength(1000, ErrorMessage = "Les options de sécurité ne peuvent pas dépasser 1000 caractères")]
        public string? OptionsSecurite { get; set; }

        public string? AgenceId { get; set; }
    }
}