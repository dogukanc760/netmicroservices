using StackExchange.Redis;

namespace FreeCourse.Services.Basket.Services
{
    public class RedisService
    {

        private readonly string _host;
        private readonly int _port;

        private ConnectionMultiplexer _connectionMultiplexer;



        public RedisService(string host, int port /*, ConnectionMultiplexer connectionMultiplexer*/)
        {
            _host = host;
            _port = port;
            //_connectionMultiplexer = connectionMultiplexer;

        }


        public void Connect() => _connectionMultiplexer = ConnectionMultiplexer.Connect($"{_host}:{_port}");

        public IDatabase GetDB(int db = 1) => _connectionMultiplexer.GetDatabase(db);

    }
}
