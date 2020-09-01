start d:\PRG\SYS\Kafka\kafka\bin\windows\kafka-topics.bat --zookeeper localhost:2181 --delete --topic test
ping localhost
start d:\PRG\SYS\Kafka\kafka\bin\windows\kafka-topics.bat --create --zookeeper localhost:2181 --replication-factor 1 --partitions 5 --topic test
ping localhost
start  d:\PRG\SYS\Kafka\kafka\bin\windows\kafka-console-producer.bat --broker-list localhost:9092 --topic test
pause