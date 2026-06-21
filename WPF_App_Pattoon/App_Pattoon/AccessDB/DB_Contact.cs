using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;
using Npgsql;

namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public static class DB_Contact
    {
        public static Dictionary<string, Contact> All_From_Db()
        {
            Dictionary<string, Contact> retval = new Dictionary<string, Contact>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_contact " +
                                                           $"order by id_contact ",
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id_contact");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string nom = DB_Convertisseur.String(reader, "nom_contact");
                    string prenom = DB_Convertisseur.String(reader, "prenom_contact");
                    DateTime datenaiss = (DateTime)DB_Convertisseur.Date(reader, "date_naissance");
                    string niss = DB_Convertisseur.String(reader, "niss");
                    string gsm = DB_Convertisseur.String(reader, "gsm");
                    string tel = DB_Convertisseur.String(reader, "tel");
                    string mail = DB_Convertisseur.String(reader, "mail");
                    string cp = DB_Convertisseur.String(reader, "code_postale");
                    string localite = DB_Convertisseur.String(reader, "localité");
                    string adresse = DB_Convertisseur.String(reader, "adresse");

                    Contact tpe = Contact.Creer(niss, datenaiss, nom, prenom, gsm, tel, mail, cp, localite, adresse);
                    tpe.Id = id;
                    tpe.DateCreation = (DateTime)dateCreation;
                    retval.Add(tpe.Id, tpe);

                    if (AllContacts.Find(id) == null)
                    {
                        AllContacts.Add(tpe);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Contact? UnContactById(string id)
        {
            Contact? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_contact " +
                                                           $"where id_contact = @id ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string nom = DB_Convertisseur.String(reader, "nom_contact");
                    string prenom = DB_Convertisseur.String(reader, "prenom_contact");
                    DateTime datenaiss = (DateTime)DB_Convertisseur.Date(reader, "date_naissance");
                    string niss = DB_Convertisseur.String(reader, "niss");
                    string gsm = DB_Convertisseur.String(reader, "gsm");
                    string tel = DB_Convertisseur.String(reader, "tel");
                    string mail = DB_Convertisseur.String(reader, "mail");
                    string cp = DB_Convertisseur.String(reader, "code_postale");
                    string localite = DB_Convertisseur.String(reader, "localité");
                    string adresse = DB_Convertisseur.String(reader, "adresse");

                    retval = Contact.Creer(niss, datenaiss, nom, prenom, gsm, tel, mail, cp, localite, adresse);
                    retval.Id = id;
                    retval.DateCreation = (DateTime)dateCreation;

                    if (AllContacts.Find(Forma.TrimUpper(id)) == null)
                    {
                        AllContacts.Add(retval);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Contact? UnContactByGsm(string gsm)
        {
            Contact? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_contact " +
                                                           $"where gsm = @gsm ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@gsm", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@gsm"].Value = gsm;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string nom = DB_Convertisseur.String(reader, "nom_contact");
                    string prenom = DB_Convertisseur.String(reader, "prenom_contact");
                    DateTime datenaiss = (DateTime)DB_Convertisseur.Date(reader, "date_naissance");
                    string niss = DB_Convertisseur.String(reader, "niss");
                    string id = DB_Convertisseur.String(reader, "id_contact");
                    string tel = DB_Convertisseur.String(reader, "tel");
                    string mail = DB_Convertisseur.String(reader, "mail");
                    string cp = DB_Convertisseur.String(reader, "code_postale");
                    string localite = DB_Convertisseur.String(reader, "localité");
                    string adresse = DB_Convertisseur.String(reader, "adresse");

                    retval = Contact.Creer(niss, datenaiss, nom, prenom, gsm, tel, mail, cp, localite, adresse);
                    retval.Id = id;
                    retval.DateCreation = (DateTime)dateCreation;

                    if (AllContacts.Find(Forma.TrimUpper(nom)) == null)
                    {
                        AllContacts.Add(retval);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }

        public static int Add(Contact contacts)
        {
            string cmdtext = "insert into t_contact " +
                             "(id_contact, date_creation, nom_contact, prenom_contact,date_naissance, niss, gsm, tel, mail, code_postale, localité, adresse) " +
                             "values " +
                             "(@id_contact, @date_creation, @nom_contact, @prenom_contact, @date_naissance, @niss, @gsm, @tel, @mail, @code_postale, @localité, @adresse) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id_contact",(NpgsqlTypes.NpgsqlDbType.Varchar, contacts.Id) },
                { "@date_creation",(NpgsqlTypes.NpgsqlDbType.Date, contacts.DateCreation) },
                { "@nom_contact",(NpgsqlTypes.NpgsqlDbType.Varchar, contacts.Nom) },
                { "@prenom_contact",(NpgsqlTypes.NpgsqlDbType.Varchar, contacts.Prenom) },
                { "@date_naissance",(NpgsqlTypes.NpgsqlDbType.Date, contacts.DateNaissance) },
                { "@niss",(NpgsqlTypes.NpgsqlDbType.Varchar, contacts.Niss) },
                { "@gsm",(NpgsqlTypes.NpgsqlDbType.Varchar, contacts.Gsm) },
                { "@tel",(NpgsqlTypes.NpgsqlDbType.Varchar, contacts.Telephone) },
                { "@mail",(NpgsqlTypes.NpgsqlDbType.Varchar, contacts.Mail) },
                { "@code_postale",(NpgsqlTypes.NpgsqlDbType.Varchar, contacts.CodePostal) },
                { "@localité",(NpgsqlTypes.NpgsqlDbType.Varchar , contacts.Localite) },
                { "@adresse",(NpgsqlTypes.NpgsqlDbType.Varchar , contacts.Adresse) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(Contact contacts)
        {

            string cmdtext = "update t_contact set " +
                             "nom_contact = @nom_contact, " +
                             "prenom_contact = @prenom_contact," +
                             "date_naissance = @date_naissance, " +
                             "niss = @niss, " +
                             "gsm = @gsm, " +
                             "tel = @tel , " +
                             "mail = @mail, " +
                             "code_postale = @code_postale, " +
                             "localité = @localité, " +
                             "adresse = @adresse " +
                             "where id_contact = @id ";
            
            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date_creation",(NpgsqlTypes.NpgsqlDbType.Date, contacts.DateCreation) },
                { "@nom_contact",(NpgsqlTypes.NpgsqlDbType.Varchar, contacts.Nom) },
                { "@prenom_contact",(NpgsqlTypes.NpgsqlDbType.Varchar, contacts.Prenom) },
                { "@date_naissance",(NpgsqlTypes.NpgsqlDbType.Date, contacts.DateNaissance) },
                { "@niss",(NpgsqlTypes.NpgsqlDbType.Varchar, contacts.Niss) },
                { "@gsm",(NpgsqlTypes.NpgsqlDbType.Varchar, contacts.Gsm) },
                { "@tel",(NpgsqlTypes.NpgsqlDbType.Varchar, contacts.Telephone) },
                { "@mail",(NpgsqlTypes.NpgsqlDbType.Varchar, contacts.Mail) },
                { "@code_postale",(NpgsqlTypes.NpgsqlDbType.Varchar, contacts.CodePostal) },
                { "@localité",(NpgsqlTypes.NpgsqlDbType.Varchar , contacts.Localite) },
                { "@adresse",(NpgsqlTypes.NpgsqlDbType.Varchar , contacts.Adresse) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, contacts.Id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(string id)
        {
            string cmdtext = "delete from t_contact where id_contact = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }

    }
}
