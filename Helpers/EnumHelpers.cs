using AgenceLocationVoiture.Models;

namespace AgenceLocationVoiture.Helpers
{
    public static class EnumHelpers
    {
        public static string GetStatutDemandeLabel(StatutDemande statut)
        {
            return statut switch
            {
                StatutDemande.EnAttente => "En attente",
                StatutDemande.Acceptee => "Acceptée",
                StatutDemande.Refusee => "Refusée",
                StatutDemande.Annulee => "Annulée",
                StatutDemande.EnCours => "En cours",
                StatutDemande.Terminee => "Terminée",
                _ => statut.ToString()
            };
        }

        public static string GetStatutDemandeBadgeClass(StatutDemande statut)
        {
            return statut switch
            {
                StatutDemande.EnAttente => "badge bg-warning text-dark",
                StatutDemande.Acceptee => "badge bg-success",
                StatutDemande.Refusee => "badge bg-danger",
                StatutDemande.Annulee => "badge bg-secondary",
                StatutDemande.EnCours => "badge bg-info",
                StatutDemande.Terminee => "badge bg-primary",
                _ => "badge bg-secondary"
            };
        }
    }
}