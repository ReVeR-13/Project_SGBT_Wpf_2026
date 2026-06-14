using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;
using Npgsql;


namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public class DB_Demande
    {
        public static Dictionary<string, Demande> All_From_Db() 
        {
            Dictionary<string, Demande> retval = new Dictionary<string, Demande>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_demande " +
                                                           $"order by id_demande ",
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id_demande");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_ouverture");
                    DateTime? dateFet = DB_Convertisseur.Date(reader, "date_fermeture");
                    string detail = DB_Convertisseur.String(reader, "details");
                    Contact? contact = DB_Convertisseur.Contact(reader, "id_contact");
                    ETypeDemande? type = DB_Convertisseur.TypeDemande(reader, "type");
                    EStatutDemande? statut = DB_Convertisseur.StatutDemande(reader, "statut");
                    Animal? animal = DB_Convertisseur.Animal(reader, "id_animal");

                    if (animal != null && contact != null && type != null && statut != null)
                    {
                        Demande tpe = Demande.Creer(contact, animal,(ETypeDemande)type, detail);
                        tpe.Id = id;
                        tpe.DateCreation = (DateTime)dateCreation;
                        tpe.Statut = (EStatutDemande)statut;
                        retval.Add(tpe.Id, tpe);

                        if (AllDemande.Find(id) == null)
                        {
                            AllDemande.Add(tpe);
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
        public static Demande? UnDemandeById(Demande demande) 
        {
            Demande? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_demande " +
                                                           $"where id_demande = @id ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = demande.Id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string id = DB_Convertisseur.String(reader, "id_demande");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_ouverture");
                    DateTime? dateFet = DB_Convertisseur.Date(reader, "date_fermeture");
                    string detail = DB_Convertisseur.String(reader, "details");
                    Contact? contact = DB_Convertisseur.Contact(reader, "id_contact");
                    ETypeDemande? type = DB_Convertisseur.TypeDemande(reader, "type");
                    EStatutDemande? statut = DB_Convertisseur.StatutDemande(reader, "statut");
                    Animal? animal = DB_Convertisseur.Animal(reader, "id_animal");

                    if (contact != null && type != null && statut != null && animal != null)
                    {
                        retval = Demande.Creer(contact, animal,(ETypeDemande)type, detail);
                        retval.Id = id;
                        retval.Statut = (EStatutDemande)statut;
                        retval.DateCreation = (DateTime)dateCreation;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Dictionary<string, Demande> AllDemandeByContact(Contact contact) 
        {
            Dictionary<string, Demande> retval = new Dictionary<string, Demande>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_demande " +
                                                           $"where id_contact = @id",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = contact.Id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {
                    string id = DB_Convertisseur.String(reader, "id_demande");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_ouverture");
                    DateTime? dateFet = DB_Convertisseur.Date(reader, "date_fermeture");
                    string detail = DB_Convertisseur.String(reader, "details");
                    Contact? fcontact = DB_Convertisseur.Contact(reader, "id_contact");
                    ETypeDemande? type = DB_Convertisseur.TypeDemande(reader, "type");
                    EStatutDemande? statut = DB_Convertisseur.StatutDemande(reader, "type");
                    Animal? animal = DB_Convertisseur.Animal(reader, "id_animal");

                    if (fcontact != null && type != null && statut != null && animal != null)
                    {
                        Demande tpe = Demande.Creer(fcontact,animal ,(ETypeDemande)type, detail);
                        tpe.Id = id;
                        tpe.DateCreation = (DateTime)dateCreation;
                        tpe.Update((EStatutDemande)statut);
                        retval.Add(tpe.Id, tpe);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }

        public static int Add(Demande demande) 
        {
            string cmdtext = "insert into t_demande (id_demande, date_ouverture, date_fermeture, details, id_contact, type , statut ,id_animal) " +
                             "values (@id, @date, @datef, @detail,@id_cont ,@type::e_type_demande, @statut::e_statut_demande, @animal) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, demande.DateCreation) },
                { "@datef",(NpgsqlTypes.NpgsqlDbType.Date, demande.DateFermeture) },
                { "@animal",(NpgsqlTypes.NpgsqlDbType.Varchar, demande.Animal.Id) },
                { "@detail",(NpgsqlTypes.NpgsqlDbType.Varchar, demande.Remarque) },
                { "@id_cont",(NpgsqlTypes.NpgsqlDbType.Varchar, demande.Contact.Id) },
                { "@type",(NpgsqlTypes.NpgsqlDbType.Varchar, demande.Type.ToString()) },
                { "@statut",(NpgsqlTypes.NpgsqlDbType.Varchar, demande.Statut.ToString()) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , demande.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(Demande demande) 
        {

            string cmdtext = "update t_demande set date_fermeture = @datef, details = @detail , id_contact = @id_cont , " +
                             "type = @type::e_type_demande, statut = @statut::e_statut_demande , id_animal = @animal" +
                             " where id_demande = @id ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@datef",(NpgsqlTypes.NpgsqlDbType.Date, demande.DateFermeture) },
                { "@detail",(NpgsqlTypes.NpgsqlDbType.Varchar, demande.Remarque) },
                { "@animal",(NpgsqlTypes.NpgsqlDbType.Varchar, demande.Animal.Id) },
                { "@id_cont",(NpgsqlTypes.NpgsqlDbType.Varchar, demande.Contact.Id) },
                { "@type",(NpgsqlTypes.NpgsqlDbType.Varchar, demande.Type.ToString()) },
                { "@statut",(NpgsqlTypes.NpgsqlDbType.Varchar, demande.Statut.ToString()) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , demande.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(Demande demande) 
        {
            string cmdtext = "delete from t_demande where id_demande = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, demande.Id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }
}
