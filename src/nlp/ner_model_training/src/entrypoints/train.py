import click
from logging import getLogger
from src.utils.config import from_yaml_auto, to_dict
from src.trainer.trainer import TrainClassifier

logger = getLogger(__name__)


@click.command()
@click.argument("config_file")
def main(config_file):
    config = from_yaml_auto(config_file)

    print(f"Config used by TrainClassifier: {to_dict(config)}")

    classifier = TrainClassifier(config, logger)
    classifier('train')


if __name__ == '__main__':
    main()
