using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;
using Npgsql;

namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public class DB_Entree
    {
        public static Dictionary<string, Entree> All_From_Db()
        {
            Dictionary<string, Entree> retval = new Dictionary<string, Entree>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_entree " +
                                                           $"order by id_entree ",
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id_entree");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string descr = DB_Convertisseur.String(reader, "details" );
                    MotifEntree? motif = DB_Convertisseur.MotifEntree(reader, "id_motif");
                    Demande? dem = DB_Convertisseur.Demande(reader, "id_demande");

                    Entree? tpe = Entree.Creer(dem,motif,descr);
                    tpe.Id = id;
                    tpe.DateCreation = (DateTime)dateCreation;
                    retval.Add(tpe.Id, tpe);

                    if (AllEntree.Find(tpe.Id) == null)
                    {
                        AllEntree.Add(tpe);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Entree? UnEntreeById(Entree @in)
        {
            Entree? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select *" +
                                                           $"from t_entree " +
                                                           $"where id_entree = @id ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = @in.Id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string id = DB_Convertisseur.String(reader, "id_entree");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string descr = DB_Convertisseur.String(reader, "details");
                    MotifEntree? motif = DB_Convertisseur.MotifEntree(reader, "id_motif");
                    Demande? dem = DB_Convertisseur.Demande(reader, "id_demande");

                    retval = Entree.Creer( dem, motif, descr);
                    retval.Id = id;
                    retval.DateCreation = (DateTime)dateCreation;

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }

        public static int Add(Entree @in)
        {
            string cmdtext = "insert into t_entree(id_entree, date_creation, details, id_motif, id_demande) " +
                                                  "values (@id, @date, @details, @id_motif, @id_demande) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, @in.DateCreation) },
                { "@details",(NpgsqlTypes.NpgsqlDbType.Varchar, @in.Details) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , @in.Id) },
                { "@id_motif",(NpgsqlTypes.NpgsqlDbType.Varchar , @in.Motifs.Id) },
                { "@id_demande",(NpgsqlTypes.NpgsqlDbType.Varchar , @in.Demande.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(Entree @in)
        {

            string cmdtext = "update t_entree set " +
                "details = @details " +
                "id_demande = @id_demande " +
                "id_motif = @id_motif " +
                "where id_entree = @id ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                
                { "@details",(NpgsqlTypes.NpgsqlDbType.Varchar, @in.Details) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , @in.Id) },
                { "@id_motif",(NpgsqlTypes.NpgsqlDbType.Varchar , @in.Motifs.Id) },
                { "@id_demande",(NpgsqlTypes.NpgsqlDbType.Varchar , @in.Demande.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(Entree @in)
        {
            string cmdtext = "delete from t_entree where id_entree = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, @in.Id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }
    public class DB_Sortie
    {
        public static Dictionary<string, Sortie> All_From_Db()
        {
            Dictionary<string, Sortie> retval = new Dictionary<string, Sortie>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_sortie " +
                                                           $"order by id_sortie ",
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id_sortie");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string descr = DB_Convertisseur.String(reader, "details");
                    MotifSortie? motif = DB_Convertisseur.MotifSortie(reader, "id_motif");
                    Demande? dem = DB_Convertisseur.Demande(reader, "id_demande");

                    Sortie tpe = Sortie.Creer( dem, motif, descr);
                    tpe.Id = id;
                    tpe.DateCreation = (DateTime)dateCreation;
                    retval.Add(tpe.Id, tpe);

                    if (AllSortie.Find(tpe.Id) == null)
                    {
                        AllSortie.Add(tpe);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Sortie? UnSortieById(Sortie @out)
        {
            Sortie? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select *" +
                                                           $"from t_sortie " +
                                                           $"where id_sortie = @id ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = @out.Id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string id = DB_Convertisseur.String(reader, "id_sortie");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string descr = DB_Convertisseur.String(reader, "details");
                    MotifSortie? motif = DB_Convertisseur.MotifSortie(reader, "id_motif");
                    Demande? dem = DB_Convertisseur.Demande(reader, "id_demande");

                    retval = Sortie.Creer(dem, motif, descr);
                    retval.Id = id;
                    retval.DateCreation = (DateTime)dateCreation;

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }

        public static int Add(Sortie @out)
        {
            string cmdtext = "insert into t_sortie(id_sortie, date_creation, details, id_motif, id_demande) " +
                                                  "values (@id, @date, @details, @id_motif, @id_demande) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, @out.DateCreation) },
                { "@details",(NpgsqlTypes.NpgsqlDbType.Varchar, @out.Details) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , @out.Id) },
                { "@id_motif",(NpgsqlTypes.NpgsqlDbType.Varchar , @out.Motifs.Id) },
                { "@id_demande",(NpgsqlTypes.NpgsqlDbType.Varchar , @out.Demande.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(Sortie @out)
        {

            string cmdtext = "update t_sortie set " +
                "details = @details " +
                "id_demande = @id_demande " +
                "id_motif = @id_motif " +
                "where id_sortie = @id ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {

                { "@details",(NpgsqlTypes.NpgsqlDbType.Varchar, @out.Details) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , @out.Id) },
                { "@id_motif",(NpgsqlTypes.NpgsqlDbType.Varchar , @out.Motifs.Id) },
                { "@id_demande",(NpgsqlTypes.NpgsqlDbType.Varchar , @out.Demande.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(Sortie @out)
        {
            string cmdtext = "delete from t_sortie where id_sortie = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, @out.Id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }
}
