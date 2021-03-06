using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProfileStats.Api.Services
{
    public class RedisService
    {
        private readonly IConnectionMultiplexer _multiplexer;
        private readonly IConfiguration _config;
        private IDatabase _db;

        public RedisService(IConnectionMultiplexer multiplexer, IConfiguration configuration)
        {
            _multiplexer = multiplexer;
            _config = configuration;
        }

        private IServer GetServer() => _multiplexer.GetServer($"{_config["RedisServer"]}:{_config["RedisPort"]}");

        public void UseDatabse(RedisDatabase db)
        {
            _db = _multiplexer.GetDatabase((int)db);
        }

        public async Task<bool> KeyExists(string key) => await _db.KeyExistsAsync(key);

        public async Task<T> GetObject<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default;
            }
            var res = await _db.StringGetAsync(key);
            if (res == RedisValue.Null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(res);
        }

        public async Task<string> GetString(string key) => await _db.StringGetAsync(key);

        public void SetObject(string key, object value, TimeSpan? expiry = null) => _db.StringSet(key, JsonSerializer.Serialize(value), expiry, flags: CommandFlags.FireAndForget);

        public void SetString(string key, string value, TimeSpan? expiry = null) => _db.StringSet(key, value, expiry, flags: CommandFlags.FireAndForget);

        public async Task DeleteKey(string key) => await _db.KeyDeleteAsync(key);

        public List<string> GetKeys() => GetServer().Keys(_db.Database, "*").Select(k => k.ToString()).ToList();

        public async Task FlushDb() => await GetServer().FlushDatabaseAsync(_db.Database);
    }

    public enum RedisDatabase
    {
        NameIds,
        Responses,
        Stats,
        Misc,
    }
}
