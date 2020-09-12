# Kafka.Lens

The 'Kafka.Lens' application checks Kafka cluster status (connectivity, possibility to send and receive messages, brokers status)
 and a Mongo DB status (connectivity)

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

1. Install [.NETCore 3.1 Runtime](https://dotnet.microsoft.com/download/dotnet-core/3.1)
2. Download [current prerelease v.0.1](https://github.com/antkott/cSharp/releases/tag/v0.1) (currently available only for win-64 system)
3. Unzip Kafka.Lens.zip and fill in appSettings.json with your Kafka & Mongo parameters
4. Run console application Kafka.Lens.Runner.exe 


## Deployment & build
![Kafka.Lens build & publish](https://github.com/antkott/cSharp/workflows/Kafka.Lens%20build%20&%20publish/badge.svg)

## Releases

See [Releases](https://github.com/antkott/cSharp/releases) page for details

## Authors

* **Anton V. Kotliarenko** - *Initial work* - [AntKott](https://github.com/antkott)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details