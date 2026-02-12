using Microsoft.AspNetCore.Identity;

namespace AgenceLocationVoiture.Models
{
    public class Utilisateur : IdentityUser
    {
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public string Ville { get; set; } = string.Empty;
        public string CodePostal { get; set; } = string.Empty;
        public DateTime DateInscription { get; set; } = DateTime.Now;
        public bool EstActif { get; set; } = true;
        public string TypeUtilisateur { get; set; } = string.Empty;
    }
}