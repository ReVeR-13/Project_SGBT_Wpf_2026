using Wpf_App_Pattoon_Animalerie.Modele;
using Wpf_App_Pattoon_Animalerie.Service;
using Npgsql;

namespace Wpf_App_Pattoon_Animalerie.AccessDB
{
    public static class DB_TypeAnimal
    {
        public static Dictionary<string, TypeAnimal> AllTypesAnimal()
        {
            Dictionary<string, TypeAnimal> retval = new();
            
            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * from t_type_animal " +
                $"order by id_type",
                AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string? id = DB_Convertisseur.String(reader, "id_type");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string? nom = DB_Convertisseur.String(reader, "nom_type");
                    string? descr = DB_Convertisseur.String(reader, "description");

                    TypeAnimal tpe = TypeAnimal.Creer(nom, descr);
                    tpe.Id = id;
                    tpe.DateCreation = (DateTime)dateCreation;
                    retval.Add(tpe.Id, tpe);

                    if (AllTypeAnimal.FindTypeByNom(nom) == null)
                    {
                        AllTypeAnimal.Add(tpe);
                         
                    }

                }
            }catch(Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static TypeAnimal? UnTypesAnimalById(string id)
        {
            TypeAnimal? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * from t_type_animal " +
                $"where id_type = @id",
                AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {
                    string idty = DB_Convertisseur.String(reader, "id_type");
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date_creation");
                    string nom = DB_Convertisseur.String(reader, "nom_type");
                    string descr = DB_Convertisseur.String(reader, "description");

                    if (AllTypeAnimal.FindTypeByNom(Forma.TrimUpper(nom)) == null)
                    {
                        retval = TypeAnimal.Creer(nom, descr);
                        retval.Id = idty;
                        retval.DateCreation = dateCreation;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static TypeAnimal? UnTypesAnimalByNom(string nom)
        {
            TypeAnimal? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * from t_type_animal " +
                $"where nom_type = @nom",
                AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@nom", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@nom"].Value = nom;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {
                    string idty = DB_Convertisseur.String(reader, "id_type");
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date_creation");
                    string typnom = DB_Convertisseur.String(reader, "nom_type");
                    string descr = DB_Convertisseur.String(reader, "description");

                    if (AllTypeAnimal.FindTypeByNom(Forma.TrimUpper(typnom)) == null)
                    {
                        retval = TypeAnimal.Creer(typnom, descr);
                        retval.Id = idty;
                        retval.DateCreation = dateCreation;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }

        public static int Add(TypeAnimal type)
        {
            string cmdtext = "insert into t_type_animal(id_type, date_creation, nom_type, description ) values (@id, @date, @nom, @descr) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, type.DateCreation) },
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, type.Nom) },
                { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar, type.Description) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , type.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(TypeAnimal type)
        {

            string cmdtext = "update t_type_animal set nom_type = @nom, description = @descr where id_type = @id ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, type.Nom) },
                { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar, type.Description) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , type.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(TypeAnimal type)
        {
            string cmdtext = "delete from t_type_animal where id_type = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, type.Id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }

    }
    public static class DB_TypeContact
    {
        public static Dictionary<string, TypeContact> All_From_Db()
        {
            Dictionary<string, TypeContact> retval = new Dictionary<string, TypeContact>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand(DB_Convertisseur.SelectFrom("f_All_type_contact()"),
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id              = DB_Convertisseur.String(reader, "id");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date");
                    string nom             = DB_Convertisseur.String(reader, "nom");
                    string descr           = DB_Convertisseur.String(reader, "descri");

                    TypeContact tpe = TypeContact.Creer(nom, descr);
                    tpe.Id = id;
                    tpe.DateCreation = (DateTime)dateCreation;
                    retval.Add(tpe.Nom, tpe);

                    if (AllTypeContact.FindByNom(nom) == null)
                    {
                        AllTypeContact.Add(tpe);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static TypeContact? UnTypesContactById(string id)
        {
            TypeContact? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand(DB_Convertisseur.SelectFrom("f_One_type_contact(@id)"),
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string idty           = DB_Convertisseur.String(reader, "id");
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date");
                    string nom            = DB_Convertisseur.String(reader, "nom");
                    string descr          = DB_Convertisseur.String(reader, "descri");

                    retval = TypeContact.Creer(nom, descr);
                    retval.Id = idty;
                    retval.DateCreation = dateCreation;

                    if (AllTypeContact.FindByNom(Forma.TrimUpper(nom)) == null)
                    {
                        AllTypeContact.Add(retval);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static TypeContact? UnTypesContactByNom(string nom)
        {
            TypeContact? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select * from t_type_contact " +
                                                           $"where libele = @nom",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@nom", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@nom"].Value = nom;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string idty = DB_Convertisseur.String(reader, "id_type");
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date_creation");
                    string typnom = DB_Convertisseur.String(reader, "libele");
                    string descr = DB_Convertisseur.String(reader, "description");

                    retval = TypeContact.Creer(typnom, descr);
                    retval.Id = idty;
                    retval.DateCreation = dateCreation;

                    if (AllTypeContact.FindByNom(Forma.TrimUpper(typnom)) == null)
                    {
                        AllTypeContact.Add(retval);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }

        public static int Add(TypeContact type)
        {
            string cmdtext = DB_Convertisseur.SelectFrom("f_create_type_contact(@nom, @descr)");

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, type.Nom) },
                { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar, type.Description) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(TypeContact type)
        {

            string cmdtext = "update t_type_contact set libele = @nom, description = @descr where id_type = @id ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, type.Nom) },
                { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar, type.Description) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , type.Id) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(string id)
        {
            string cmdtext = DB_Convertisseur.SelectFrom("f_delete_type_contact(@id)");

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }    

    }
    
}
