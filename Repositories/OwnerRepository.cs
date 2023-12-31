﻿using DogGo.Models;
using Microsoft.Data.SqlClient;

namespace DogGo.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public OwnerRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Owner> GetAllOwners()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT o.Id, 
                        o.[Name],
                        o.Email,
                        o.Phone,
                        o.Address,
                        o.NeighborhoodId,
                        n.[Name] AS HoodName
                        FROM Owner o 
                        JOIN Neighborhood n ON o.NeighborhoodId= n.Id   
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Owner> owners = new List<Owner>();
                    while (reader.Read())
                    {
                        Owner owner = new Owner
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone")),
                            Neighborhood = new Neighborhood
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
                                Name = reader.GetString(reader.GetOrdinal("hoodName"))
                            }
                        };

                        owners.Add(owner);
                    }

                    reader.Close();

                    return owners;
                }
            }
        }

        /*use this version to display dog(s) for each owner*/
        //public Owner GetOwnerById(int id)
        //{
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText =

        //                @"
        //       SELECT o.Id as OwnerId, 
        //        o.[Name] As OwnerName,
        //        o.Email,
        //        o.Phone,
        //        o.Address,
        //        o.NeighborhoodId,
        //        n.[Name] AS HoodName,
        //        d.Id AS DogId,
        //        d.Name AS DogName,
        //        d.Breed,
        //        d.ImageUrl,
        //        d.OwnerId,
        //        d.Notes
        //        FROM Owner o 
        //        JOIN Neighborhood n on o.NeighborhoodId= n.Id
        //        JOIN Dog d on o.Id= d.OwnerId
        //        WHERE o.Id = @id";

        //            cmd.Parameters.AddWithValue("@id", id);

        //            SqlDataReader reader = cmd.ExecuteReader();

        //            Owner owner = null;

        //            while (reader.Read())
        //            {
        //                if (owner == null)
        //                {
        //                    owner = new Owner()
        //                    {
        //                        Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
        //                        Name = reader.GetString(reader.GetOrdinal("OwnerName")),
        //                        Email = reader.GetString(reader.GetOrdinal("Email")),
        //                        Phone = reader.GetString(reader.GetOrdinal("Phone")),
        //                        Address = reader.GetString(reader.GetOrdinal("Address")),
        //                        NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
        //                        Neighborhood = new Neighborhood()
        //                        {
        //                            Id = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
        //                            Name = reader.GetString(reader.GetOrdinal("HoodName"))
        //                        },
        //                        DogList = new List<Dog>()
        //                    };
        //                }

        //                if (!reader.IsDBNull(reader.GetOrdinal("DogId")))
        //                {
        //                    Dog dog = new Dog()
        //                    {
        //                        Id = reader.GetInt32(reader.GetOrdinal("DogId")),
        //                        Name = reader.GetString(reader.GetOrdinal("DogName")),
        //                        Breed = reader.GetString(reader.GetOrdinal("Breed")),
        //                        OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
        //                        ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl"))
        //                                ? null
        //                                : reader.GetString(reader.GetOrdinal("ImageUrl")),
        //                        Notes = reader.IsDBNull(reader.GetOrdinal("Notes"))
        //                                ? null
        //                                : reader.GetString(reader.GetOrdinal("Notes")),
        //                    };

        //                    owner.DogList.Add(dog);
        //                }
        //            }

        //            reader.Close();
        //            return owner;

        //        }
        //    }
        //}


        public Owner GetOwnerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], Email, Address, Phone, NeighborhoodId
                        FROM Owner
                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Owner owner = new Owner()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };

                        reader.Close();
                        return owner;
                    }

                    reader.Close();
                    return null;
                }
            }
        }

        public Owner GetOwnerByEmail(string email)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], Email, Address, Phone, NeighborhoodId
                        FROM Owner
                        WHERE Email = @email";

                    cmd.Parameters.AddWithValue("@email", email);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Owner owner = new Owner()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };

                        reader.Close();
                        return owner;
                    }

                    reader.Close();
                    return null;
                }
            }
        }

        public void AddOwner(Owner owner)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Owner ([Name], Email, Phone, Address, NeighborhoodId)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @email, @phoneNumber, @address, @neighborhoodId);
                ";

                    cmd.Parameters.AddWithValue("@name", owner.Name);
                    cmd.Parameters.AddWithValue("@email", owner.Email);
                    cmd.Parameters.AddWithValue("@phoneNumber", owner.Phone);
                    cmd.Parameters.AddWithValue("@address", owner.Address);
                    cmd.Parameters.AddWithValue("@neighborhoodId", owner.NeighborhoodId);

                    int id = (int)cmd.ExecuteScalar();

                    owner.Id = id;
                }
            }
        }

        public void UpdateOwner(Owner owner)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Owner
                            SET 
                                [Name] = @name, 
                                Email = @email, 
                                Address = @address, 
                                Phone = @phone, 
                                NeighborhoodId = @neighborhoodId
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", owner.Name);
                    cmd.Parameters.AddWithValue("@email", owner.Email);
                    cmd.Parameters.AddWithValue("@address", owner.Address);
                    cmd.Parameters.AddWithValue("@phone", owner.Phone);
                    cmd.Parameters.AddWithValue("@neighborhoodId", owner.NeighborhoodId);
                    cmd.Parameters.AddWithValue("@id", owner.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteOwner(int ownerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Owner
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", ownerId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

    

