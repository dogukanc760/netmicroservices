# netmicroservices
Docker kurulumunu yaptıktan sonra sırasıyla aşağıya bu kod bloklarını yapıştırıyoruz
#### docker volume create portainer_data
#### docker run -d -p 9000:9000 -v /var/run/docker.sock:/var/run/docker.sock:z portainer/portainer

#### localhost:9000 diyerek portainer açıyoruz

daha sonra portainer kullanıcısını ayarlayarak içeriden MongoDb kurulumu yapıyoruz ve
Test için MongoDb Compass GUI indirip test edebilirsiniz..
