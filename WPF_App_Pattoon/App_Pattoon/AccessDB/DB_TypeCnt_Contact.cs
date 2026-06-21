using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;
using Npgsql;

namespace Wpf_App_Pattoon_Animalerie.AccessDB
{

    public class DB_TypeCnt_Contact
    {
        public static Dictionary<string, TypeContact_Contact> AllRoles()
        {
            Dictionary<string, TypeContact_Contact> retval = new Dictionary<string, TypeContact_Contact>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id_ct_tpecont, date_creation, id_contact, id_type_contact " +
                                                           $"from t_contact_typecontact " +
                                                           $"order by id_ct_tpecont ",
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id             = DB_Convertisseur.String(reader, "id_ct_tpecont");
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date_creation");
                    Contact? cnt          = AllContacts.Find(DB_Convertisseur.String(reader, "id_contact"));
                    TypeContact? type     = AllTypeContact.FindById(DB_Convertisseur.String(reader, "id_type_contact"));

                    TypeContact_Contact relation = TypeContact_Contact.Creer(cnt, type);
                    relation.Id = id;
                    relation.DateCreation = dateCreation;

                    retval.Add(id, relation);

                    if (AllTypeContact_Contact.Find(relation.Id) == null)
                    {
                        AllTypeContact_Contact.Add(relation);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Dictionary<string, TypeContact_Contact> AllRoles(Contact contact)
        {
            Dictionary<string, TypeContact_Contact> retval = new();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_type_contact " +
                                                           $"where id_contact = @id " +
                                                           $"order by id_ct_tpecont",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = contact.Id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {
                    string id             = DB_Convertisseur.String(reader, "id_ct_tpecont");
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date_creation");
                    Contact? cnt         = AllContacts.Find(DB_Convertisseur.String(reader, "id_contact"));
                    TypeContact? type     = AllTypeContact.FindById(DB_Convertisseur.String(reader, "id_type_contact"));

                    TypeContact_Contact relation = TypeContact_Contact.Creer(cnt, type);
                    relation.Id = id;
                    relation.DateCreation = dateCreation;
                    retval.Add(id, relation);

                    if (cnt.GetUnType(type) == null)
                    {
                        AllTypeContact_Contact.Add(relation);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Dictionary<string, TypeContact_Contact> AllRoles(TypeContact type)
        {
            Dictionary<string, TypeContact_Contact> retval = new();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_type_contact " +
                                                           $"where id_type_contact = @id " +
                                                           $"order by id_ct_tpecont",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = type.Id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {
                    string id = DB_Convertisseur.String(reader, "id_ct_tpecont");
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date_creation");
                    Contact? cnt = AllContacts.Find(DB_Convertisseur.String(reader, "id_contact"));
                    TypeContact? tpe = AllTypeContact.FindById(DB_Convertisseur.String(reader, "id_type_contact"));

                    TypeContact_Contact relation = TypeContact_Contact.Creer(cnt, type);
                    relation.Id = id;
                    relation.DateCreation = dateCreation;
                    retval.Add(id, relation);

                    if (cnt.GetUnType(type) == null)
                    {
                        AllTypeContact_Contact.Add(relation);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static TypeContact_Contact? UnRoles(TypeContact_Contact type)
        {
            TypeContact_Contact? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * " +
                                                           $"from t_contact_typecontact " +
                                                           $"where id_ct_tpecont = @id ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = type.Id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string id             = DB_Convertisseur.String(reader, "id_ct_tpecont");
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date_creation");
                    Contact? cnt          = AllContacts.Find(DB_Convertisseur.String(reader, "id_contact"));
                    TypeContact? tpe      = AllTypeContact.FindById(DB_Convertisseur.String(reader, "id_type_contact"));

                    Console.WriteLine(DB_Convertisseur.String(reader, "id_type_contact"));

                    if (cnt == null || tpe == null)
                    {
                        throw new Exception($"[DB_typeContact ContactSelectionne] Valeur retour DB null");
                    }

                    retval = TypeContact_Contact.Creer(cnt, tpe);
                    retval.Id = id;
                    retval.DateCreation = dateCreation;

                    if (cnt.GetUnType(tpe) == null)
                    {
                        AllTypeContact_Contact.Add(retval);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }

        public static int Add(TypeContact_Contact relation)
        {
            string cmdtext = "insert into t_contact_typecontact(id_ct_tpecont, date_creation, id_contact, id_type_contact ) values(@id, @date, @idcnt, @idtpe) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, relation.DateCreation) },
                { "@idtpe",(NpgsqlTypes.NpgsqlDbType.Varchar, relation.Type.Id) },
                { "@idcnt",(NpgsqlTypes.NpgsqlDbType.Varchar, relation.Contact.Id) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , relation.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
        public static int Delete(TypeContact_Contact relation)
        {
            string cmdtext = "delete from t_contact_typecontact where id_ct_tpecont = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, relation.Id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }
}
