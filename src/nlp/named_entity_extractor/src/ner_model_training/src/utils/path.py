import os
from argparse import Namespace

DATA_DIR_PREFIX = os.path.abspath(os.path.join(os.path.dirname(__file__), os.pardir, os.pardir, "data"))


def get_data_path(config: Namespace) -> str:
    return get_local_data_path(config)


def get_local_data_path(config: Namespace) -> str:
    return os.path.join(DATA_DIR_PREFIX, config.name, config.lang, str(config.version))
