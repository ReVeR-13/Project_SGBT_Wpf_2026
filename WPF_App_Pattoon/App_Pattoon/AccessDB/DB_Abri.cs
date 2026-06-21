
using Npgsql;
using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;


namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public static class DB_Abri
    {

        public static Dictionary<string, Abri> All_From_db()
        {
            Dictionary<string, Abri> retval = new Dictionary<string, Abri>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand(DB_Convertisseur.SelectFrom("f_All_abri()"),
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id") ?? string.Empty;
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date");
                    string libelle = DB_Convertisseur.String(reader, "_nom") ?? string.Empty;
                    string descr = DB_Convertisseur.String(reader, "_descrip") ?? string.Empty;
                    EStatutAbri? statut = DB_Convertisseur.StatutAbri(reader, "_statut");

                    if (!string.IsNullOrEmpty(id))
                    {
                        Abri abri = Abri.Creer(libelle, descr);
                        abri.Id = id;
                        abri.DateCreation = dateCreation ?? DateTime.Now;
                        abri.Statut = statut ?? EStatutAbri.HORS_SERVICE;
                        retval.Add(abri.Id, abri);

                        if (AllAbri.Find(abri.Id) == null)
                        {
                            AllAbri.Add(abri);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Abri? UnAbriId_db(string id)
        {
            Abri? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * from f_One_abri(@id)",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string idty = DB_Convertisseur.String(reader, "id") ?? string.Empty;
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date");
                    string nom = DB_Convertisseur.String(reader, "_nom") ?? string.Empty;
                    string descr = DB_Convertisseur.String(reader, "_descrip") ?? string.Empty;
                    EStatutAbri? statut = DB_Convertisseur.StatutAbri(reader, "_statut");

                    if (!string.IsNullOrEmpty(idty))
                    {
                        retval = Abri.Creer(nom, descr);
                        retval.Id = idty;
                        retval.Statut = statut ?? EStatutAbri.HORS_SERVICE;
                        retval.DateCreation = dateCreation ?? DateTime.Now;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Abri? UnAbriByNom_db(string nom)
        {
            Abri? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand("select * from f_One_abri_By_nom(@nom)",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@nom"].Value = nom;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string idty = DB_Convertisseur.String(reader, "id") ?? string.Empty;
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date");
                    string nom_ = DB_Convertisseur.String(reader, "_nom") ?? string.Empty;
                    string descr = DB_Convertisseur.String(reader, "_descrip") ?? string.Empty;
                    EStatutAbri? statut = DB_Convertisseur.StatutAbri(reader, "_statut");

                    if (!string.IsNullOrEmpty(idty))
                    {
                        retval = Abri.Creer(nom_, descr);
                        retval.Id = idty;
                        retval.Statut = statut ?? EStatutAbri.HORS_SERVICE;
                        retval.DateCreation = dateCreation ?? DateTime.Now;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }

        public static int Add(Abri abri)
        {
            try
            {
                string cmdtext = DB_Convertisseur.SelectFrom("f_create_abri(@nom,@descr)");

                var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
                {
                    { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, abri.Libelle) },
                    { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar , abri.Description) },
                };

                return Requets.ExecuteNonQuery(cmdtext, parametres);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        public static int Update(Abri abri)
        {
            try
            {
                string cmdtext = DB_Convertisseur.SelectFrom("f_update_abri(@id,@nom,@descr,@statut::e_statut_abri)");

                var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
                {
                    { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, abri.Libelle) },
                    { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , abri.Id) },
                    { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar , abri.Description) },
                    { "@statut" , (NpgsqlTypes.NpgsqlDbType.Varchar, abri.Statut.ToString()) }
                };

                return Requets.ExecuteNonQuery(cmdtext, parametres);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        public static int Delete(string id)
        {
            string cmdtext = "select * from f_delete_abri(@id)";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }
}
