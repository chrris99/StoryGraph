import click

from ner_model_training.src.data_preparation.ner_data_preparator import NerDataPreparation


@click.command()
@click.argument("config_file")
def main(config_file):
    data_prep = NerDataPreparation.create_from_config(config_file)
    data_prep.run()


if __name__ == "__main__":
    main()
