using Npgsql;
using System.Data.SqlTypes;
using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;


namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public static class DB_Convertisseur
    {
        public static string? String(NpgsqlDataReader reader,string colonne)
        {
            int index = reader.GetOrdinal(colonne);
            return reader.IsDBNull(index) ? null :  reader.GetString(index);
        }
        public static DateTime? Date(NpgsqlDataReader reader,string colonne)
        {
            return reader.IsDBNull(reader.GetOrdinal(colonne)) ? null : reader.GetDateTime(reader.GetOrdinal(colonne));
        }
        public static bool Bool(NpgsqlDataReader reader,string colonne) { return (bool)reader[colonne]; }

        public static EStatutAbri? StatutAbri(NpgsqlDataReader reader,string colonne)
        {
            EStatutAbri? retval = null;
            string? valeur = String(reader,colonne);
            if (Enum.TryParse(valeur,true,out EStatutAbri result))
            {
                retval = result; 
            }
            return retval;
        }
        public static EStatutAnimal? StatutAnimal(NpgsqlDataReader reader, string colonne)
        {
            EStatutAnimal? retval = null;
            string? valeur = String(reader, colonne);
            if (Enum.TryParse(valeur,true ,out EStatutAnimal result))
            {
                retval = result;
            }
            return retval;
        }
        public static ETypeDemande? TypeDemande(NpgsqlDataReader reader, string colonne)
        {
            ETypeDemande? retval = null;
            string? valeur = String(reader, colonne);
            if (Enum.TryParse(valeur, true, out ETypeDemande result))
            {
                retval = result;
            }
            return retval;
        }
        public static EStatutDemande? StatutDemande(NpgsqlDataReader reader, string colonne)
        {
            EStatutDemande? retval = null;
            string? valeur = String(reader, colonne);
            if (Enum.TryParse(valeur,true, out EStatutDemande result))
            {
                retval = result;
            }
            return retval;
        }
        public static EStatutValidation? StatutValidation(NpgsqlDataReader reader, string colonne)
        {
            EStatutValidation? retval = null;
            string? valeur = String(reader, colonne);
            if (Enum.TryParse(valeur, true, out EStatutValidation result))
            {
                retval = result;
            }
            return retval;
        }

        public static Animal? Animal(NpgsqlDataReader reader,string colonne)
        {
            string? info = String(reader, colonne);
            return info == null? null: AllAnimal.Rechercher(info);

        }
        public static Contact? Contact(NpgsqlDataReader reader, string colonne)
        {
            string? info = String(reader, colonne);
            return info == null ? null : AllContacts.Find(info);

        }
        public static Vaccin? Vaccin(NpgsqlDataReader reader, string colonne)
        {
            string? info = reader.GetString(reader.GetOrdinal(colonne));
            return info == null ? null : AllVaccin.Find(info);

        }
        public static Compatibilite? Compatibilite(NpgsqlDataReader reader, string colonne)
        {
            string? info = reader.GetString(reader.GetOrdinal(colonne));
            return info == null ? null : AllCompatibilite.Find(info);
        }
        public static Couleur? Couleur(NpgsqlDataReader reader, string colonne)
        {
            string? info = reader.GetString(reader.GetOrdinal(colonne));
            return info == null ? null : AllCouleur.Find(info);
        }
        public static Demande? Demande(NpgsqlDataReader reader, string colonne)
        {
            string? info = reader.GetString(reader.GetOrdinal(colonne));
            return info == null ? null : AllDemande.Find(info);

        }
        public static MotifEntree? MotifEntree(NpgsqlDataReader reader, string colonne)
        {
            string? info = reader.GetString(reader.GetOrdinal(colonne));
            return info == null ? null : AllMotifsEntrees.FindById(info);

        }
        public static MotifSortie? MotifSortie(NpgsqlDataReader reader, string colonne)
        {
            string? info = reader.GetString(reader.GetOrdinal(colonne));
            return info == null ? null : AllMotifsSortie.FindById(info);

        }

        public static string SelectFrom(string func) => $"Select * from {func}";

    }

    public static class Requets
    {
        public static int ResultCommande(string cmdText, Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)> parametres)
        {
            int result = 0;
            using (var sqlCmd = new NpgsqlCommand(cmdText, AccessDB.SqlConn))
            {

                if (parametres != null)
                {
                    foreach (var param in parametres)
                    {
                        sqlCmd.Parameters.Add(new NpgsqlParameter(param.Key, param.Value.Type));
                    }
                }

                sqlCmd.Prepare();

                if (parametres != null)
                {
                    int i = 0;
                    foreach (var param in parametres)
                    {
                        sqlCmd.Parameters[i].Value = param.Value.Value ?? DBNull.Value;
                        i++;
                    }

                }

                try
                {
                    using (var sqlreader = sqlCmd.ExecuteReader())
                    {
                        if (sqlreader.Read())
                        {
                            if (sqlreader[0] != null)
                            {
                                result = 1;
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.Read();

                    throw new ExceptionDB(cmdText, ex.Message);
                }
                return result;
            }
        }
        public static int ExecuteNonQuery(string cmdText, Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)> parametres)
        {
            using (NpgsqlCommand npgsql = new NpgsqlCommand(cmdText, AccessDB.SqlConn))
            {
                if (parametres != null)
                {
                    foreach (var param in parametres)
                    {
                        npgsql.Parameters.Add(new NpgsqlParameter(param.Key, param.Value.Type));
                    }
                }

                npgsql.Prepare();

                if (parametres != null)
                {
                    int i = 0;
                    foreach (var param in parametres)
                    {
                        npgsql.Parameters[i].Value = param.Value.Value ?? DBNull.Value;
                        i++;
                    }
                }

                try
                {
                    return npgsql.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                    throw new ExceptionDB(cmdText, ex.Message);
                }
            }
        }

    }
}
