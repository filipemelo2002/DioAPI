using System;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using DioAPI.Data.Collections;

namespace DioAPI.Data
{
    public class MongoDB
    {
        public IMongoDatabase DB {get;}

        public MongoDB(IConfiguration configuration) {
            try {   
                var settings = MongoClientSettings.FromUrl(new MongoUrl(configuration["ConnectionString"]));
                var client  = new MongoClient(settings);
                DB = client.GetDatabase(configuration["NomeBanco"]);
                MapClasses();
            } catch (Exception ex) {
                throw new MongoException("It was not possible to connect to MongDB", ex);
            }
        }

        private void MapClasses(){
            var convetionPack = new ConventionPack {new CamelCaseElementNameConvention()};
            ConventionRegistry.Register("camelCase", convetionPack, t => true);

            if(!BsonClassMap.IsClassMapRegistered(typeof(Infectado))){
                BsonClassMap.RegisterClassMap<Infectado>(i => {
                    i.AutoMap();
                    i.SetIgnoreExtraElements(true);
                });
            }
        }
    }
}