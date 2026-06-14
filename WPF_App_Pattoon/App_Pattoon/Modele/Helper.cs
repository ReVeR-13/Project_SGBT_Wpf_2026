using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_App_Pattoon_Animalerie.Modele
{
    public enum ESexe
    {
        M,
        F
    }
    public enum EStatutAnimal
    {
        EXAMINATION,
        REFUGE,
        ACCUEIL,
        ADOPTION,
        PROPRIETAIRE,
        DECEDE
    }
    public enum EStatutAbri
    {
        DISPONIBLE,
        OCCUPE,
        HORS_SERVICE,
    }
    public enum EStatutDemande
    {
        EXAMINATION,
        EN_COURS,
        VALIDATION,
        TERMINEE,
        CLOTUREE,
        ANNULEE
    }
    public enum ETypeDemande
    {
        ENTREE,
        SORTIE,
        ADOPTION,
        ACCUEIL,
        DECES,
        NAISSANCE,
        INFO
    }
    public enum EStatutValidation
    {
        EN_COURS,
        ACCEPTEE,
        REFUSEE
    }
    public enum ETypeCompatibilite
    {
        Chat,
        Chien,
        Jeune_Enfant,
        Enfant,
        Jardin,
        Poney
    }
    public enum EValeurCompatibilite
    {
        Non,
        Oui
    }
    public interface ITable
    {
        string Id { get; }
        DateTime DateCreation { get; }
    } 

    public static class LesMessage
    {
        public static string SuccesAjout => "[Succes] L'element a été ajouter";
        public static string ErrorAjout => "[Echec] L'element n'a pu etre ajouter";
        public static string SuccesSuppression => "[Succes] L'element a été supprimé";
        public static string ErrorSuppression => "[Echec] L'element n'a pu etre supprimé";
        public static string ErrorMaj => "[Echec] L'element n'a pu etre mise à jour";
        public static string SuccesMaj => "[Succes] L'element a été mise à jour";


        public static string ErrorExiste => "[Echec] L'element existe deja";
    }
  
}
