from typing import Dict, Any

import attr
import yaml


def from_yaml_auto(yaml_path: str):
    with open(yaml_path) as f:
        data = yaml.load(f.read(), Loader=yaml.FullLoader)
        return from_dict_auto(data)


def from_yaml(yaml_path: str, config_class):
    with open(yaml_path) as f:
        data = yaml.load(f.read(), Loader=yaml.FullLoader)
        return from_dict(data, config_class)


def from_dict_auto(data: dict):
    if data['name'] == "ner":
        return from_dict(data, NerConfig)
    else:
        raise ValueError(f"Unsupported task type: {data['name']}")


def from_dict(data: dict, config_class):
    non_opt_names = set(x.name for x in config_class.__attrs_attrs__)

    opt_data, non_opt_data = dict(), dict()
    for key, value in data. items():
        if key in non_opt_names:
            non_opt_data[key] = value
        else:
            opt_data[key] = value

    config = config_class(**non_opt_data)
    for key, value in opt_data.items():
        config.__setattr__(key, value)

    return config


def to_dict(config: Any) -> Dict[str, Any]:
    return {attr_name: config.__getattribute__(attr_name)
            for attr_name in dir(config)
            if not (attr_name.startswith('__') and attr_name.endswith('__'))}


@attr.s(auto_attribs=True)
class ExperimentParameters:
    name: str
    lang: str
    version: float


@attr.s(auto_attribs=True)
class TextDataParameters:
    max_seq_len: int
    word_count_thresh: int
    num_classes: int
    embeddings_dim: int
    embeddings_name: str


@attr.s(auto_attribs=True)
class TokenDataParameters(TextDataParameters):
    max_word_len: int
    char_embeddings_dim: int


@attr.s(auto_attribs=True)
class TrainingParameters:
    num_epochs: int
    batch_size: int
    learning_rate: float
    es_patience: int


@attr.s(auto_attribs=True)
class NerConfig(ExperimentParameters, TokenDataParameters, TrainingParameters):
    pass
