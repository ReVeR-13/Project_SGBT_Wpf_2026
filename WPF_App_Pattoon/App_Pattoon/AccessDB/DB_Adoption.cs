
using Npgsql;
using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;

namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public class DB_Adoption
    {
        public static Dictionary<string, Adoption> All_From_Db()
        {
            Dictionary<string, Adoption> retval = new Dictionary<string, Adoption>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_adoption " +
                                                           $"order by id_adoption ",
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id_adoption");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string detail = DB_Convertisseur.String(reader, "details");
                    EStatutValidation? statut = DB_Convertisseur.StatutValidation(reader, "statut");
                    Demande? demande = DB_Convertisseur.Demande(reader, "id_demande");
                    DateTime? dateD = DB_Convertisseur.Date(reader, "date_debut");
                    DateTime? dateF = DB_Convertisseur.Date(reader, "date_fin"); 
                    string? refus = DB_Convertisseur.String(reader, "raison_refus");

                    if (demande != null && statut != null)
                    {
                        Adoption tpe = Adoption.Creer(demande, detail);
                        tpe.Id = id;
                        tpe.DateCreation = (DateTime)dateCreation;
                        tpe.Statut = (EStatutValidation)statut;
                        tpe.DateD = dateD;
                        tpe.DateF = dateF;
                        tpe.RaisonRefus = refus;

                        retval.Add(tpe.Id, tpe);

                        if (AllAdoption.Find(tpe.Id) == null)
                        {
                            AllAdoption.Add(tpe);
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
        public static Adoption? UnAdoptionById(Adoption adoption)
        {
            Adoption? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_adoption " +
                                                           $"where id_adoption = @id ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = adoption.Id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string id = DB_Convertisseur.String(reader, "id_adoption");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string detail = DB_Convertisseur.String(reader, "details");
                    EStatutValidation? statut = DB_Convertisseur.StatutValidation(reader, "statut");
                    Demande? demande = DB_Convertisseur.Demande(reader, "id_demande");
                    DateTime? dateD = DB_Convertisseur.Date(reader, "date_debut");
                    DateTime? dateF = DB_Convertisseur.Date(reader, "date_fin");
                    string? refus = DB_Convertisseur.String(reader, "raison_refus");

                    if (demande != null && statut != null)
                    {
                        retval = Adoption.Creer(demande, detail);
                        retval.DateCreation = (DateTime)dateCreation;
                        retval.Statut = (EStatutValidation)statut;
                        retval.DateD = dateD == null ? null : dateD;
                        retval.DateF = dateF == null ? null : dateF;
                        retval.Id = id;
                        retval.RaisonRefus = refus;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Adoption? UnAdoptionByDemande(Demande demande)
        {
            Adoption? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * from t_adoption " +
                                                           $"where id_demande = @iddemande",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@iddemande", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@iddemande"].Value = demande.Id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string id = DB_Convertisseur.String(reader, "id_adoption");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string detail = DB_Convertisseur.String(reader, "details");
                    EStatutValidation? statut = DB_Convertisseur.StatutValidation(reader, "statut");
                    Demande? fdemande = DB_Convertisseur.Demande(reader, "id_demande");
                    DateTime? dateD = DB_Convertisseur.Date(reader, "date_debut");
                    DateTime? dateF = DB_Convertisseur.Date(reader, "date_fin");
                    string? refus = DB_Convertisseur.String(reader, "raison_refus");

                    if (demande != null && statut != null)
                    {
                        retval = Adoption.Creer(fdemande, detail);
                        retval.Id = id;
                        retval.DateCreation = (DateTime)dateCreation;
                        retval.Statut = (EStatutValidation)statut;
                        retval.DateD = (DateTime)dateD;
                        retval.DateF = (DateTime)dateF;
                        retval.RaisonRefus = refus;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }

        public static int Add(Adoption adoption)
        {
            string cmdtext = "insert into t_adoption(id_adoption, date_creation, details, statut, id_demande, date_debut, date_fin, raison_refus ) " +
                "values (@id, @date, @detail, @statut::e_statut_adoption, @iddm, @dtedebut , @dtefin, @refus) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, adoption.DateCreation) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, adoption.Id) },
                { "@detail",(NpgsqlTypes.NpgsqlDbType.Varchar, adoption.Info) },
                { "@statut",(NpgsqlTypes.NpgsqlDbType.Varchar , adoption.Statut.ToString()) },
                { "@iddm",(NpgsqlTypes.NpgsqlDbType.Varchar , adoption.Demande.Id) },
                { "@dtedebut",(NpgsqlTypes.NpgsqlDbType.Date , adoption.DateD) },
                { "@dtefin",(NpgsqlTypes.NpgsqlDbType.Date , adoption.DateF) },
                { "@refus",(NpgsqlTypes.NpgsqlDbType.Varchar, adoption.RaisonRefus) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(Adoption adoption)
        {

            string cmdtext = "update t_adoption set details = @detail, statut = @statut::e_statut_adoption, " +
                "id_demande =@iddm ,date_debut = @dtedebut ,date_fin = @dtefin, raison_refus = @refus " +
                "where id_adoption = @id ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, adoption.Id) },
                { "@detail",(NpgsqlTypes.NpgsqlDbType.Varchar, adoption.Info) },
                { "@refus",(NpgsqlTypes.NpgsqlDbType.Varchar, adoption.RaisonRefus) },
                { "@statut",(NpgsqlTypes.NpgsqlDbType.Varchar , adoption.Statut.ToString()) },
                { "@iddm",(NpgsqlTypes.NpgsqlDbType.Varchar , adoption.Demande.Id) },
                { "@dtedebut",(NpgsqlTypes.NpgsqlDbType.Date , adoption.DateD) },
                { "@dtefin",(NpgsqlTypes.NpgsqlDbType.Date , adoption.DateF) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(Adoption adoption)
        {
            string cmdtext = "delete from t_adoption where id_adoption = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, adoption.Id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }
}
