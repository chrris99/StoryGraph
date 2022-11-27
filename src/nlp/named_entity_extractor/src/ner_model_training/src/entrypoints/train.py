from logging import getLogger

import click
from ner_model_training.src.trainer.trainer import TrainClassifier
from ner_model_training.src.utils.config import from_yaml_auto, to_dict

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
